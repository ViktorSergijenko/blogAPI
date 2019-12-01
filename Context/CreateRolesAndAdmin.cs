using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectStructure.Constants;
using ProjectStructure.Models;

namespace ProjectStructure.Context
{
    public class CreateRolesAndAdmin
    {
        protected ProjectContext DB { get; private set; }
        private readonly UserManager<User> userManager;
        private readonly RoleManager<Role> roleManager;
        public CreateRolesAndAdmin(IServiceProvider service, RoleManager<Role> roleManager, UserManager<User> userManager)
        {
            DB = service.GetRequiredService<ProjectContext>();
            this.userManager = userManager;
            this.roleManager = roleManager;

        }
        public static async Task PopulateInitialData(IServiceProvider serviceProvider)
        {

            var userManager = serviceProvider.GetService<UserManager<User>>();
            var roleManager = serviceProvider.GetService<RoleManager<Role>>();
            var dbContext = serviceProvider.GetService<ProjectContext>();

            #region Root user creation
            var sysAdmin = await userManager.FindByNameAsync("root");
            // We also should check for if any user is assigned to global admin role
            var globalAdminRole = await roleManager.FindByNameAsync(AuthConstants.Role_GlobalAdmin);
            bool globalAdminExists = false;
            if (globalAdminRole != null)
            {
                globalAdminExists = await dbContext.UserRoles.AnyAsync(x => x.RoleId == globalAdminRole.Id);
            }
            if (sysAdmin == null && !globalAdminExists)
            {
                sysAdmin = new User
                {
                    UserName = "root",
                    FullName = "System Admin",
                    FirstName = "System",
                    LastName = "Admin",
                    LockoutEnabled = false,
                    Email = "support@storage.lv",
                    EmailConfirmed = true
                };
                var userCreationResult = await userManager.CreateAsync(sysAdmin, "P@ssw0rd");
                if (userCreationResult.Succeeded)
                {
                    if (!await roleManager.RoleExistsAsync(AuthConstants.Role_GlobalAdmin))
                    {
                        var roleCreationResult = await roleManager.CreateAsync(new Role(AuthConstants.Role_GlobalAdmin));
                        if (roleCreationResult.Succeeded)
                        {
                            var roleAdditionResult = await userManager.AddToRoleAsync(sysAdmin, AuthConstants.Role_GlobalAdmin);
                        }
                    }
                }
            }
            #endregion Root user creation

            #region Default role creation
            if (!await roleManager.RoleExistsAsync(AuthConstants.Role_GlobalAdmin))
            {
                var roleCreationResult = await roleManager.CreateAsync(new Role(AuthConstants.Role_GlobalAdmin));
            }
            if (!await roleManager.RoleExistsAsync(AuthConstants.Role_Moderator))
            {
                var roleCreationResult = await roleManager.CreateAsync(new Role(AuthConstants.Role_Moderator));
            }
            if (!await roleManager.RoleExistsAsync(AuthConstants.Role_RegularUser))
            {
                var roleCreationResult = await roleManager.CreateAsync(new Role(AuthConstants.Role_RegularUser));
            }
            #endregion Default role creation

            #region Initial languages

            if (!dbContext.Languages.Any())
            {
                var en = new Language()
                {
                    Code = "en",
                    Title = "English",
                    
                };
                var lv = new Language()
                {
                    Code = "lv",
                    Title = "Latvian",
                    IsDefault = true,
                };
                var ru = new Language()
                {
                    Code = "ru",
                    Title = "Russian",
                };
                dbContext.Languages.AddRange(en, lv, ru);
                dbContext.SaveChanges();
            }

            #endregion Initial languages


        }

        public static async Task InitializeIdentityDbAsync(IServiceProvider serviceProvider)
        {

            var db = serviceProvider.GetService<ProjectContext>();
            // Create the database if it does not already exist
            await db.Database.EnsureCreatedAsync();

            // Populate database with data
            await PopulateDatabaseWithDemoData(serviceProvider);
        }

        private static async Task PopulateDatabaseWithDemoData(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetService<UserManager<User>>();
            var roleManager = serviceProvider.GetService<RoleManager<Role>>();
            var dbContext = serviceProvider.GetService<ProjectContext>();

            await PopulateInitialData(serviceProvider);

            #region Example for demo data creation
            //if (!dbContext.Actions.Any())
            //{
            //    dbContext.Actions.Add(new Act()
            //    {

            //    }
            //}
            #endregion

            await dbContext.SaveChangesAsync();
        }
    }
}
