using authentication_project.Data.Entities;
using authentication_project.DTOs.Card;
using authentication_project.Models.CardModels;
using AutoMapper;

namespace authentication_project.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<TaskCard, CreateTaskCardDTO>().ReverseMap();
            CreateMap<TaskCard, TaskCardModel>().ReverseMap();
        }
    }
}
