using authentication_project.Common;
using authentication_project.DTOs.Auth;
using authentication_project.DTOs.AuthDTOs;
using authentication_project.DTOs.FilterDTOs;
using System.Security.Claims;

namespace authentication_project.Services.UserService
{
    public interface IUserService
    {
        Task<Result<List<UserProfilDTO>>> GetAllUsersAsync(UserFilterDTO filter);
        Task<Result<UserProfilDTO>> GetUserProfilAsync(ClaimsPrincipal user);
        Task<Result<UserProfilDTO>> GetProfileByIdAsync(int id);
        Task<Result<bool>> UpdateAsync(int id, UserUpdateDTO dto);
        Task<Result<bool>> DeleteAsync(int id);
    }
}
