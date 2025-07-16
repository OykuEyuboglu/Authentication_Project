using authentication_project.DTOs;

namespace authentication_project.Services
{
    public interface ILoginService
    {
        Task<LoginResponseDTO?> LoginAsync(LoginRequestDTO request);
    }
}
