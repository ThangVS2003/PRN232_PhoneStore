using Microsoft.AspNetCore.Mvc;

namespace PhoneStoreAPI.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
