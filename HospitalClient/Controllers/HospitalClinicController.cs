using HospitalClient.Models;
using HospitalClient.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace HospitalClient.Controllers
{
    public class HospitalClinicController : Controller
    {
        private readonly HttpClient _httpClient;

        public HospitalClinicController()
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

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Match() {

            HttpResponseMessage hospitalResponse = await _httpClient.GetAsync("api/Hospital");
            HttpResponseMessage clinicResponse = await _httpClient.GetAsync("api/Clinic");

            if (hospitalResponse.IsSuccessStatusCode)
            {
                var jsonString = await hospitalResponse.Content.ReadAsStringAsync();
                var hospitals = JsonSerializer.Deserialize<List<Hospital>>(jsonString);
                
                HospitalClinicViewModel viewModel = new HospitalClinicViewModel();
                viewModel.Hospitals = hospitals;

                if (clinicResponse.IsSuccessStatusCode)
                {
                    var jsonString2 = await clinicResponse.Content.ReadAsStringAsync();
                    var clinics = JsonSerializer.Deserialize<List<Clinic>>(jsonString2);

                    viewModel.Clinics = clinics;

                    return View(viewModel);
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Match(HospitalClinicViewModel viewModel)
        {
            HospitalClinic hospitalClinic = new HospitalClinic
            {
                HospitalId = viewModel.SelectedHospitalId,
                ClinicId = viewModel.SelectedClinicId
            };

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/HospitalClinic/create", hospitalClinic);
            if(response.IsSuccessStatusCode)
            {
                Console.WriteLine("Hospital ve Clinic başarıyla eşleştirildi!");
                return RedirectToAction("Index", "Home");
            }

            return View(viewModel);
        }
    }
}
