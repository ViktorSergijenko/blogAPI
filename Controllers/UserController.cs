using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ProjectStructure.Constants;
using ProjectStructure.DTO;
using ProjectStructure.Models;
using ProjectStructure.ViewModels;

namespace ProjectStructure.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("MyPolicy")]
    [ApiController]
    public class UserController : BaseApiController
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _singInManager;


        public UserController(IServiceProvider services,UserManager<User> userManager, SignInManager<User> signInManager)
            : base(services)
        {
            _userManager = userManager;
            _singInManager = signInManager;
        }

        [HttpPost]
        [Route("Register")]
        //POST : /api/ApplicationUser/Register
        public async Task<ActionResult<User>> PostApplicationUser(UserRegisterDTO model)
         {

            var applicationUser = new User();
            applicationUser.RoleName = "RegularUser";
            Mapper.Map(model, applicationUser);

            try
            {
                var result = await _userManager.CreateAsync(applicationUser, model.Password);
                await _userManager.AddToRoleAsync(applicationUser, applicationUser.RoleName);
                return Ok(model);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        [Route("Login")]
        //POST : /api/ApplicationUser/Login
        public async Task<IActionResult> Login(LoginContext model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email) ?? await _userManager.FindByNameAsync(model.UserName);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                //Get role assigned to the user
                var role = await _userManager.GetRolesAsync(user);
                IdentityOptions _options = new IdentityOptions();

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim("UserID",user.Id.ToString()),
                        new Claim(ClaimTypes.Role, role.FirstOrDefault()),
                        new Claim("FullName",role.FirstOrDefault())
                    }),
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes("vfvrbylhzybr18021")), SecurityAlgorithms.HmacSha256Signature)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                var token = tokenHandler.WriteToken(securityToken);
                return Ok(new { token });
            }
            else
                return BadRequest(new { message = "Username or password is incorrect." });
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Route("GetProfile")]
        public UserProfileVM GetUserClaims()
        {
            var identityClaims = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identityClaims.Claims;
            UserProfileVM model = new UserProfileVM()
            {
                FullName = identityClaims.FindFirst("FullName").Value,
                Id = Guid.Parse(identityClaims.FindFirst("UserID").Value),
                RoleName = identityClaims.FindFirst(ClaimTypes.Role).Value,
            };
            return model;
        }

        [HttpGet]
        [Authorize]
        //GET : /api/UserProfile
        public async Task<Object> GetUserProfile()
        {
            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId);
            return new
            {
                user.FullName,
                user.Email,
                user.UserName
            };
        }

        [HttpGet]
        [Authorize(Policy = AuthConstants.AtLeastModerator)]
        [Route("ForAdmin")]
        public string GetForAdmin()
        {
            return "Web method for Admin";
        }
        [HttpGet]
        [Authorize(Roles = "Global Admin")]
        [Route("ForAdmin1")]
        public string GetForAdmin1()
        {
            return "Web method for Admin";
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Route("ForAdmin12")]
        public string GetForAdmin12()
        {
            return "Web method for Admin";
        }
        [HttpGet]
        [Route("ForAdmin123")]
        public string GetForAdmin123()
        {
            return "Web method for Admin";
        }

        [HttpGet]
        [Authorize(Roles = AuthConstants.AuthorizedPolicy)]
        [Route("ForDirector")]
        public string GetCustomer()
        {
            return "Web method for Customer";
        }

        [HttpGet]
        [Authorize(Roles = "Global Admin, Moderator")]
        [Route("ForAdminOrCustomer")]
        public string GetForAdminOrCustomer()
        {
            return "Web method for Admin or Customer";
        }
    }
}