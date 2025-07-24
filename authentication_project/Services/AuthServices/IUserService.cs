using authentication_project.DTOs.Auth;
using System.Security.Claims;

namespace authentication_project.Services.AuthServices
{
    public interface IUserService
    {
        Task<List<UserProfilDTO>> GetAllUsersAsync();

        Task<UserProfilDTO> GetUserProfilAsync(ClaimsPrincipal user);

    }
}
