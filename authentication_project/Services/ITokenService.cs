using authentication_project.DTOs;

namespace authentication_project.Services
{
    public interface ITokenService
    {
        Task<LoginResponseDTO?> Authenticate(LoginRequestDTO request);
    }
}
