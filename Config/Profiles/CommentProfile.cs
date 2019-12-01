using AutoMapper;
using ProjectStructure.DTO;
using ProjectStructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectStructure.Config.Profiles
{
    public class CommentProfile : Profile
    {
        public CommentProfile()
        {
            CreateMap<Comment, Comment>();

            CreateMap<CommentDTO, Comment>()
                 .ForMember(x => x.IsDeleted, o => o.Ignore())
                 ;
        }
    }
}
