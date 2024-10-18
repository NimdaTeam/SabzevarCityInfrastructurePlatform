using Microsoft.AspNetCore.Mvc;

namespace WebHost.Controllers
{
    public class AccountController : Controller
    {
        [Route("Login")]
        public IActionResult Login()
        {
            return View();
        }
    }
}
