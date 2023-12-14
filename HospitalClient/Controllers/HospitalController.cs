using HospitalClient.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace HospitalClient.Controllers
{
    
    public class HospitalController : Controller   
    {
        private readonly HttpClient _httpClient;
        
        public HospitalController()
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
            HttpResponseMessage response = await _httpClient.GetAsync("api/Hospital");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var hospitals = JsonSerializer.Deserialize<List<Hospital>>(jsonString);

                return View(hospitals);
            }

            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Hospital hospital)
        {  
            if (!ModelState.IsValid)
            {
                return View(hospital);
            }

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/Hospital/create", hospital);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Hospital başarıyla oluşturuldu!");
                return RedirectToAction("Index");
            }

            return View(hospital);
            
        }
    }
}
