using HospitalClient.Models;
using HospitalClient.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace HospitalClient.Controllers
{
    public class CitizenController : Controller
    {
        private readonly HttpClient _httpClient;
        public CitizenController()
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

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if(!ModelState.IsValid)
            {
                return View(loginViewModel);
            }

            HttpResponseMessage loginResponse = await _httpClient.PostAsJsonAsync("api/Account/login", loginViewModel);

            if (loginResponse.IsSuccessStatusCode)
            {
                var jsonString = await loginResponse.Content.ReadAsStringAsync();
                TokenResponse tokenResponse = JsonSerializer.Deserialize<TokenResponse>(jsonString);

                List<string> roles = tokenResponse.Roles;
                string jwtToken = tokenResponse.Token;

                foreach(var role in roles)
                {
                    if (role.Equals("Admin"))
                    {
                        HttpContext.Session.SetString("role", role);
                        HttpContext.Session.SetString("token", jwtToken);
                    }
                    else if (role.Equals("User"))
                    {
                        HttpContext.Session.SetString("role", role);
                        HttpContext.Session.SetString("token", jwtToken);
                    }
                }

                return RedirectToAction("Index", "Home");
            }

            return View(loginResponse);
        }



        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if(!ModelState.IsValid)
            {
                return View(registerViewModel);
            }

            HttpResponseMessage registerResponse = await _httpClient.PostAsJsonAsync("api/Account/register", registerViewModel);

            if (registerResponse.IsSuccessStatusCode)
            {
                Console.WriteLine("Kullanıcı başarıyla oluşturuldu!");
                return RedirectToAction("Index", "Home");
            }

            return View(registerViewModel);
        }

        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();

            //await HttpContext.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}
