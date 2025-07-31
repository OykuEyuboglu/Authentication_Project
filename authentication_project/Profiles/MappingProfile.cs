using authentication_project.Data.Entities;
using authentication_project.DTOs.Auth;
using authentication_project.DTOs.Card;
using authentication_project.Enums;
using authentication_project.Extensions;
using authentication_project.Models.CardModels;
using AutoMapper;

namespace authentication_project.Profiles
{
    public abstract class MappingProfile : Profile
    {
        protected readonly IMapper mapper;
        protected MappingProfile()
        {
            var config = new MapperConfiguration(map =>
            {
                map.CreateMap<TaskCard, CreateTaskCardDTO>().ReverseMap();
                map.CreateMap<TaskCard, TaskCardDto>().ReverseMap();
                map.CreateMap<User, UserProfilDTO>()
                    .ForMember(dest => dest.RoleDescription, opt => opt.MapFrom(src => ((UserRoleEnum)src.RoleId).GetDescription()));
            });
            mapper = config.CreateMapper();
        }
    }
}
