using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectStructure.DTO
{
    public class UserRegisterDTO
    {
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string AvatarImageBase64 { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Age { get; set; }
        public Guid? CompanyId { get; set; }
        public string Password { get; set; }
    }
    public class LoginContext
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }

    }
}
