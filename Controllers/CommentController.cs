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
    public class CommentController : BaseApiController
    {
        private CommentService _commentService { get; set; }

        public CommentController(IServiceProvider services)
             : base(services)
        {
            _commentService = services.GetRequiredService<CommentService>();
        }

        [HttpGet("post/{id}")]
        public async Task<ActionResult<List<Comment>>> Get(Guid id)
        {
            var comments = await _commentService.Get(null, true, x => x.Include(y => y.User));
            return Ok(comments.Where(x => x.PostId == id).OrderByDescending(x => x.CommentedAt));
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Post>> GetById(Guid id)
        {
            return Ok(await _commentService.GetById(id));
        }

        [HttpPost]
        [Authorize(Policy = AuthConstants.AtLeastRegularUser)]
        public async Task<ActionResult> Save([FromBody] CommentDTO regionDTO)
        {
            if (ModelState.IsValid)
            {
                var region = await _commentService.Save(regionDTO);
                return Ok(region);
            }
            throw new Exception();
        }


        [HttpDelete("{id}")]
        [Authorize(Policy = AuthConstants.AtLeastRegularUser)]
        public async Task<ActionResult> Delete(Guid id)
        {
            return Ok(await _commentService.Delete(id));
        }
    }
}