using IdentityModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using ProjectStructure.Authorization;
using ProjectStructure.Constants;
using ProjectStructure.Context;
using ProjectStructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthConstants = ProjectStructure.Constants.AuthConstants;

namespace ProjectStructure.Startups
{
    public static class IdentityStartup
    {
        public static void Configure(IApplicationBuilder app, IConfiguration config)
        {
            app.UseAuthentication();
            app.UseMvcWithDefaultRoute();
        }

        public static void ConfigureServices(IServiceCollection services, IHostingEnvironment hEnv, IConfiguration config)
        {

            // Configure Identity
            services.AddIdentity<User,Role>()
                .AddRoleManager<RoleManager<Role>>()
                .AddEntityFrameworkStores<ProjectContext>()
                .AddDefaultTokenProviders();



            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = Boolean.TryParse(config[AuthConstants.RequireDigit], out bool requireDigitBool);
                options.Password.RequiredLength = Int32.TryParse(config[AuthConstants.RequiredLength], out int requiredLength) ? requiredLength : 6;
                options.Password.RequireNonAlphanumeric = Boolean.TryParse(config[AuthConstants.RequireNonAlphanumeric], out bool requireNonAlphanumericBool);
                options.Password.RequireUppercase = Boolean.TryParse(config[AuthConstants.RequireUppercase], out bool requireUppercaseBool);
                options.Password.RequireLowercase = Boolean.TryParse(config[AuthConstants.RequireLowercase], out bool requireLowercaseBool);
                options.Password.RequiredUniqueChars = Int32.TryParse(config[AuthConstants.RequiredUniqueChars], out int requiredUniqueChars) ? requiredUniqueChars : 0;
            });
            var key = Encoding.UTF8.GetBytes("vfvrbylhzybr18021".ToString());
            services.AddAuthentication(options =>
            {
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                    .AddJwtBearer(options =>
                    {
                        options.RequireHttpsMetadata = false;
                        options.SaveToken = false;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(key),
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ClockSkew = TimeSpan.Zero
                        };
                    })
                    ;
            services.AddAuthorization(ao => ao.AddPolicy(AuthConstants.AuthorizedPolicy, p => p.RequireAuthenticatedUser()));
            services.AddAuthorization(ao => ao.AddPolicy(AuthConstants.GlobalAdminPolicy, p => p.AddRequirements(new OnlyGlobalAdminRequirement())));
            services.AddAuthorization(ao => ao.AddPolicy(AuthConstants.AtLeastModerator, p => p.AddRequirements(new AtleastModerator())));
            services.AddAuthorization(ao => ao.AddPolicy(AuthConstants.AtLeastRegularUser, p => p.AddRequirements(new AtLeastRegularUser())));
        }
    }
}
