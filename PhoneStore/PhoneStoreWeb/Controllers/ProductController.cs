// PhoneStoreWeb/Controllers/ProductController.cs
using Microsoft.AspNetCore.Mvc;
using PhoneStore.BusinessObjects.Models;
using System.Net.Http.Json;

namespace PhoneStoreWeb.Controllers
{
    public class ProductController : Controller
    {
        private readonly HttpClient _httpClient;

        public ProductController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:5001/api/"); // URL API
        }

        public async Task<IActionResult> Index()
        {
            var products = await _httpClient.GetFromJsonAsync<List<Product>>("Product");
            return View(products);
        }
        public async Task<IActionResult> ProductDetail(int id)
        {
            var product = await _httpClient.GetFromJsonAsync<Product>($"Product/{id}");
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

    }
}