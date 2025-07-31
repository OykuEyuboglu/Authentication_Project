using authentication_project.Data.Contexts;
using authentication_project.Data.Entities;
using authentication_project.DTOs.Auth;
using authentication_project.Handlers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace authentication_project.Services.AuthServices
{
    public class AuthService(IConfiguration configuration, ProjectContext dbContext) : IAuthService
    {
        public async Task<LoginResponseDTO?> Authenticate(LoginRequestDTO request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
                return null;

            var userAccount = await dbContext.Users.FirstOrDefaultAsync(x => x.Email == request.Email);
            if (userAccount is null || !PasswordHashHandler.VerifyPassword(request.Password, userAccount.PasswordHash))
                return null;

            var issuer = configuration["Jwt:Issuer"];
            var audience = configuration["Jwt:Audience"];
            var key = configuration["Jwt:Key"];
            var tokenValidityMins = configuration.GetValue<int>("Jwt:ExpireMinutes");
            var tokenExpiryTimeStamp = DateTime.UtcNow.AddMinutes(tokenValidityMins);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    //new Claim(JwtRegisteredClaimNames.Email, request.Email)
                    new Claim(ClaimTypes.Email, request.Email)

                }),
                Expires = tokenExpiryTimeStamp,
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha512Signature),
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                var accessToken = tokenHandler.WriteToken(securityToken);

                return new LoginResponseDTO
                {
                    AccessToken = accessToken,
                    Email = request.Email,
                    ExpiresIn = (int)tokenExpiryTimeStamp.Subtract(DateTime.UtcNow).TotalSeconds
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine("TOKEN OLUŞTURMA HATASI: " + ex.Message);
                return null;
            }
        }
        public async Task<LoginResponseDTO?> LoginAsync(LoginRequestDTO request)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null || !PasswordHashHandler.VerifyPassword(request.Password, user.PasswordHash))
                return null;

            return await Authenticate(request);
        }
        public async Task<bool> RegisterAsync(RegisterDTO request)
        {
            var exists = await dbContext.Users.AnyAsync(u => u.Email == request.Email);
            if (exists) return false;

            var hashPassword = PasswordHashHandler.HashPassword(request.Password);

            dbContext.Users.Add(new User
            {
                Email = request.Email,
                PasswordHash = hashPassword,
                UserName = request.UserName
            });

            await dbContext.SaveChangesAsync();
            return true;
        }
    }
}
