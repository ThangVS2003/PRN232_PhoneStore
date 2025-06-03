using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using PhoneStoreWeb.Models;
using PhoneStoreAPI.ViewModels;

namespace PhoneStoreWeb.Controllers
{
    public class AuthController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AuthController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
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
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            // Debug: Kiểm tra model nhận được
            System.Console.WriteLine($"Received model in Web: Username={model?.Username}, Password={model?.Password}");
            if (model == null)
            {
                System.Console.WriteLine("Model is null in Web");
            }

            // Debug: Kiểm tra toàn bộ ModelState
            if (!ModelState.IsValid)
            {
                System.Console.WriteLine("ModelState is invalid in Web");
                var errors = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
                System.Console.WriteLine("Validation errors in Web: " + string.Join(", ", errors));
                foreach (var state in ModelState)
                {
                    System.Console.WriteLine($"Key: {state.Key}, Value: {state.Value.AttemptedValue}, Errors: {string.Join(", ", state.Value.Errors.Select(e => e.ErrorMessage))}");
                }
                return Json(new { success = false, errors = errors });
            }

            var client = _httpClientFactory.CreateClient("PhoneStoreAPI");
            var response = await client.PostAsJsonAsync("api/Auth/login", model);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
                HttpContext.Session.SetString("JwtToken", result.Token);
                return Json(new { success = true, redirect = Url.Action("Index", "Home") });
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            return Json(new { success = false, message = errorContent ?? "Invalid username or password." });
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, errors = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage) });
            }

            var client = _httpClientFactory.CreateClient("PhoneStoreAPI");
            var response = await client.PostAsJsonAsync("api/Auth/register", model);
            if (response.IsSuccessStatusCode)
            {
                return Json(new { success = true, message = "Registration successful. Please log in.", redirect = Url.Action("Login") });
            }

            var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
            return Json(new { success = false, message = error?.Message ?? "An error occurred during registration." });
        }
    }
}