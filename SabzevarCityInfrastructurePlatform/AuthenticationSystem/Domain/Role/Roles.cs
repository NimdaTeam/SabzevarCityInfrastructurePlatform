using _0_Framework.EntityBase;
using AuthenticationSystem.Domain.RolePermission;
using AuthenticationSystem.Domain.User;

namespace AuthenticationSystem.Domain.Role
{
    public class Roles : EntityBase
    {
        public string Name { get; set; }


        #region Relations 


        public virtual List<Users> Users { get; set; }

        public virtual List<RolePermissions> RolePermissions { get; set; }

        public virtual List<UserRoles> UserRoles { get; set; }

        #endregion
    }
}
