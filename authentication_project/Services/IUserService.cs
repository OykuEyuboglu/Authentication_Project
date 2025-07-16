using authentication_project.DTOs;
using System.Security.Claims;

namespace authentication_project.Services
{
    public interface IUserService
    {
        Task<List<UserProfilDTO>> GetAllUsersAsync();

        Task<UserProfilDTO> GetUserProfilAsync(ClaimsPrincipal user);

    }
}
