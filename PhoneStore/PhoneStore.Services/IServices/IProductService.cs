using PhoneStore.BusinessObjects.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhoneStore.Services.IServices
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllAsync();
    }
}