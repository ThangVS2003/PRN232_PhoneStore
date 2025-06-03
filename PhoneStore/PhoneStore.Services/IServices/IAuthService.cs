using PhoneStore.BusinessObjects.Models;
using System.Threading.Tasks;

namespace PhoneStore.Services.IServices
{
    public interface IAuthService
    {
        Task<string> AuthenticateAsync(string username, string password);
        Task<User> RegisterAsync(User user); 
    }
}