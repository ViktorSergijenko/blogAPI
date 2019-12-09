using AutoMapper;
using ProjectStructure.DTO;
using ProjectStructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectStructure.Config.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, User>();
            CreateMap<UserRegisterDTO, User>()
                 .ForMember(x => x.FullName, o => o.MapFrom(s => s.FirstName + " " + s.LastName))
            ;
        }
    }
}
