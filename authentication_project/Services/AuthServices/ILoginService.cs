using authentication_project.DTOs.Auth;

namespace authentication_project.Services.AuthServices
{
    public interface ILoginService
    {
        Task<LoginResponseDTO?> LoginAsync(LoginRequestDTO request);
    }
}
