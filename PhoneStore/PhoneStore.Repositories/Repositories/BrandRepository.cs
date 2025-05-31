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
    }
}