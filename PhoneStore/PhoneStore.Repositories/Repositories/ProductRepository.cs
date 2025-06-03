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

        //public async Task<IEnumerable<Product>> SearchProductAsync(ProductSearchFilterRequest request)
        //{
        //    var query = _context.Products.Include(p => p.ProductVariants)
        //        .Where(p => p.IsDeleted == null || p.IsDeleted == false).AsQueryable();

        //    if (!string.IsNullOrEmpty(request.keyword))
        //    {
        //        query = query.Where(p => p.Name.Contains(request.keyword));
        //    }

        //    if (request.minPrice.HasValue)
        //    {
        //        query = query.Where(p => p.ProductVariants.Any(v => v.SellingPrice >= request.minPrice));
        //    } 
        //    if (request.maxPrice.HasValue)
        //    {
        //        query = query.Where(p => p.ProductVariants.Any(v => v.SellingPrice <= request.maxPrice));
        //    }

        //    if (request.sortByPrice.HasValue)
        //    {
        //        query = request.sortByPrice.Value ? query.OrderBy(p => p.ProductVariants.Min(v => v.SellingPrice))
        //            : query.OrderByDescending(p => p.ProductVariants.Max(v => v.SellingPrice));
        //    }
        //    return await query.ToListAsync();
        //}
        
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

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.ProductVariants)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AddAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task<Brand?> GetBrandByNameAsync(string brandName)
        {
            return await _context.Brands.FirstOrDefaultAsync(b => b.Name == brandName);
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

    }
}