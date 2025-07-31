using authentication_project.DTOs.Auth;
using authentication_project.DTOs.FilterDTOs;
using System.Security.Claims;

namespace authentication_project.Services.UserService
{
    public interface IUserService
    {
        Task<List<UserProfilDTO>> GetAllUsersAsync(UserFilterDTO filter);
        Task<UserProfilDTO> GetUserProfilAsync(ClaimsPrincipal user);
    }
}
