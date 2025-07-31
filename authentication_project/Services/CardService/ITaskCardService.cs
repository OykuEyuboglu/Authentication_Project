using authentication_project.Data.Entities;
using authentication_project.DTOs.Card;
using authentication_project.Models.CardModels;

namespace authentication_project.Services.CardServices
{
    public interface ITaskCardService
    {
        Task<List<TaskCardDto>> GetAllAsync();
        Task<TaskCardDto> CreateAsync(CreateTaskCardDTO dto);
    }
}
