using authentication_project.Common;
using authentication_project.DTOs.Auth;

namespace authentication_project.Services.AuthServices
{
    public interface IAuthService
    {
        Task<Result<LoginResponseDTO?>> Authenticate(LoginRequestDTO request);
        Task<Result<LoginResponseDTO?>> LoginAsync(LoginRequestDTO request);
        Task<Result<bool>> RegisterAsync(RegisterDTO request);
    }
}
