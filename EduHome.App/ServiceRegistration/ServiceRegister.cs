using EduHome.App.Context;
using EduHome.App.Services.Implementations;
using EduHome.App.Services.Interfaces;
using EduHome.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EduHome.App.ServiceRegistration
{
    public static class ServiceRegister
    {
        public static void Register(this IServiceCollection service, IConfiguration configuration)
        {
            service.AddScoped<IMailService, MailService>();

            service.AddIdentity<AppUser, IdentityRole>()
                   .AddDefaultTokenProviders()
                   .AddEntityFrameworkStores<EduHomeDbContext>();
            service.Configure<IdentityOptions>(options =>
            {
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 3;
                options.Lockout.AllowedForNewUsers = true;
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;
            });
            service.AddDbContext<EduHomeDbContext>(opt =>
            {
                opt.UseSqlServer(configuration.GetConnectionString("Default"));
            });

        }
    }
}
