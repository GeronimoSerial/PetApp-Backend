using AutoMapper;
using BusinessAccessLayer.Dto;
using DataAccessLayer.Entities;

namespace PresentationLayer.Infrastucture.AutoMapper
{
    public class AutoMapperProfile: Profile 
    {
        public AutoMapperProfile()
        {
            CreateMap<Pet, PetDto>();
            CreateMap<User, UserDto>();
        }
    }
}
