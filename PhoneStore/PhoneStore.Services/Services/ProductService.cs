using PhoneStore.BusinessObjects.Models;
using PhoneStore.Repositories.IRepositories;
using PhoneStore.Repositories.Repositories;
using PhoneStore.Services.IServices;
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
        public async Task<Product?> GetProductDetailAsync(int id)
        {
            return await _repository.GetProductDetailByIdAsync(id);
        }
        public async Task<Product?> GetProductByColorAndVersionAsync(string color, string version)
        {
            return await _repository.GetProductByColorAndVersionAsync(color, version);
        }
        public async Task<IEnumerable<string>> GetAllColorsAsync()
        {
            return await _repository.GetAllColorsAsync();
        }

        public async Task<IEnumerable<string>> GetAllVersionsAsync()
        {
            return await _repository.GetAllVersionsAsync();
        }
        public async Task<IEnumerable<Product>> GetProductsByColorAsync(string color)
        {
            return await _repository.GetProductsByColorAsync(color);
        }

        public async Task<IEnumerable<Product>> GetProductsByVersionAsync(string version)
        {
            return await _repository.GetProductsByVersionAsync(version);
        }




    }
}