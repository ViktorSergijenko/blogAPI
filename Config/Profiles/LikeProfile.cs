using AutoMapper;
using ProjectStructure.DTO;
using ProjectStructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectStructure.Config.Profiles
{
    public class LikeProfile : Profile
    {
        public LikeProfile()
        {
            CreateMap<Like, Like>();

            CreateMap<LikeDTO, Like>()
                 .ForMember(x => x.IsDeleted, o => o.Ignore())
                 ;
        }
    }
}
