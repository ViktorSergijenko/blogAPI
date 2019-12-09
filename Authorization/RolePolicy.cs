using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using ProjectStructure.Constants;
using ProjectStructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectStructure.Authorization
{
    // https://docs.asp.net/en/latest/security/authorization/policies.html

    /// <summary>
    /// Access only to Global Admin
    /// </summary>
    public class OnlyGlobalAdminRequirement : AuthorizationHandler<OnlyGlobalAdminRequirement>, IAuthorizationRequirement
    {
        public OnlyGlobalAdminRequirement()
        {
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OnlyGlobalAdminRequirement requirement)
        {

            if (!context.User.IsInRole(AuthConstants.Role_GlobalAdmin))
            {

                context.Fail();
                return Task.FromResult(0);
            }
            context.Succeed(requirement);
            return Task.FromResult(0);
        }
    }


    /// <summary>
    /// Access for company director and global admin
    /// </summary>
    public class AtleastModerator : AuthorizationHandler<AtleastModerator>, IAuthorizationRequirement
    {
        public AtleastModerator()
        {
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AtleastModerator requirement)
        {
            var a = context.User.Identity.Name;
            if (!context.User.IsInRole(AuthConstants.Role_GlobalAdmin)
                && !context.User.IsInRole(AuthConstants.Role_Moderator)
            )
            {
                context.Fail();
                return Task.FromResult(0);
            }
            context.Succeed(requirement);
            return Task.FromResult(0);
        }
    }
    /// <summary>
    /// Access for company director and global admin
    /// </summary>
    public class AtLeastRegularUser : AuthorizationHandler<AtLeastRegularUser>, IAuthorizationRequirement
    {
        public AtLeastRegularUser()
        {
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AtLeastRegularUser requirement)
        {
            if (!context.User.IsInRole(AuthConstants.Role_GlobalAdmin)
                && !context.User.IsInRole(AuthConstants.Role_Moderator)
                && !context.User.IsInRole(AuthConstants.Role_RegularUser)
            )
            {
                context.Fail();
                return Task.FromResult(0);
            }
            context.Succeed(requirement);
            return Task.FromResult(0);
        }
    }
}

