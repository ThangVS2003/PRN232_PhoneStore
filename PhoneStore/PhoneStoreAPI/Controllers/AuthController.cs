using Microsoft.AspNetCore.Mvc;
using PhoneStore.BusinessObjects.Models;
using PhoneStore.Services.IServices;
using PhoneStoreAPI.ViewModels;
using System.Threading.Tasks;

namespace PhoneStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            Console.WriteLine($"Received model: Username={model?.Username}, Password={model?.Password}");
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                Console.WriteLine("Validation errors: " + string.Join(", ", errors));
                return BadRequest(new { Errors = errors });
            }

            try
            {
                var token = await _authService.AuthenticateAsync(model.Username, model.Password);
                if (token == null)
                {
                    return Unauthorized("Invalid username or password.");
                }
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { Errors = errors });
            }

            try
            {
                var user = new User
                {
                    Username = model.Username,
                    Name = model.Name,
                    Email = model.Email,
                    Phone = model.Phone,
                    Address = model.Address,
                    Password = model.Password, // Nên hash password trong thực tế
                    CreatedAt = DateTime.Now,
                    IsDeleted = false,
                    Role = 3 // Default role is User
                };

                var registeredUser = await _authService.RegisterAsync(user);
                return Ok(new { Message = "User registered successfully", UserId = registeredUser.Id });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}