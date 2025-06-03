using Microsoft.EntityFrameworkCore;
using PhoneStore.BusinessObjects.Models;
using PhoneStore.Repositories;
using PhoneStore.Repositories.IRepositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhoneStore.Repositories.Repositories
{
    public class BrandRepository : IBrandRepository
    {
        private readonly Prn232PhoneContext _context;

        public BrandRepository(Prn232PhoneContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Brand>> GetAllAsync()
        {
            return await _context.Brands.ToListAsync();
        }

        public async Task<Brand?> GetByIdAsync(int id)
        {
            return await _context.Brands.FindAsync(id);
        }

        public async Task AddAsync(Brand brand)
        {
            await _context.Brands.AddAsync(brand);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Brand brand)
        {
            var existingBrand = await _context.Brands.FindAsync(brand.Id);
            if (existingBrand != null)
            {
                existingBrand.Name = brand.Name;
                existingBrand.Description = brand.Description;
                // Nếu có thêm các quan hệ hay logic khác thì xử lý tại đây.

                _context.Brands.Update(existingBrand);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var brand = await _context.Brands.FindAsync(id);
            if (brand != null)
            {
                _context.Brands.Remove(brand);
                await _context.SaveChangesAsync();
            }
        }
    }
}