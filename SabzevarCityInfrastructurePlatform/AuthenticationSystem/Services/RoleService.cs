using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using _0_Framework.DTO;
using _0_Framework.GenericRepositoy.Service;
using _0_Framework.Utilities.Generators;
using AuthenticationSystem.Domain.Role;
using AuthenticationSystem.Domain.RolePermission;
using AuthenticationSystem.Domain.User;
using AuthenticationSystem.Infrastructure;
using AuthenticationSystem.Services.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationSystem.Services
{
    public class RoleService : RepositoryService<long, Roles>, IRoleRepository
    {
        private readonly AuthenticationSystemContext _context;
        private readonly IUserRepository _userService;
        private readonly IHttpContextAccessor _httpContext;

        public RoleService(AuthenticationSystemContext context, IUserRepository userService, IHttpContextAccessor httpContext) : base(context)
        {
            _context = context;
            _userService = userService;
            _httpContext = httpContext;
        }

        #region Roles
        public bool IsUserInRole(string phoneNumber, long roleId)
        {
            var user = _userService.GetUserByPhoneNumber(phoneNumber);

            return _context.UserRoles
                .Any(x => x.UserId == user.Id && x.RoleId == roleId);
        }


        public void AddRoleToUser(List<long> roles, long userId)
        {
            try
            {
                foreach (var role in roles)
                {
                    _context.UserRoles.Add(new UserRoles()
                    {
                        UserId = userId,
                        RoleId = role
                    });
                }
                SaveChanges();
            }
            catch (Exception e)
            {
                //TODO : Add exception to log
            }
        }

        public void RemoveRolesFromUser(long userId)
        {
            var userRoles = GetUserRolesFromUserRolesTable(userId);

            foreach (var role in userRoles)
            {
                _context.UserRoles.Remove(role);
            }

            SaveChanges();
        }

        public List<Roles> GetUserRoles(long userId)
        {
            var roleList = _context.UserRoles
                .Where(x => x.UserId == userId)
                .Select(x => x.RoleId)
                .ToList();

            var roles = new List<Roles>();


            foreach (var r in roleList)
            {
                roles.Add(Get(r));
            }

            return roles;
        }

        public List<UserRoles> GetUserRolesFromUserRolesTable(long userId)
        {
            return _context.UserRoles.Where(x => x.UserId == userId).ToList();
        }

        public List<RoleViewModel> GetRolesForRolesGrid()
        {
            var roles = new List<RoleViewModel>();

            foreach (var r in _context.Roles)
            {
                roles.Add(new RoleViewModel()
                {
                    Name = r.Name,
                    Id = r.Id,
                    UserCount = _context.UserRoles.Count(x => x.RoleId == r.Id)
                });
            }

            return roles;
        }

        public bool IsRoleNameDuplicate(string name)
        {
            return _context.Roles.Any(x => x.Name == name);
        }

        public bool IsRoleNameDuplicateForEdit(long roleId, string name)
        {
            return _context.Roles.Any(x => x.Name == name && x.Id != roleId);
        }

        public Roles CreateNewRole(string name)
        {
            var role = new Roles()
            {
                Name = name,
                CreationDate = DateTime.Now
            };

            _context.Roles.Add(role);
            _context.SaveChanges();

            return role;
        }

        public OperationResult UpdateRoleInformation(long roleId, string name, List<long> permissions)
        {
            try
            {
                var role = Get(roleId);
                role.Name = name;

                if (DoesRoleHasAnyPermissions(roleId))
                {
                    RemovePermissionsFromRole(roleId);
                }

                Update(role);

                AddPermissionsToRole(permissions, roleId);

                return new OperationResult()
                {
                    IsSuccessful = true
                };
            }
            catch (Exception e)
            {
                return new OperationResult()
                {
                    IsSuccessful = false,
                    Exception = e
                };
            }
        }

        public bool DoesRoleHasAnyPermissions(long roleId)
        {
            return _context.RolePermissions.Any(x => x.RoleId == roleId);
        }

        public Roles GetDefaultRole()
        {
            return _context.Roles.FirstOrDefault(x => x.Name == "پیش فرض");
        }

        public OperationResult DeleteAllRolePermissions(long roleId)
        {
            try
            {
                foreach (var p in _context.RolePermissions.Where(x=>x.RoleId == roleId))
                {
                    _context.RolePermissions.Remove(p);
                }
                SaveChanges();

                return new OperationResult()
                {
                    IsSuccessful = true
                };
            }
            catch (Exception e)
            {
                return new OperationResult()
                {
                    IsSuccessful = false,
                    Exception = e
                };
            }
        }

        public OperationResult MigrateAllUsersToDefaultRole(long roleId)
        {
            try
            {
                var role = GetDefaultRole();

                foreach (var ur in _context.UserRoles
                             .Where(x=>x.RoleId == roleId))
                {
                    ur.RoleId = role.Id;
                    _context.Update(ur);
                }

                SaveChanges();

                return new OperationResult()
                {
                    IsSuccessful = true
                };
            }
            catch (Exception e)
            {
                return new OperationResult()
                {
                    IsSuccessful = false,
                    Exception = e
                };
            }
        }

        public bool DoesRoleHasAnyUsers(long roleId)
        {
            return _context.UserRoles.Any(x => x.RoleId == roleId);
        }

        #endregion


        #region Permissions
        public List<RolePermissions> GetRolePermissions(long roleId)
        {
            return _context.RolePermissions
                .Where(x => x.RoleId == roleId).ToList();
        }

        public List<PermissionGroupViewModel> GetPermissionsForRoleEdit(long roleId)
        {
            var allPermissions = SystemPermissions.SystemPermissions.AllSystemPermissions;

            var rolePermissions = GetRolePermissions(roleId);

            foreach (var pg in allPermissions)
            {
                foreach (var p in pg.Permissions)
                {
                    if (rolePermissions.Any(x => x.Permission == p.Permission))
                    {
                        p.IsSelected = true;
                    }
                }
            }

            return allPermissions;
        }

        public void AddPermissionsToRole(List<long> permissions, long roleId)
        {
            if (_context.RolePermissions.Any(x => x.RoleId == roleId))
                RemovePermissionsFromRole(roleId);


            foreach (var p in permissions)
            {
                _context.RolePermissions.Add(new RolePermissions()
                {
                    Permission = p,
                    RoleId = roleId
                });
            }

            SaveChanges();
        }

        public void RemovePermissionsFromRole(long roleId)
        {
            var permissions = GetRolePermissions(roleId);

            foreach (var p in permissions)
            {
                _context.RolePermissions.Remove(p);
            }

            SaveChanges();
        }

        public bool CheckPermission(long permission, long userId)
        {
            var userPermissions = GetUserPermissions(userId);

            return userPermissions.Any(x => x.Permission == permission);
        }

        public List<RolePermissions> GetUserPermissions(long userId)
        {
            var roles = GetUserRoles(userId);

            List<RolePermissions> permissions = new List<RolePermissions>();

            foreach (var r in roles)
            {
                var rolePermissions = GetRolePermissions(r.Id);

                GetRolePermissions(r.Id).ForEach(x => permissions.Add(x));
            }

            return permissions;
        }

        public List<long> GetLoggedInUserPermissions()
        {
            var context = GetHttpContext();

            var permissionClaim = context.User.FindFirst("permissions")?.Value;

            if (permissionClaim != null)
            {
                return JsonSerializer.Deserialize<List<long>>(permissionClaim);
            }

            return new List<long>();
        }

        public void AddUserPermissionsToHttpContext(List<long> permissions)
        {
            var context = GetHttpContext();

            string permissionsJson = JsonSerializer.Serialize(permissions);

            var claim = new Claim("permissions", permissionsJson);

            var identity = context.User.Identity as ClaimsIdentity;

            identity.AddClaim(claim);
        }

        public HttpContext GetHttpContext()
        {
            return _httpContext.HttpContext;
        }

        public bool VerifyOTPCode(string otpCode, string phoneNumber)
        {
            return _context.OTPCodes
                .Any(x => x.PhoneNumber == phoneNumber
                          && x.OTPCode == otpCode
                          && x.IsActive);
        }

        public OTPCodes GetOTPCodeByPhoneNumber(string phoneNumber)
        {
            return _context.OTPCodes.FirstOrDefault(x =>
                x.PhoneNumber == phoneNumber && x.IsActive && x.ExpireDate > DateTime.Now);
        }

        public OTPCodes GetOTPCode(string otp)
        {
            return _context.OTPCodes.FirstOrDefault(x => x.OTPCode == otp);
        }

        public void DeleteOTPCode(OTPCodes code)
        {
            _context.OTPCodes.Remove(code);
            SaveChanges();
        }

        public void DeleteOTPCode(long id)
        {
            var otp = _context.OTPCodes.Find(id);
            _context.OTPCodes.Remove(otp);
            SaveChanges();
        }

        public string GenerateOTPCode(Users user)
        {
            var otp = OTPCodeGenerator.GenerateOTPCode();

            _context.OTPCodes.Add(new OTPCodes()
            {
                UserId = user.Id,
                IsActive = true,
                OTPCode = otp,
                PhoneNumber = user.PhoneNumber,
                ExpireDate = DateTime.Now.AddSeconds(120)
            });

            SaveChanges();

            return otp;
        }

        public OTPCodes GetOTPCodeById(long id)
        {
            return _context.OTPCodes.Find(id);
        }

        public void UpdateOTP(OTPCodes code)
        {
            _context.OTPCodes.Update(code);
            SaveChanges();
        }

        public void DeactivateOTP(long id)
        {
            var otp = GetOTPCodeById(id);
            otp.IsActive = false;
            UpdateOTP(otp);
        }

        public void DeactivateOTP(string otp)
        {
            var code = GetOTPCode(otp);
            code.IsActive = false;
            UpdateOTP(code);
        }
        #endregion
    }
}
