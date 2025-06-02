using PhoneStore.BusinessObjects.Models;
using PhoneStoreWeb.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhoneStore.Repositories.IRepositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();

        Task<IEnumerable<Product>> SearchProductAsync(ProductSearchFilterRequest request);
    }
}