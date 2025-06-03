using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using PhoneStore.BusinessObjects.Models;
using System.Threading.Tasks;

namespace PhoneStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: api/product
        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> Get()
        {
            var products = await _productService.GetAllAsync();
            return Ok(products);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] ProductSearchFilterRequest request)
        {
            var result = await _productService.SearchProductsAsync(request);
            return Ok(result);
        }

        // GET: api/product/detail/
        [HttpGet("detail/{id}")]
        public async Task<IActionResult> GetProductDetail(int id)
        {
            var product = await _productService.GetProductDetailAsync(id);
            if (product == null)
            {
                return NotFound(new { Message = $"Product with id {id} not found." });
            }
            return Ok(product);
        }
        
        [HttpGet("by-variant-info")]
        public async Task<IActionResult> GetProductByColorAndVersion([FromQuery] string color, [FromQuery] string version)
        {
            if (string.IsNullOrWhiteSpace(color) || string.IsNullOrWhiteSpace(version))
            {
                return BadRequest(new { Message = "Color and Version are required." });
            }

            var product = await _productService.GetProductByColorAndVersionAsync(color, version);
            if (product == null)
            {
                return NotFound(new { Message = $"No product found with color '{color}' and version '{version}'." });
            }

            return Ok(product);
        }
        // GET: api/product/colors
        [HttpGet("colors")]
        public async Task<IActionResult> GetAllColors()
        {
            var colors = await _productService.GetAllColorsAsync();
            return Ok(colors);
        }

        // GET: api/product/versions
        [HttpGet("versions")]
        public async Task<IActionResult> GetAllVersions()
        {
            var versions = await _productService.GetAllVersionsAsync();
            return Ok(versions);
        }
        // GET: api/product/by-color?color=Black
        [HttpGet("string-color")]
        public async Task<IActionResult> GetProductsByColor([FromQuery] string color)
        {
            if (string.IsNullOrWhiteSpace(color))
            {
                return BadRequest(new { Message = "Color is required." });
            }

            var products = await _productService.GetProductsByColorAsync(color);
            if (!products.Any())
            {
                return NotFound(new { Message = $"No products found with color '{color}'." });
            }

            return Ok(products);
        }

        // GET: api/product/by-version?version=128GB
        [HttpGet("string-version")]
        public async Task<IActionResult> GetProductsByVersion([FromQuery] string version)
        {
            if (string.IsNullOrWhiteSpace(version))
            {
                return BadRequest(new { Message = "Version is required." });
            }

            var products = await _productService.GetProductsByVersionAsync(version);
            if (!products.Any())
            {
                return NotFound(new { Message = $"No products found with version '{version}'." });
            }

            return Ok(products);
        }




        [HttpGet("{id}")]
        private async Task<IActionResult> GetById(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null) return NotFound();

            var result = new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                MainImage = product.MainImage,
                BrandName = product.Brand?.Name
            };

            return Ok(result);
        }

        // POST: api/Product
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductCreateDto dto)
        {
            var brand = await _productService.GetBrandByNameAsync(dto.BrandName);
            if (brand == null)
                return BadRequest("Brand không tồn tại.");

            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                MainImage = dto.MainImage,
                BrandId = brand.Id,
                CreatedAt = DateTime.UtcNow
            };

            await _productService.AddAsync(product);

            // Trả về DTO đơn giản, tránh vòng lặp
            var result = new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                MainImage = product.MainImage,
                BrandName = brand.Name
            };

            return CreatedAtAction(nameof(GetById), new { id = product.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductDto dto)
        {
            if (id != dto.Id)
                return BadRequest("ID không khớp.");

            var product = await _productService.GetByIdAsync(id);
            if (product == null)
                return NotFound();

            // Gán lại các thuộc tính
            product.Name = dto.Name;
            product.Description = dto.Description;
            product.MainImage = dto.MainImage;

            // Nếu muốn cập nhật Brand thông qua BrandName:
            if (!string.IsNullOrEmpty(dto.BrandName))
            {
                // Tìm Brand theo tên (giả sử bạn có BrandRepository hoặc context)
                var brand = await _productService.GetBrandByNameAsync(dto.BrandName);
                if (brand == null)
                    return BadRequest("Brand không tồn tại.");
                product.BrandId = brand.Id;
            }

            await _productService.UpdateAsync(product);

            return NoContent();
        }

        // DELETE: api/Product/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _productService.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            await _productService.DeleteAsync(id);
            return NoContent();
        }
    }
}
