using Microsoft.AspNetCore.Mvc;
using PhoneStore.BusinessObjects.Models;
using PhoneStore.Services.IServices;
using PhoneStoreAPI.Models;
using System.Threading.Tasks;

namespace PhoneStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IBrandService _brandService;

        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var brands = await _brandService.GetAllAsync();
            return Ok(brands);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var brand = await _brandService.GetByIdAsync(id);
            if (brand == null)
                return NotFound();
            return Ok(brand);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BrandDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var brand = new Brand
            {
                Name = dto.Name,
                Description = dto.Description
            };

            await _brandService.AddAsync(brand);

            dto.Id = brand.Id; // Gán lại id sau khi lưu

            return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] BrandDto dto)
        {
            if (id != dto.Id)
                return BadRequest("ID mismatch");

            var existingBrand = await _brandService.GetByIdAsync(id);
            if (existingBrand == null)
                return NotFound();

            // Cập nhật 3 thuộc tính
            existingBrand.Name = dto.Name;
            existingBrand.Description = dto.Description;

            await _brandService.UpdateAsync(existingBrand);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existingBrand = await _brandService.GetByIdAsync(id);
            if (existingBrand == null)
                return NotFound();

            await _brandService.DeleteAsync(id);
            return NoContent();
        }
    }
}