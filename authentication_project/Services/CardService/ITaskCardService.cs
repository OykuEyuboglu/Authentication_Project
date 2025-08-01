using authentication_project.Common;
using authentication_project.Data.Entities;
using authentication_project.DTOs.Card;
using authentication_project.DTOs.FilterDTOs;
using authentication_project.Models.CardModels;

namespace authentication_project.Services.CardServices
{
    public interface ITaskCardService
    {
        Task<Result<List<TaskCardFilterDTO>>> GetAllAsync(TaskCardFilterDTO? dto = null);
        Task<Result<TaskCardModel>> CreateAsync(CreateTaskCardDTO dto);
        Task<Result<TaskCardModel?>> GetByIdAsync(int id);
        Task<Result<bool>> UpdateAsync(int id, CreateTaskCardDTO dto);
        Task<Result<bool>> DeleteAsync(int id);
    }
}
