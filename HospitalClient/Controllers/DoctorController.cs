using HospitalClient.Data;
using HospitalClient.Models;
using HospitalClient.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace HospitalClient.Controllers
{
    public class DoctorController : Controller
    {
        private readonly HttpClient _httpClient;

        public DoctorController()
        {

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7029")
            };

            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(
            new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json")
            );
        }

        

        public async Task<IActionResult> Index()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("api/Doctor");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var doctors = System.Text.Json.JsonSerializer.Deserialize<List<Doctor>>(jsonString);

                return View(doctors);
            }
            return View();
        }
        public async Task<IActionResult> Create()
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

            HttpResponseMessage hospitalResponse = await _httpClient.GetAsync("api/Hospital");

            if (hospitalResponse.IsSuccessStatusCode)
            {
                var jsonString = await hospitalResponse.Content.ReadAsStringAsync();
                var hospitals = System.Text.Json.JsonSerializer.Deserialize<List<Hospital>>(jsonString);

                DoctorViewModel doctorViewModel = new DoctorViewModel();
                doctorViewModel.Hospitals = hospitals;

                return View(doctorViewModel);
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(DoctorViewModel doctorViewModel)
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

            if (!ModelState.IsValid)
            {
                return View(doctorViewModel);
            }

            Doctor doctor = new Doctor
            {
                Name = doctorViewModel.Name,
                Email = doctorViewModel.Email,
            };

            HttpResponseMessage doctorResponse = await _httpClient.PostAsJsonAsync("api/Doctor/create?hospitalId=" + doctorViewModel.SelectedHospitalId, doctor);

            if(doctorResponse.IsSuccessStatusCode)
            {
                Console.WriteLine(doctorResponse.Content);
                return RedirectToAction("Index");
            }
            
           
            return View(doctorViewModel);



        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
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


            HttpResponseMessage doctorResponse = await _httpClient.GetAsync("/api/Doctor/" + id);
            if (doctorResponse.IsSuccessStatusCode)
            {
                var jsonString = await doctorResponse.Content.ReadAsStringAsync();
                var doctor = System.Text.Json.JsonSerializer.Deserialize<Doctor>(jsonString);

                DoctorDetailViewModel viewModel = new DoctorDetailViewModel
                {
                    Name = doctor.Name,
                    Email = doctor.Email,
                    Hospital = doctor.Hospital,
                    Appointments = doctor.Appointments,
                };
                HttpResponseMessage appointmentResponse = await _httpClient.GetAsync("/api/Appointment/getbydoctorid/?id=" + id);

                if (appointmentResponse.IsSuccessStatusCode)
                {
                    var jsonString2 = await appointmentResponse.Content.ReadAsStringAsync();
                    var appointments = System.Text.Json.JsonSerializer.Deserialize<List<Appointment>>(jsonString2);

                    viewModel.Appointments = appointments;

                    return View(viewModel);
                }

            }

            return View("Index", "Home");
            
        }
        
        public async Task<IActionResult> Edit(int id)
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

            HttpResponseMessage doctorResponse = await _httpClient.GetAsync("/api/Doctor/" + id);

            if (doctorResponse.IsSuccessStatusCode)
            {
                var jsonString = await doctorResponse.Content.ReadAsStringAsync();
                var doctor = System.Text.Json.JsonSerializer.Deserialize<Doctor>(jsonString);

                DoctorViewModel viewModel = new DoctorViewModel
                {
                    Id = id,
                    Name = doctor.Name,
                    Email = doctor.Email,
                };

                HttpResponseMessage hospitalResponse = await _httpClient.GetAsync("api/Hospital");

                if (hospitalResponse.IsSuccessStatusCode)
                {
                    var jsonString2 = await hospitalResponse.Content.ReadAsStringAsync();
                    var hospitals = System.Text.Json.JsonSerializer.Deserialize<List<Hospital>>(jsonString2);

                    viewModel.Hospitals = hospitals;

                    return View(viewModel);
                }
            }

            return View("Index");
        }

        [HttpPost]
        public async Task<IActionResult> EditResponse(DoctorViewModel viewModel)
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

            Doctor doctor = new Doctor
            {
                Id = viewModel.Id,
                Name = viewModel.Name,
                Email = viewModel.Email,
            };

            HttpResponseMessage doctorResponse = await _httpClient.PutAsJsonAsync("/api/Doctor/update/" + viewModel.Id + "?hospitalId=" + viewModel.SelectedHospitalId, doctor);
            if(doctorResponse.IsSuccessStatusCode)
            {
                Console.WriteLine(doctorResponse.Content);
                return RedirectToAction("Index");
            }

            return View();
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

            HttpResponseMessage doctorResponse = await _httpClient.DeleteAsync("/api/Doctor/delete/" + id);
            if (doctorResponse.IsSuccessStatusCode)
            {
                Console.WriteLine(doctorResponse.Content);
                return RedirectToAction("Index");
            }

            return View("Index");

        }

        public async Task<IActionResult> AssignWorkHours(int id)
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

            TimingViewModel timingViewModel = new TimingViewModel();
            timingViewModel.DoctorId = id;

            return View(timingViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AssignWorkHours(TimingViewModel timingViewModel)
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

            if(timingViewModel.SelectedShiftStart > timingViewModel.SelectedShiftEnd)
            {
                return RedirectToAction("Index", "Home");   // error should be here
            }

            Timing timing = new Timing
            {
                WorkDay = timingViewModel.WorkDay,
                ShiftStart = timingViewModel.SelectedShiftStart,
                ShiftEnd = timingViewModel.SelectedShiftEnd
            };
            

            HttpResponseMessage timingResponse = await _httpClient.PostAsJsonAsync("/api/Timing/create?doctorId=" + 
                timingViewModel.DoctorId
                , timing);

            if (timingResponse.IsSuccessStatusCode)
            {
                Console.WriteLine(timingResponse.Content);
                return RedirectToAction("Index");
            }

            return View("Index", "Home");
        }
    }
}
