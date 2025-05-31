using PhoneStore.BusinessObjects.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhoneStore.Repositories.IRepositories
{
    public interface IBrandRepository
    {
        Task<IEnumerable<Brand>> GetAllAsync();
    }
}