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
            var role = HttpContext.Session.GetString("role");
            if (role == null)
            {
                return RedirectToAction("AccessDenied", "Home");
            }
            if (!role.Equals("Admin"))
            {
                return RedirectToAction("AccessDenied", "Home");
            }

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
            var role = HttpContext.Session.GetString("role");
            if (role == null)
            {
                return RedirectToAction("AccessDenied", "Home");
            }
            if (!role.Equals("Admin"))
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Clinic clinic)
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

            HttpResponseMessage response = await _httpClient.GetAsync("api/Clinic/" + id);

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var clinic = JsonSerializer.Deserialize<Clinic>(jsonString);

                return View(clinic);
            }

            return View("Index");
        }

        [HttpPost]
        public async Task<IActionResult> EditResponse(Clinic clinic)
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

            HttpResponseMessage response = await _httpClient.PutAsJsonAsync("/api/Clinic/update/" + clinic.Id, clinic);

            if(response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.Content);
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

            HttpResponseMessage clinicResponse = await _httpClient.DeleteAsync("/api/Clinic/delete/" + id);
            if (clinicResponse.IsSuccessStatusCode)
            {
                Console.WriteLine(clinicResponse.Content);
                return RedirectToAction("Index");
            }

            return View("Index");

        }

    }
}
