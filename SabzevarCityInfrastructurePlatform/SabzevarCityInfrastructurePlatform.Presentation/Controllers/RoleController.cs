using _0_Framework.Utilities.NotificationSystem;
using AuthenticationSystem.Infrastructure;
using AuthenticationSystem.Services.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WebHost.Controllers
{
    public class RoleController : Controller
    {
        private readonly IRoleRepository _roleService;

        public RoleController(IRoleRepository roleService)
        {
            _roleService = roleService;
        }

        [Route("AllRoles")]
        public IActionResult AllRoles()
        {
            var roles = _roleService.GetRolesForRolesGrid();
            return View(roles);
        }

        [Route("AddRole")]
        public IActionResult AddRole()
        {
            var permissions = AuthenticationSystemBootstrapper
                .SystemPermissionExposer();

            ViewBag.Permissions = permissions;

            return PartialView();
        }

        [Route("AddRole")]
        [HttpPost]
        public IActionResult AddRole(string roleName, List<long> permissions)
        {
            if (_roleService.IsRoleNameDuplicate(roleName))
            {
                NotificationSystem
                    .ShowNotification(TempData, ApplicationMessages.DuplicateValueTitle, ApplicationMessages.DuplicateValueDescription, ApplicationMessagesIcon.ErrorIcon);
                return RedirectToAction("AllRoles");
            }

            var role = _roleService.CreateNewRole(roleName);

            _roleService.AddPermissionsToRole(permissions, role.Id);

            NotificationSystem
                .ShowNotification(TempData, ApplicationMessages.OperationSuccessful, "", ApplicationMessagesIcon.SuccessIcon);
            return RedirectToAction("AllRoles");
        }


        [Route("UpdateRole/{roleId}")]
        public IActionResult UpdateRole(long roleId)
        {
            var role = _roleService.Get(roleId);

            if (role == null)
            {
                NotificationSystem
                    .ShowNotification(TempData, "عملیات ناموفق", "رکوردی با این مشخصات یافت نشد", ApplicationMessagesIcon.ErrorIcon);
                return RedirectToAction("AllRoles");
            }

            ViewBag.permissions = _roleService.GetPermissionsForRoleEdit(roleId);
            return PartialView(role);
        }

        [Route("UpdateRole/{roleId}")]
        [HttpPost]
        public IActionResult UpdateRole(long roleId, string roleName, List<long> permissions)
        {
            var role = _roleService.Get(roleId);

            if (role == null)
            {
                NotificationSystem
                    .ShowNotification(TempData, "عملیات ناموفق", "رکوردی با این مشخصات یافت نشد", ApplicationMessagesIcon.ErrorIcon);
                return RedirectToAction("AllRoles");
            }

            if (_roleService.IsRoleNameDuplicateForEdit(roleId,roleName))
            {
                NotificationSystem
                    .ShowNotification(TempData, ApplicationMessages.DuplicateValueTitle, ApplicationMessages.DuplicateValueDescription, ApplicationMessagesIcon.ErrorIcon);
                return RedirectToAction("AllRoles");
            }

            var operation = _roleService
                .UpdateRoleInformation(roleId, roleName, permissions);

            if (operation.IsSuccessful)
            {
                NotificationSystem
                    .ShowNotification(TempData, ApplicationMessages.OperationSuccessful, "", ApplicationMessagesIcon.SuccessIcon);
            }
            else
            {
                NotificationSystem
                    .ShowNotification(TempData, ApplicationMessages.OperationFailed, operation.Exception.ToString(), ApplicationMessagesIcon.ErrorIcon);
            }

            return RedirectToAction("AllRoles");
        }


        [Route("DeleteRole/{roleId}")]
        public IActionResult DeleteRole(long roleId)
        {
            var role = _roleService.Get(roleId);

            if (role == null)
            {
                NotificationSystem
                    .ShowNotification(TempData, "عملیات ناموفق", "رکوردی با این مشخصات یافت نشد", ApplicationMessagesIcon.ErrorIcon);
                return RedirectToAction("AllRoles");
            }


            if (role.Name == "پیش فرض")
            {
                NotificationSystem
                    .ShowNotification(TempData, "عملیات ناموفق", "سطح دسترسی پیش فرض قابل حذف شدن نیست", ApplicationMessagesIcon.ErrorIcon);
                return RedirectToAction("AllRoles");
            }

            if (_roleService.DoesRoleHasAnyUsers(roleId))
                _roleService.MigrateAllUsersToDefaultRole(roleId);

            if (_roleService.DoesRoleHasAnyPermissions(roleId))
                _roleService.DeleteAllRolePermissions(roleId);

            _roleService.Delete(roleId);
            _roleService.SaveChanges();

            NotificationSystem
                .ShowNotification(TempData, ApplicationMessages.OperationSuccessful, "", ApplicationMessagesIcon.SuccessIcon);


            return RedirectToAction("AllRoles");
        }

    }
}
