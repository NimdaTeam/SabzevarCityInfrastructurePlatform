using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _0_Framework.DTO;
using _0_Framework.GenericRepositoy.Interface;
using AuthenticationSystem.Domain.Role;
using AuthenticationSystem.Domain.RolePermission;
using AuthenticationSystem.Domain.User;
using Microsoft.AspNetCore.Http;

namespace AuthenticationSystem.Services.Repositories
{
    public interface IRoleRepository : IRepository<long, Roles>
    {
        #region Roles

        bool IsUserInRole(string phoneNumber, long roleId);

        void AddRoleToUser(List<long> roles, long userId);

        void RemoveRolesFromUser(long userId);

        List<Roles> GetUserRoles(long userId);


        List<UserRoles> GetUserRolesFromUserRolesTable(long userId);

        List<RoleViewModel> GetRolesForRolesGrid();

        bool IsRoleNameDuplicate(string name);

        bool IsRoleNameDuplicateForEdit(long roleId, string name);

        Roles CreateNewRole(string name);

        OperationResult UpdateRoleInformation(long roleId, string name, List<long> permissions);

        bool DoesRoleHasAnyPermissions(long roleId);

        Roles GetDefaultRole();

        OperationResult DeleteAllRolePermissions(long roleId);

        OperationResult MigrateAllUsersToDefaultRole(long roleId);

        bool DoesRoleHasAnyUsers(long roleId);
        #endregion

        #region Permissions
        List<RolePermissions> GetRolePermissions(long roleId);

        List<PermissionGroupViewModel> GetPermissionsForRoleEdit(long roleId);

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
        #endregion
    }
}
