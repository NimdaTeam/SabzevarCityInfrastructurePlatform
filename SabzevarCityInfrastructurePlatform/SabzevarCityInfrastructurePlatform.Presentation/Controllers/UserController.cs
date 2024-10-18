using AuthenticationSystem.Domain.User;
using AuthenticationSystem.Services.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace WebHost.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userService;

        private readonly IRoleRepository _roleService;

        public UserController(IUserRepository userService, IRoleRepository roleService)
        {
            _userService = userService;
            _roleService = roleService;
        }

        [Route("AllUsers")]
        public IActionResult AllUsers()
        {
            var users = _userService.GetAll();
            return View(users);
        }

        [Route("AddUser")]
        public IActionResult AddUser()
        {
            ViewBag.roles = _roleService.GetAll();

            return PartialView();
        }

        [Route("AddUser")]
        [HttpPost]
        public IActionResult AddUser(Users user)
        {
            _userService.Create(user);
            return RedirectToAction("AllUsers");
        }

        [Route("UpdateUser/{userId}")]
        public IActionResult UpdateUser(long userId)
        {
            var user = _userService.Get(userId);
            ViewBag.roles = _roleService.GetAll();
            return PartialView(user);
        }

        [Route("UpdateUser/{userId}")]
        [HttpPost]
        public IActionResult UpdateUser(Users user)
        {
            _userService.Update(user);
            return RedirectToAction("AllUsers");
        }

        [Route("DeleteUser/{userId}")]
        public IActionResult DeleteUser(long userId)
        {
            _userService.Delete(userId);
            return RedirectToAction("AllUsers");
        }
    }
}
