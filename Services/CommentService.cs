using ProjectStructure.DTO;
using ProjectStructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectStructure.Services
{
    public class CommentService : BaseApiService<Comment, CommentDTO>
    {
        public CommentService(IServiceProvider services) : base(services)
        {
        }

        public override async Task IsNewEntity(CommentDTO CommentDTO, Comment entityToSave)
        {
            entityToSave.CommentedAt = DateTime.Now;
        }
    }
}
