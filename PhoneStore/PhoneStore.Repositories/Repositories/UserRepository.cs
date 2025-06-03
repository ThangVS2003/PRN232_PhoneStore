using Microsoft.EntityFrameworkCore;
using PhoneStore.BusinessObjects.Models;
using PhoneStore.Repositories.IRepositories;

namespace PhoneStore.Repositories.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly Prn232PhoneContext _context;

        public UserRepository(Prn232PhoneContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username && (u.IsDeleted == null || !u.IsDeleted.Value));
        }

        public async Task<bool> IsUsernameExistsAsync(string username)
        {
            return await _context.Users.AnyAsync(u => u.Username == username && (u.IsDeleted == null || !u.IsDeleted.Value));
        }

        public async Task<User> AddUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
    }
}