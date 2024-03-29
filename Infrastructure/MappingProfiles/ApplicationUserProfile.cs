using AutoMapper;
using Domain.Models;
using Infrastructure.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.MappingProfiles
{
    public class ApplicationUserProfile : Profile
    {
        public ApplicationUserProfile() 
        {
            CreateMap<User, ApplicationUser>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src));

            CreateMap<ApplicationUser, User>()
                .ConvertUsing(src => src.User);
        }
    }
}
