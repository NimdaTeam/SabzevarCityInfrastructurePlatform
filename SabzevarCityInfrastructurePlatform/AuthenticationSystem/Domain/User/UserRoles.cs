using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _0_Framework.EntityBase;
using AuthenticationSystem.Domain.Role;

namespace AuthenticationSystem.Domain.User
{
    public class UserRoles:EntityBase
    {
        [ForeignKey("Users")]
        public long UserId { get; set; }

        [ForeignKey("Roles")]
        public long RoleId { get; set; }

        #region Relations

        
        public virtual Users Users { get; set; }

        public virtual Roles Roles { get; set; }

        #endregion
    }
}
