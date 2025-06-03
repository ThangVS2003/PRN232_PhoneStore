using PhoneStore.BusinessObjects.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhoneStore.Repositories.IRepositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();


        //Task<IEnumerable<Product>> SearchProductAsync(ProductSearchFilterRequest request);

        Task<Product?> GetProductDetailByIdAsync(int id);
        Task<Product?> GetProductByColorAndVersionAsync(string color, string version);
        Task<IEnumerable<string>> GetAllColorsAsync();
        Task<IEnumerable<string>> GetAllVersionsAsync();
        Task<IEnumerable<Product>> GetProductsByColorAsync(string color);
        Task<IEnumerable<Product>> GetProductsByVersionAsync(string version);


        Task<Product?> GetByIdAsync(int id);
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task<Brand?> GetBrandByNameAsync(string brandName);

        Task DeleteAsync(int id);
    }
}