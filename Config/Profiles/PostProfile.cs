using AutoMapper;
using ProjectStructure.DTO;
using ProjectStructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectStructure.Config.Profiles
{
    public class PostProfile : Profile
    {
        public PostProfile()
        {
            CreateMap<Post, Post>();

            CreateMap<PostDTO, Post>()
                 .ForMember(x => x.IsDeleted, o => o.Ignore())
                 ;
        }
    }
}
