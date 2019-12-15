using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectStructure.Constants;
using ProjectStructure.DTO;
using ProjectStructure.Models;
using ProjectStructure.Services;

namespace ProjectStructure.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("MyPolicy")]
    [ApiController]
    public class LikeController : BaseApiController
    {
        private LikeService _likeService { get; set; }

        public LikeController(IServiceProvider services)
             : base(services)
        {
            _likeService = services.GetRequiredService<LikeService>();
        }

        [HttpGet]
        public async Task<ActionResult<List<Like>>> Get()
        {
            return Ok(await _likeService.Get(null,true, x => x.Include(y => y.User)));
        }
        
        [HttpGet("{id}")]
        [Authorize(Policy = AuthConstants.AtLeastRegularUser)]
        public async Task<ActionResult<Like>> GetById(Guid id)
        {
            return Ok(await _likeService.GetById(id));
        }


        [HttpPost]
        [Authorize(Policy = AuthConstants.AtLeastRegularUser)]
        public async Task<ActionResult> Save([FromBody] LikeDTO regionDTO)
        {
            if (ModelState.IsValid)
            {
                var region = await _likeService.Save(regionDTO);
                return Ok(region);
            }
            throw new Exception();
        }


        [HttpDelete("{id}")]
        [Authorize(Policy = AuthConstants.AtLeastRegularUser)]
        public async Task<ActionResult> Delete(Guid id)
        {
            return Ok(await _likeService.Delete(id));
        }
    }
}