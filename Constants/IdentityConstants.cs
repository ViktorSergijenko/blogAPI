using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectStructure.Constants
{
    public class AuthConstants
    {
        /// Configuration path to value of should PII(sensitive information)
        /// shown or not (debug purposes)
        /// </summary>
        public const string ShowPII = "AppSettings:ShowPII";

        #region Identity password settings
        public const string RequiredLength = "IdentityPasswordSettings:RequiredLength";
        public const string RequireDigit = "IdentityPasswordSettings:RequireDigit";
        public const string RequireLowercase = "IdentityPasswordSettings:RequireLowercase";
        public const string RequireNonAlphanumeric = "IdentityPasswordSettings:RequireNonAlphanumeric";
        public const string RequireUppercase = "IdentityPasswordSettings:RequireUppercase";
        public const string RequiredUniqueChars = "IdentityPasswordSettings:RequiredUniqueChars";
        #endregion Identity password settings

        #region Policies
        /// <summary>
        /// Policy name, that grants access to all authorized users
        /// </summary>
        public const string AuthorizedPolicy = "Authorized";
        /// <summary>
        /// Policy name, that grants access only to global administrators
        /// </summary>
        public const string GlobalAdminPolicy = "GlobalAdmin";
        /// <summary>
        /// Policy name, that grants access only to moderator
        /// </summary>
        public const string AtLeastModerator = "AtLeastModerator";
        public const string AtLeastRegularUser = "AtLeastRegularUser";
        #endregion Policies

        #region Roles

        public const string Role_GlobalAdmin = "Global Admin";
        public const string Role_Moderator = "Moderator";
        public const string Role_RegularUser = "RegularUser";
        #endregion Roles

        #region Claims
        /// <summary>
        /// Claim type for user full name
        /// </summary>
        public const string FullNameClaimType = "FullName";
        /// <summary>
        /// Claim type for user full name
        /// </summary>
        public const string UserIdClaimType = "UserID";


        #endregion Claims

        public const string AUDIENCE = "http://localhost:81/testProject1/"; // потребитель токена
        const string KEY = "vfvrbylhzybr18021";   // ключ для шифрации
        public const int LIFETIME = 12000; // время жизни токена - 50 минут
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
