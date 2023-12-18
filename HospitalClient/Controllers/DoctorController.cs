using HospitalClient.Models;
using HospitalClient.ViewModels;
using Microsoft.AspNetCore.Mvc;
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
                var doctors = JsonSerializer.Deserialize<List<Doctor>>(jsonString);

                return View(doctors);
            }
            return View();
        }
        public async Task<IActionResult> Create()
        {
            HttpResponseMessage hospitalResponse = await _httpClient.GetAsync("api/Hospital");

            if (hospitalResponse.IsSuccessStatusCode)
            {
                var jsonString = await hospitalResponse.Content.ReadAsStringAsync();
                var hospitals = JsonSerializer.Deserialize<List<Hospital>>(jsonString);

                DoctorViewModel doctorViewModel = new DoctorViewModel();
                doctorViewModel.Hospitals = hospitals;

                return View(doctorViewModel);
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(DoctorViewModel doctorViewModel)
        {
            if(!ModelState.IsValid)
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

            HttpResponseMessage doctorResponse = await _httpClient.GetAsync("/api/Doctor/" + id);
            if (doctorResponse.IsSuccessStatusCode)
            {
                var jsonString = await doctorResponse.Content.ReadAsStringAsync();
                var doctor = JsonSerializer.Deserialize<Doctor>(jsonString);

                DoctorDetailViewModel viewModel = new DoctorDetailViewModel
                {
                    Name = doctor.Name,
                    Email = doctor.Email,
                    Hospital = doctor.Hospital,
                    Appointments = doctor.Appointments,
                };
                HttpResponseMessage appointmentResponse = await _httpClient.GetAsync("/api/Appointment/getbydoctorid/" + id);

                if (appointmentResponse.IsSuccessStatusCode)
                {
                    var jsonString2 = await appointmentResponse.Content.ReadAsStringAsync();
                    var appointments = JsonSerializer.Deserialize<List<Appointment>>(jsonString2);

                    viewModel.Appointments = appointments;

                    return View(viewModel);
                }

            }

            return View("Index", "Home");
            
        }
    }
}
