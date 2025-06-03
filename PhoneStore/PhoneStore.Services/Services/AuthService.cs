using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PhoneStore.BusinessObjects.Models;
using PhoneStore.Repositories.IRepositories;
using PhoneStore.Services.IServices;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PhoneStore.Services.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<string> AuthenticateAsync(string username, string password)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);
            if (user == null || user.Password != password) // Nên hash password trong thực tế
            {
                return null;
            }

            string role = user.Role switch
            {
                1 => "Admin",
                2 => "Staff",
                3 => "User",
                _ => null
            };

            if (role == null)
            {
                return null; // Vai trò không hợp lệ
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, role),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<User> RegisterAsync(User user)
        {
            if (await _userRepository.IsUsernameExistsAsync(user.Username))
            {
                throw new InvalidOperationException("Username already exists.");
            }

            user.CreatedAt = DateTime.Now;
            user.IsDeleted = false;
            user.Role ??= 3; // Đặt mặc định là User nếu Role null
            _userRepository.AddUserAsync(user); // Gọi phương thức AddUserAsync từ repository
            return user;
        }
    }
}