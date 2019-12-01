using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using ProjectStructure.Constants;
using ProjectStructure.Contracts;
using ProjectStructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ProjectStructure.Interceptors
{
    public class ProjectAppContext : IAppContext
    {
        protected IServiceProvider DI;
        private IMemoryCache MemoryCache { get; set; }

        public ProjectAppContext(IServiceProvider services)
        {
            DI = services;
            // Removed this one, because it's not doing anything usefull
            // and result aren't used anywhere
            //Utility.GetUrlBaseFromRequest(Context?.Request);
            MemoryCache = services.GetRequiredService<IMemoryCache>();
        }

        /// <summary>
        /// HttpContext relevant for current http request
        /// </summary>
        public HttpContext Context => DI.GetService<IHttpContextAccessor>().HttpContext;
        /// <summary>
        /// Entity Framework context relevant for current application context
        /// </summary>
        public ProjectAppContext DB => DI.GetService<ProjectAppContext>();



        public Guid? CurrentUserId
        {
            get
            {
                // First of all we're checking if the value is already cached and retrieving it
                Guid? CurrentUserId;
                if (MemoryCache.TryGetValue(ProjectConstants.CurrentUserIdCachePath, out CurrentUserId))
                {
                    return CurrentUserId;
                }
                // Overwise, we're getting the first leasehold claim form users token and saving it to cache
                var defaultUserId = GetIdClaim(AuthConstants.UserIdClaimType);
                if (defaultUserId.HasValue)
                {
                    MemoryCache.Set(ProjectConstants.CurrentUserIdCachePath, defaultUserId);
                    return defaultUserId;
                }
                // Returning null if leasehold claim is not present in current users token
                return null;
            }
            set
            {
                // Allowing to change current leasehold id, but ensuring it's present in users claims
                // If leasehold id is present in users claims, then user can have access to it
                if (GetClaimsGuidIdsList(AuthConstants.UserIdClaimType).Any(x => x == value))
                {
                    // If leasehold id is valid for this user, then storing it in cache
                    MemoryCache.Set(ProjectConstants.CurrentUserIdCachePath, value);
                    // The current house id is dependant on leasehold, so we need to ensure it matches new leasehold
                    // Setting it to Guid.Empty value will trigger house id recalculation proccess which will ensure correct house id

                }
                else
                {
                    // If new leasehold id isn't present in users token, 
                    // then users has no permissions to see this particular leasehold
                    throw ProjectExceptions.Forbidden;
                }
            }
        }

        /// <summary>
        /// This method used only for guids claims
        /// </summary>
        /// <param name="claimType">Claim type for guid claims (any claim with ids) </param>
        /// <returns>Guid list</returns>
        private List<Guid> GetClaimsGuidIdsList(string claimType)
        {
            // Check have we context
            var userContext = Context?.User;
            // If user context is null, returning empty list 
            // to avoid null reference exception due to lack of null checks in dependant code
            if (userContext == null) { return new List<Guid>(); }
            // Find all needed claims
            var claimList = userContext.Claims.Where(x => x.Type == claimType).ToList();
            // Parse it to guid list
            return claimList.Select(x => GetIdFromString(x.Value)).Where(x => x.HasValue).Select(x => x.Value).ToList();
        }

        /// <summary>
        /// Вспомогательный метод, возвращающий числовое значение утверждения с заданным типом
        /// </summary>
        /// <param name="claimType">Тип утверждения, значение которого нужно получить</param>
        /// <returns></returns>
        private Guid? GetIdClaim(string claimType)
        {
            var p = Context?.User;
            if (p == null) { return null; }
            var c = p.Claims.Where(x => x.Type == claimType).FirstOrDefault();
            if (c != null && !string.IsNullOrEmpty(c.Value))
                return Guid.Parse(c.Value);
            return null;
        }

        private Guid? GetIdFromString(string id)
        {
            if (String.IsNullOrEmpty(id)) { return null; }
            Guid.TryParse(id, out Guid tmp);
            if (tmp == Guid.Empty) { return null; }
            return tmp;
        }

        /// <summary>
        /// Указывает, является ли авторизованный пользователь глобальным администратором
        /// </summary>
        public bool IsAdmin => Context != null ? Context.User.IsInRole(AuthConstants.Role_GlobalAdmin) : false;

        /// <summary>
        /// Идентификатор авторизованного пользователя
        /// </summary>
        public Guid? UserId => GetIdClaim(ClaimTypes.NameIdentifier);

    }
}
