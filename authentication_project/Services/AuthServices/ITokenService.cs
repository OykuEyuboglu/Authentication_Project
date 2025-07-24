using authentication_project.DTOs.Auth;

namespace authentication_project.Services.AuthServices
{
    public interface ITokenService
    {
        Task<LoginResponseDTO?> Authenticate(LoginRequestDTO request);
    }
}
