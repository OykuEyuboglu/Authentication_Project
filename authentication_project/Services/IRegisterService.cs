using authentication_project.DTOs;

namespace authentication_project.Services
{
    public interface IRegisterService
    {
        Task<bool> RegisterAsync(RegisterDTO request);

    }
}
