using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthenticationSystem.Domain.Role;
using AuthenticationSystem.Domain.RolePermission;
using AuthenticationSystem.Domain.User;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationSystem.Infrastructure
{
    public class AuthenticationSystemContext :DbContext
    {
        public DbSet<Users> Users { get; set; }

        public DbSet<Roles> Roles { get; set; }

        public DbSet<RolePermissions> RolePermissions { get; set; }

        public DbSet<OTPCodes> OTPCodes { get; set; }

        public DbSet<UserRoles> UserRoles { get; set; }

        public AuthenticationSystemContext(DbContextOptions<AuthenticationSystemContext> options): base(options) { }
    }
}
