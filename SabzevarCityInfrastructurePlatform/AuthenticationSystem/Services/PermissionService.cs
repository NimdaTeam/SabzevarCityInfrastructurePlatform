using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using _0_Framework.Utilities.Generators;
using AuthenticationSystem.Domain.RolePermission;
using AuthenticationSystem.Domain.User;
using AuthenticationSystem.Infrastructure;
using AuthenticationSystem.Services.Repositories;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace AuthenticationSystem.Services
{
    public class PermissionService : IPermissionRepository
    {
        private readonly IRoleRepository _roleService;

        private readonly AuthenticationSystemContext _context;

        private readonly IHttpContextAccessor _httpContext;

        public PermissionService(IRoleRepository roleService, AuthenticationSystemContext context, IHttpContextAccessor httpContext)
        {
            _roleService = roleService;
            _context = context;
            _httpContext = httpContext;
        }


        public List<RolePermissions> GetRolePermissions(long roleId)
        {
            return _context.RolePermissions
                .Where(x => x.RoleId == roleId).ToList();
        }

        public void AddPermissionsToRole(List<long> permissions, long roleId)
        {
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
            var roles = _roleService.GetUserRoles(userId);

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

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

    }
}
