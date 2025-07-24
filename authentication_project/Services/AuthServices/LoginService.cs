using authentication_project.Data.Contexts;
using authentication_project.DTOs.Auth;
using authentication_project.Handlers;
using Microsoft.EntityFrameworkCore;

namespace authentication_project.Services.AuthServices
{
    public class LoginService : ILoginService
    {

        private readonly ProjectContext _dbContext;
        private readonly ITokenService _tokenService;

        public LoginService(ProjectContext dbContext, ITokenService tokenService)
        {
            _dbContext = dbContext;
            _tokenService = tokenService;
        }

        public async Task<LoginResponseDTO?> LoginAsync(LoginRequestDTO request)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null || !PasswordHashHandler.VerifyPassword(request.Password, user.PasswordHash))
                return null;

            return await _tokenService.Authenticate(request);
        }
    }
}
