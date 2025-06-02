using PhoneStore.BusinessObjects.Models;
using PhoneStoreWeb.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhoneStore.Services.IServices
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<IEnumerable<Product>> SearchProductsAsync(ProductSearchFilterRequest request);
    }
}