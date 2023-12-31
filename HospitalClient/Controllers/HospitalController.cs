﻿using HospitalClient.Models;
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
            var role = HttpContext.Session.GetString("role");
            if (!role.Equals("Admin"))
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Hospital hospital)
        {
            var role = HttpContext.Session.GetString("role");
            if (!role.Equals("Admin"))
            {
                return RedirectToAction("AccessDenied", "Home");
            }

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

        public async Task<IActionResult> Edit(int id)
        {
            var role = HttpContext.Session.GetString("role");
            if (!role.Equals("Admin"))
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            HttpResponseMessage response = await _httpClient.GetAsync("api/Hospital/" + id);

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var hospital = JsonSerializer.Deserialize<Hospital>(jsonString);

                return View(hospital);
            }

            return View("Index");
        }

        [HttpPost]
        public async Task<IActionResult> EditResponse(Hospital hospital)
        {
            var role = HttpContext.Session.GetString("role");
            if (!role.Equals("Admin"))
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            HttpResponseMessage response = await _httpClient.PutAsJsonAsync("/api/Hospital/update/" + hospital.Id, hospital);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.Content);
                return RedirectToAction("Index");
            }

            return View();
        }

        public async Task<IActionResult> Delete(int id)
        {
            var role = HttpContext.Session.GetString("role");
            if (!role.Equals("Admin"))
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            HttpResponseMessage hospitalResponse = await _httpClient.DeleteAsync("/api/Hospital/delete/" + id);
            if (hospitalResponse.IsSuccessStatusCode)
            {
                Console.WriteLine(hospitalResponse.Content);
                return RedirectToAction("Index");
            }

            return View("Index");

        }
    }
}
