using ProjectStructure.DTO;
using ProjectStructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectStructure.Services
{
    public class LikeService : BaseApiService<Like, LikeDTO>
    {
        public LikeService(IServiceProvider services) : base(services)
        {

        }
    }
}
