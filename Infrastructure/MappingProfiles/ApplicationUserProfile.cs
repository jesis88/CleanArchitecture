using AutoMapper;
using Domain.Entity;
using Infrastructure.Persistence.Repositories;

namespace Infrastructure.MappingProfiles
{
    public class ApplicationUserProfile : Profile
    {
        public ApplicationUserProfile() 
        {
            CreateMap<User, ApplicationUser>();

            CreateMap<ApplicationUser, User>();
        }
    }
}
