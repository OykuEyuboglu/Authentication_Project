using authentication_project.DTOs.Auth;

namespace authentication_project.Services.AuthServices
{
    public interface IRegisterService
    {
        Task<bool> RegisterAsync(RegisterDTO request);

    }
}
