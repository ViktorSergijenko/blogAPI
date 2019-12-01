using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
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
        public async Task<Object> PostApplicationUser(UserRegisterDTO model)
        {

            model.RoleName = "Admin";
            var applicationUser = new User()
            {
                UserName = model.NickName,
                Email = model.Email,
                FullName = model.FullName
            };

            try
            {
                var result = await _userManager.CreateAsync(applicationUser, model.Password);
                await _userManager.AddToRoleAsync(applicationUser, model.RoleName);
                return Ok(result);
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
            var user = await _userManager.FindByEmailAsync(model.Email);
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
        [Authorize]
        [Route("GetProfile")]
        public UserProfileVM GetUserClaims()
        {
            Guid companyId = new Guid();
            var identityClaims = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identityClaims.Claims;
            UserProfileVM model = new UserProfileVM()
            {
                FullName = identityClaims.FindFirst("FullName").Value,
                Id = Guid.Parse(identityClaims.FindFirst("UserID").Value),
                RoleName = identityClaims.FindFirst(ClaimTypes.Role).Value,
            };
            Guid.TryParse(identityClaims.FindFirst("UserID").Value, out companyId);
            model.CompanyId = companyId;

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
        [Authorize(Policy = AuthConstants.GlobalAdminPolicy)]
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
        [Authorize(Roles = "Company Director")]
        [Route("ForDirector")]
        public string GetCustomer()
        {
            return "Web method for Customer";
        }

        [HttpGet]
        [Authorize(Roles = "Global Admin,Company Director")]
        [Route("ForAdminOrCustomer")]
        public string GetForAdminOrCustomer()
        {
            return "Web method for Admin or Customer";
        }
    }
}