using authentication_project.DTOs.Auth;

namespace authentication_project.Services.AuthServices
{
    public interface IAuthService
    {
        Task<LoginResponseDTO?> Authenticate(LoginRequestDTO request);
        Task<LoginResponseDTO?> LoginAsync(LoginRequestDTO request);
        Task<bool> RegisterAsync(RegisterDTO request);
    }
}
