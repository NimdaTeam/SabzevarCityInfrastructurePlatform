using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _0_Framework.GenericRepositoy.Interface;
using AuthenticationSystem.Domain.RolePermission;
using AuthenticationSystem.Domain.User;
using Microsoft.AspNetCore.Http;
using Microsoft.Identity.Client;

namespace AuthenticationSystem.Services.Repositories
{
    public interface IPermissionRepository
    {
        List<RolePermissions> GetRolePermissions(long roleId);

        void AddPermissionsToRole(List<long> permissions, long roleId);

        void RemovePermissionsFromRole(long roleId);

        bool CheckPermission(long permission, long userId);

        List<RolePermissions> GetUserPermissions(long userId);

        List<long> GetLoggedInUserPermissions();

        void AddUserPermissionsToHttpContext(List<long> permissions);

        HttpContext GetHttpContext();


        bool VerifyOTPCode(string otpCode, string phoneNumber);

        OTPCodes GetOTPCodeByPhoneNumber(string phoneNumber);

        OTPCodes GetOTPCode(string otp);

        void DeleteOTPCode(OTPCodes code);

        void DeleteOTPCode(long id);

        string GenerateOTPCode(Users user);

        OTPCodes GetOTPCodeById(long id);

        void UpdateOTP(OTPCodes code);

        void DeactivateOTP(long id);

        void DeactivateOTP(string otp);

        void SaveChanges();

       

    }
}
