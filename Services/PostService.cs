using ProjectStructure.DTO;
using ProjectStructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectStructure.Services
{
    public class PostService : BaseApiService<Post, PostDTO>
    {
        public PostService(IServiceProvider services) : base(services)
        {
        }
    }
}
