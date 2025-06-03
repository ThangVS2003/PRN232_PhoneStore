using Microsoft.AspNetCore.Mvc;
using PhoneStore.BusinessObjects.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Json;
using System.Linq;

namespace PhoneStoreWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient;

        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("PhoneStoreAPI");
        }

        public async Task<IActionResult> Index(int? brandId, int page = 1, int pageSize = 6)
        {
            List<Brand> brands = new List<Brand>();
            List<Product> products = new List<Product>();

            // Lấy danh sách Brand
            var brandsResponse = await _httpClient.GetAsync("api/Brand");
            if (brandsResponse.IsSuccessStatusCode)
            {
                brands = await brandsResponse.Content.ReadFromJsonAsync<List<Brand>>() ?? new List<Brand>();
            }

            // Lấy danh sách Product
            var productsResponse = await _httpClient.GetAsync("api/Product");
            if (productsResponse.IsSuccessStatusCode)
            {
                products = await productsResponse.Content.ReadFromJsonAsync<List<Product>>() ?? new List<Product>();
            }

            // Lọc sản phẩm không bị xóa
            var activeProducts = products.ToList();

            // Lọc theo brandId nếu có
            var filteredProducts = !brandId.HasValue || brandId == 0
                ? activeProducts
                : activeProducts.Where(p => p.BrandId == brandId).ToList();

            // Tính số lượng sản phẩm cho mỗi thương hiệu
            var brandProductCounts = activeProducts
                .GroupBy(p => p.BrandId)
                .ToDictionary(g => g.Key, g => g.Count());

            // Phân trang
            int totalItems = filteredProducts.Count;
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            page = Math.Max(1, Math.Min(page, totalPages)); // Đảm bảo page hợp lệ
            var pagedProducts = filteredProducts
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Truyền dữ liệu vào ViewBag
            ViewBag.Brands = brands;
            ViewBag.Products = pagedProducts;
            ViewBag.SelectedBrandId = brandId;
            ViewBag.BrandProductCounts = brandProductCounts;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.PageSize = pageSize;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Filter(string? keyword, decimal? minPrice, decimal? maxPrice)
        {
            // Gọi API filter
            var queryParams = new Dictionary<string, string>();
            if (!string.IsNullOrWhiteSpace(keyword)) queryParams["keyword"] = keyword;
            if (minPrice.HasValue) queryParams["minPrice"] = minPrice.Value.ToString();
            if (maxPrice.HasValue) queryParams["maxPrice"] = maxPrice.Value.ToString();

            string query = string.Join("&", queryParams.Select(kv => $"{kv.Key}={Uri.EscapeDataString(kv.Value)}"));
            string url = string.IsNullOrEmpty(query) ? "api/Product/search" : $"api/Product/search?{query}";

            var response = await _httpClient.GetAsync(url);
            var products = new List<Product>();
            if (response.IsSuccessStatusCode)
            {
                products = await response.Content.ReadFromJsonAsync<List<Product>>() ?? new();
            }

            // Lấy danh sách Brand
            var brands = new List<Brand>();
            var brandsResponse = await _httpClient.GetAsync("api/Brand");
            if (brandsResponse.IsSuccessStatusCode)
            {
                brands = await brandsResponse.Content.ReadFromJsonAsync<List<Brand>>() ?? new();
            }

            // Đếm sản phẩm theo Brand
            var brandCounts = products
                .GroupBy(p => p.BrandId)
                .ToDictionary(g => g.Key, g => g.Count());

            // ViewBag truyền về
            ViewBag.Products = products;
            ViewBag.Brands = brands;
            ViewBag.BrandProductCounts = brandCounts;
            ViewBag.Keyword = keyword;
            ViewBag.MinPrice = minPrice;
            ViewBag.MaxPrice = maxPrice;

            return View("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}