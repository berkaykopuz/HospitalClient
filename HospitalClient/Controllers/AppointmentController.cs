using HospitalClient.Data;
using HospitalClient.Models;
using HospitalClient.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace HospitalClient.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly HttpClient _httpClient;
        private AppointmentViewModel _viewModel;

        public AppointmentController()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7029")
            };

            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(
            new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json")
            );

            _viewModel = new AppointmentViewModel();
        }
        public IActionResult Index()
        {
            var role = HttpContext.Session.GetString("role");
            if (role == null)
            {
                return RedirectToAction("AccessDenied", "Home");
            }
            if (!role.Equals("User"))
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            return View();
        }

        public async Task<IActionResult> Create(AppointmentViewModel viewModel)
        {
            var role = HttpContext.Session.GetString("role");
            if (role == null)
            {
                return RedirectToAction("AccessDenied", "Home");
            }
            if (!role.Equals("User"))
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            _viewModel = viewModel;
            

            if (_viewModel.SelectedClinicId != 0) // select clinic for getting hospitals
            {

                HttpResponseMessage hospitalResponse = await _httpClient.GetAsync("/api/HospitalClinic/GetHospitalsByClinicId/" + _viewModel.SelectedClinicId);

                if(hospitalResponse.IsSuccessStatusCode)
                {
                    var jsonString = await hospitalResponse.Content.ReadAsStringAsync();
                    var hospitals = JsonSerializer.Deserialize<List<Hospital>>(jsonString);

                    _viewModel.Hospitals = hospitals;

                    return View(_viewModel);
                }
            }

            if (_viewModel.SelectedHospitalId != 0) // select hospital for getting doctors
            {
                HttpResponseMessage doctorResponse = await _httpClient.GetAsync("/api/Doctor/GetDoctorsByHospitalId/" + _viewModel.SelectedHospitalId);

                if (doctorResponse.IsSuccessStatusCode)
                {
                    var jsonString = await doctorResponse.Content.ReadAsStringAsync();
                    var doctors = JsonSerializer.Deserialize<List<Doctor>>(jsonString);

                    _viewModel.Doctors = doctors;

                    return View(_viewModel);
                }
            }

            if(_viewModel.SelectedDoctorId != 0) // last step, doctor is chosen so can create appointment
            {
                var jwt = HttpContext.Session.GetString("token");
                var citizenId = JwtHelper.DecodeCitizenJwtToken(jwt, Secret.SecretKey);

                AppointmentRequest request = new AppointmentRequest
                {
                    date = _viewModel.Date,
                    citizenId = citizenId,
                    doctorId = _viewModel.SelectedDoctorId
                };

                HttpResponseMessage timingResponse = await _httpClient.GetAsync("/api/Timing?doctorId=" + request.doctorId); //checking for doctor work hours
                if(timingResponse.IsSuccessStatusCode)
                {
                    var jsonString = await timingResponse.Content.ReadAsStringAsync();
                    var timings = JsonSerializer.Deserialize<List<Timing>>(jsonString);
                    bool control = false;

                    foreach(var timing in timings)
                    {
                        if(timing.WorkDay.ToString("yyyy-MM-dd") == request.date.ToString("yyyy-MM-dd"))
                        {
                            control = true;
                            if(request.date.Hour < timing.ShiftStart || request.date.Hour >= timing.ShiftEnd)
                            {
                                return View("NotAvailable");
                            }
                        }
                    }

                    if (!control)
                    {
                        return View("NotAvailable");
                    }
                }

                HttpResponseMessage isTakenResponse = await _httpClient.PostAsJsonAsync("/api/Appointment/istaken", request);
                if(isTakenResponse.IsSuccessStatusCode)
                {
                    var jsonString = await isTakenResponse.Content.ReadAsStringAsync();
                    var isTaken = JsonSerializer.Deserialize<bool>(jsonString);

                    if (isTaken)
                    {
                        return View("AlreadyTaken");
                    }
                }

                HttpResponseMessage appointmentResponse = await _httpClient.PostAsJsonAsync("api/Appointment/create", request);


                if (appointmentResponse.IsSuccessStatusCode)
                {
                    Console.WriteLine("Appointment başarıyla oluşturuldu!");
                    return RedirectToAction("Index", "Home");
                }

                return View(appointmentResponse);
            }

            HttpResponseMessage clinicResponse = await _httpClient.GetAsync("/api/Clinic");

            if (clinicResponse.IsSuccessStatusCode)
            {
                var jsonString = await clinicResponse.Content.ReadAsStringAsync();
                var clinics = JsonSerializer.Deserialize<List<Clinic>>(jsonString);
                
                _viewModel.Clinics = clinics;

                ModelState.Clear();

                return View(_viewModel);

            }

            ModelState.Clear();

            return View(_viewModel);
        }

        public async Task<IActionResult> UserAppointments()
        {
            var role = HttpContext.Session.GetString("role");
            if (role == null)
            {
                return RedirectToAction("AccessDenied", "Home");
            }
            if (!role.Equals("User"))
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            var jwt = HttpContext.Session.GetString("token");
            var citizenId = JwtHelper.DecodeCitizenJwtToken(jwt, Secret.SecretKey);

            HttpResponseMessage appointmentResponse = await _httpClient.GetAsync("/api/Appointment/getbyuserid?id=" + citizenId);
            if(appointmentResponse.IsSuccessStatusCode)
            {
                var jsonString = await appointmentResponse.Content.ReadAsStringAsync();
                var appointments = JsonSerializer.Deserialize<List<Appointment>>(jsonString);

                return View(appointments);
            }

            return RedirectToAction("Index", "Home"); // should be not found here
        }

        public async Task<IActionResult> Delete(int id)
        {
            var role = HttpContext.Session.GetString("role");
            if (role == null)
            {
                return RedirectToAction("AccessDenied", "Home");
            }
            if (!role.Equals("Admin"))
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            HttpResponseMessage appointmentResponse = await _httpClient.DeleteAsync("/api/Appointment/delete/" + id);
            if (appointmentResponse.IsSuccessStatusCode)
            {
                Console.WriteLine(appointmentResponse.Content);
                return RedirectToAction("Index");
            }

            return View("Index", "Home"); //should be deleted 

        }


    }
}
