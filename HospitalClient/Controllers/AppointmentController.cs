using HospitalClient.Models;
using HospitalClient.ViewModels;
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
            return View();
        }

        public async Task<IActionResult> Create(AppointmentViewModel viewModel)
        {
            _viewModel = viewModel;

            if (_viewModel.SelectedClinicId != 0)
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

            if (_viewModel.SelectedHospitalId != 0)
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
    }
}
