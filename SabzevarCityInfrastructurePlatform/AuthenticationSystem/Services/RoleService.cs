using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _0_Framework.GenericRepositoy.Service;
using AuthenticationSystem.Domain.Role;
using AuthenticationSystem.Domain.User;
using AuthenticationSystem.Infrastructure;
using AuthenticationSystem.Services.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationSystem.Services
{
    public class RoleService:RepositoryService<long,Roles> , IRoleRepository
    {
        private readonly AuthenticationSystemContext _context;

        private readonly IUserRepository _userService;

        public RoleService(AuthenticationSystemContext context, IUserRepository userService) : base(context)
        {
            _context = context;
            _userService = userService;
        }

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
    }
}
