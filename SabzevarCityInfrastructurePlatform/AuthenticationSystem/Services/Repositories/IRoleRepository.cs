using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _0_Framework.GenericRepositoy.Interface;
using AuthenticationSystem.Domain.Role;
using AuthenticationSystem.Domain.User;

namespace AuthenticationSystem.Services.Repositories
{
    public interface IRoleRepository:IRepository<long,Roles>
    {
        bool IsUserInRole(string phoneNumber, long roleId);

        void AddRoleToUser(List<long> roles , long  userId);

        void RemoveRolesFromUser(long userId);

        List<Roles> GetUserRoles(long userId);


        List<UserRoles> GetUserRolesFromUserRolesTable(long userId);
    }
}
