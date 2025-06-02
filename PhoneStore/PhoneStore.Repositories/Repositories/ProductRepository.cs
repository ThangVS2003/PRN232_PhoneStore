using Microsoft.EntityFrameworkCore;
using PhoneStore.BusinessObjects.Models;
using PhoneStore.Repositories.IRepositories;
using PhoneStoreWeb.ViewModel;
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

        public async Task<IEnumerable<Product>> SearchProductAsync(ProductSearchFilterRequest request)
        {
            var query = _context.Products.Include(p => p.ProductVariants)
                .Where(p => p.IsDeleted == null || p.IsDeleted == false).AsQueryable();

            if (!string.IsNullOrEmpty(request.keyword))
            {
                query = query.Where(p => p.Name.Contains(request.keyword));
            }

            if (request.minPrice.HasValue)
            {
                query = query.Where(p => p.ProductVariants.Any(v => v.SellingPrice >= request.minPrice));
            } 
            if (request.maxPrice.HasValue)
            {
                query = query.Where(p => p.ProductVariants.Any(v => v.SellingPrice <= request.maxPrice));
            }

            if (request.sortByPrice.HasValue)
            {
                query = request.sortByPrice.Value ? query.OrderBy(p => p.ProductVariants.Min(v => v.SellingPrice))
                    : query.OrderByDescending(p => p.ProductVariants.Max(v => v.SellingPrice));
            }
            return await query.ToListAsync();
        }
    }
}