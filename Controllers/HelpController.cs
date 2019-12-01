using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectStructure.Constants;
using ProjectStructure.Context;
using ProjectStructure.Models;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace ProjectStructure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HelpController : ControllerBase
    {


        protected ProjectContext DB { get; private set; }
        private readonly UserManager<User> userManager;
        private readonly RoleManager<Role> roleManager;

        public  HelpController(IServiceProvider service, RoleManager<Role> roleManager, UserManager<User> userManager)
        {
            DB = service.GetRequiredService<ProjectContext>();
            this.userManager = userManager;
            this.roleManager = roleManager;

        }
       
        // GET api/values
        [HttpGet]
        public async Task<string> Get()
        {

            #region Root user creation

            var sysAdmin = await userManager.FindByNameAsync("root");

            // We also should check for if any user is assigned to global admin role
            var globalAdminRole = await roleManager.FindByNameAsync(AuthConstants.Role_GlobalAdmin);
            bool globalAdminExists = false;
            if (globalAdminRole != null)
            {
                globalAdminExists = await DB.UserRoles.AnyAsync(x => x.RoleId == globalAdminRole.Id);
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
                    Email = "admin@storage.lv",
                    EmailConfirmed = true
                };
                var sysManager = new User
                {
                    UserName = "moderator",
                    FullName = "System Moderator",
                    FirstName = "System Moderator",
                    LastName = "Moderator",
                    LockoutEnabled = false,
                    Email = "moderator@storage.lv",
                    EmailConfirmed = true
                };
                var userCreationResult = await userManager.CreateAsync(sysAdmin, "P@ssw0rd");
                var userCreationResult1 = await userManager.CreateAsync(sysManager, "P@ssw0rd");

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

                if (userCreationResult1.Succeeded)
                {
                    if (!await roleManager.RoleExistsAsync(AuthConstants.Role_Moderator))
                    {
                        var roleCreationResult = await roleManager.CreateAsync(new Role(AuthConstants.Role_Moderator));
                        if (roleCreationResult.Succeeded)
                        {
                            var roleAdditionResult = await userManager.AddToRoleAsync(sysManager, AuthConstants.Role_Moderator);
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

            if (!DB.Languages.Any())
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
                DB.Languages.AddRange(en, lv, ru);
                DB.SaveChanges();
            }

            #endregion Initial languages
            return "All was done";
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
