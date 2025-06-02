using Microsoft.EntityFrameworkCore;
using PhoneStore.BusinessObjects.Models;
using PhoneStore.Repositories.IRepositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoneStore.Repositories.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly Prn232PhoneContext _context;

        public ProductRepository(Prn232PhoneContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products
                .ToListAsync();
        }
        public async Task<Product?> GetProductDetailByIdAsync(int id)
        {
            return await _context.Products
                .Include(p => p.ProductVariants)
                .Include(p => p.Comments)
                .Include(p => p.Feedbacks)
                .FirstOrDefaultAsync(p => p.Id == id && (p.IsDeleted == null || p.IsDeleted == false));
        }
        public async Task<Product?> GetProductByColorAndVersionAsync(string color, string version)
        {
            var variant = await _context.ProductVariants
                .Include(v => v.Product)
                    .ThenInclude(p => p.ProductVariants)
                .FirstOrDefaultAsync(v =>
                    v.Color != null && v.Version != null &&
                    v.Color.ToLower() == color.ToLower() &&
                    v.Version.ToLower() == version.ToLower() &&
                    (v.IsDeleted == null || v.IsDeleted == false)
                );

            return variant?.Product;
        }
        public async Task<IEnumerable<string>> GetAllColorsAsync()
        {
            return await _context.ProductVariants
                .Where(v => v.Color != null && (v.IsDeleted == null || v.IsDeleted == false))
                .Select(v => v.Color!)
                .Distinct()
                .ToListAsync();
        }

        public async Task<IEnumerable<string>> GetAllVersionsAsync()
        {
            return await _context.ProductVariants
                .Where(v => v.Version != null && (v.IsDeleted == null || v.IsDeleted == false))
                .Select(v => v.Version!)
                .Distinct()
                .ToListAsync();
        }
        public async Task<IEnumerable<Product>> GetProductsByColorAsync(string color)
        {
            return await _context.ProductVariants
                .Where(v => v.Color != null && v.Color.ToLower() == color.ToLower()
                    && (v.IsDeleted == null || v.IsDeleted == false)
                    && (v.Product.IsDeleted == null || v.Product.IsDeleted == false))
                .Select(v => v.Product)
                .Distinct()
                .Include(p => p.ProductVariants)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByVersionAsync(string version)
        {
            return await _context.ProductVariants
                .Where(v => v.Version != null && v.Version.ToLower() == version.ToLower()
                    && (v.IsDeleted == null || v.IsDeleted == false)
                    && (v.Product.IsDeleted == null || v.Product.IsDeleted == false))
                .Select(v => v.Product)
                .Distinct()
                .Include(p => p.ProductVariants)
                .ToListAsync();
        }




    }
}