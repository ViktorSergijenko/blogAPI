using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectStructure.Constants
{
    public class ProjectConstants
    {
        public const string ApiUrl = "AppSettings:JwtBearerAuthority";
        public const string IssuerKey = "AppSettings:JwtBearerIssuer";
        public const string DashboardUrlKey = "AppSettings:DashboardUrl";
        public const string ApiResourceName = "api1";

        #region MemoryCache 
        public const string CurrentUserIdCachePath = "CurrentUserId";
        public const string CurrentRoleNameCachePath = "CurrentRoleName";
        public const string CurrentRoleIdCachePath = "CurrentRoleId";
        #endregion MemoryCache
    }
}
