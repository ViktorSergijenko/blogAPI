using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ProjectStructure.Constants;
using ProjectStructure.DTO;
using ProjectStructure.Models;
using ProjectStructure.Services;

namespace ProjectStructure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : BaseApiController
    {
        private PostService _postService { get; set; }

        public PostController(IServiceProvider services)
             : base(services)
        {
            _postService = services.GetRequiredService<PostService>();
        }

        [HttpGet]
        public async Task<ActionResult<List<Post>>> Get()
        {
            return Ok(await _postService.Get());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Post>> GetById(Guid id)
        {
            return Ok(await _postService.GetById(id));
        }


        [HttpPost]
        [Authorize(Policy = AuthConstants.AtLeastRegularUser)]
        public async Task<ActionResult> Save([FromBody] PostDTO regionDTO)
        {
            if (ModelState.IsValid)
            {
                var region = await _postService.Save(regionDTO);
                return Ok(region);
            }
            throw new Exception();
        }


        [HttpDelete("{id}")]
        [Authorize(Policy = AuthConstants.AtLeastRegularUser)]
        public async Task<ActionResult> Delete(Guid id)
        {
            return Ok(await _postService.Delete(id));
        }
    }
}