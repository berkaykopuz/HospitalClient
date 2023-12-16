using HospitalClient.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace HospitalClient.Controllers
{
    public class ClinicController : Controller
    {
        private readonly HttpClient _httpClient;
        public ClinicController()
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
            HttpResponseMessage response = await _httpClient.GetAsync("api/Clinic");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var clinics = JsonSerializer.Deserialize<List<Clinic>>(jsonString);

                return View(clinics);
            }

            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Clinic clinic)
        {
            if (!ModelState.IsValid)
            {
                return View(clinic);
            }

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/Clinic/create", clinic);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Clinic başarıyla oluşturuldu!");
                return RedirectToAction("Index");
            }

            return View(clinic);
        }


    }
}
