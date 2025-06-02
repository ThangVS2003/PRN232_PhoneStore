using PhoneStore.BusinessObjects.Models;
using PhoneStore.Repositories.IRepositories;
using PhoneStore.Repositories.Repositories;
using PhoneStore.Services.IServices;
using PhoneStoreWeb.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhoneStore.Services.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<IEnumerable<Product>> SearchProductsAsync(ProductSearchFilterRequest request)
        {
            return await _repository.SearchProductAsync(request);
        }
    }
}