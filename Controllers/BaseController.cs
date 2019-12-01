using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ProjectStructure.Context;
using ProjectStructure.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectStructure.Controllers
{
    [Route("~/api/[controller]")]
    public class BaseApiController : Controller
    {
        protected ProjectContext DB { get; private set; }
        protected IAppContext AppCtx { get; private set; }
        protected IAuthorizationService Auth { get; private set; }
        public BaseApiController(IServiceProvider services)
        {
            DB = services.GetRequiredService<ProjectContext>();
            AppCtx = services.GetRequiredService<IAppContext>();
            Auth = services.GetRequiredService<IAuthorizationService>();

        }
    }
}
