using PhoneStore.BusinessObjects.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhoneStore.Services.IServices
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product?> GetProductDetailAsync(int id);
        Task<Product?> GetProductByColorAndVersionAsync(string color, string version);
        Task<IEnumerable<string>> GetAllColorsAsync();
        Task<IEnumerable<string>> GetAllVersionsAsync();
        Task<IEnumerable<Product>> GetProductsByColorAsync(string color);
        Task<IEnumerable<Product>> GetProductsByVersionAsync(string version);

        //Task<IEnumerable<Product>> GetProductsByNameAsync(string name);


    }

}