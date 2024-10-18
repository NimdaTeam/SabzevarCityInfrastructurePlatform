using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthenticationSystem.Services;
using AuthenticationSystem.Services.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AuthenticationSystem.Infrastructure
{
    public static class AuthentionSystemBootstrapper
    {
        public static void Configure(IServiceCollection services, string connectionString)
        {

            services.AddTransient<IPermissionRepository, PermissionService>();
            services.AddTransient<IRoleRepository, RoleService>();
            services.AddTransient<IUserRepository, UserService>();

            services.AddDbContext<AuthenticationSystemContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });
        }
    }
}
