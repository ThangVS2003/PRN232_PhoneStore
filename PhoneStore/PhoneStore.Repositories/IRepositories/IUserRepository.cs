using PhoneStore.BusinessObjects.Models;
using System.Threading.Tasks;

namespace PhoneStore.Repositories.IRepositories
{
    public interface IUserRepository
    {
        Task<User> GetUserByUsernameAsync(string username);
        Task<bool> IsUsernameExistsAsync(string username);
        Task<User> AddUserAsync(User user);
    }
}