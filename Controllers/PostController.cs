using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
    [ApiController]
    public class PostController : BaseApiController
    {
        private PostService _postService { get; set; }

        public PostController(IServiceProvider services)
             : base(services)
        {
            _postService = services.GetRequiredService<PostService>();
        }

        [HttpGet("new")]
        public async Task<ActionResult<List<Post>>> Get()
        {
            var posts = await _postService.Get(null, true, x => x.Include(y => y.User).Include(y => y.Likes));
            return Ok(posts.OrderByDescending(x => x.PostedAt));
        }
        [HttpGet("best")]
        public async Task<ActionResult<List<Post>>> GetBestPosts()
        {
            var posts = await _postService.Get(null, true, x => x.Include(y => y.User).Include(y => y.Likes));
            return Ok(posts.OrderByDescending(x => x.Likes.Count()));
        }
        [HttpGet("my")]
        public async Task<ActionResult<List<Post>>> GetUserPosts()
        {
            var posts = await _postService.Get(null, true, x => x.Include(y => y.User).Include(y => y.Likes));
            return Ok(posts.Where(x => x.UserId == AppCtx.CurrentUserId).OrderByDescending(x => x.Likes.Count()));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Post>> GetById(Guid id)
        {
            return Ok(await _postService.GetById(id, include: x => x.Include(y => y.User).Include(y => y.Likes).Include(y => y.Comments)));
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