using authentication_project.Data.Entities;
using authentication_project.DTOs.Auth;
using authentication_project.DTOs.AuthDTOs;
using authentication_project.DTOs.Card;
using authentication_project.DTOs.CardDTOs;
using authentication_project.DTOs.FilterDTOs;
using authentication_project.Enums;
using authentication_project.Extensions;
using authentication_project.Models.CardModels;
using AutoMapper;

namespace authentication_project.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserUpdateDTO, User>()
         .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
         .ForAllMembers(opt => opt.Condition(
             (src, dest, srcMember) => srcMember != null && srcMember.ToString() != ""));

            CreateMap<TaskCard, TaskCardDto>().ReverseMap();
            CreateMap<TaskCard, TaskCardFilterDTO>().ReverseMap();
            CreateMap<TaskCard, CreateTaskCardDTO>().ReverseMap();
            CreateMap<TaskCard, TaskCardModel>().ReverseMap();
            CreateMap<RegisterDTO, User>()
               .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()).ReverseMap();
            CreateMap<TaskCard, CreateTaskCardDTO>().ReverseMap();
            CreateMap<TaskCard, TaskCardModel>().ReverseMap();
            CreateMap<User, UserProfilDTO>()
                .ForMember(dest => dest.RoleDescription, opt => opt.MapFrom(src => ((UserRoleEnum)src.RoleId).GetDescription()));

            CreateMap<User, UserDto>();
            CreateMap<CreateUserDto, User>();
            CreateMap<UpdateUserDto, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());
        }
    }
}