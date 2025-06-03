using PhoneStore.BusinessObjects.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhoneStore.Repositories.IRepositories
{
    public interface IBrandRepository
    {
        Task<IEnumerable<Brand>> GetAllAsync();
        Task<Brand?> GetByIdAsync(int id);
        Task AddAsync(Brand brand);
        Task UpdateAsync(Brand brand);
        Task DeleteAsync(int id);
    }
}