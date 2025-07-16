using authentication_project.Data.Contexts;
using authentication_project.DTOs;
using authentication_project.Handlers;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace authentication_project.Services
{
    public class TokenService : ITokenService
    {
        private readonly ProjectContext _dbContext;
        private readonly IConfiguration _configuration;
        public TokenService(ProjectContext dbContext, IConfiguration configuration)
        {

            _dbContext = dbContext;
            _configuration = configuration;
        }

        public async Task<LoginResponseDTO?> Authenticate(LoginRequestDTO request)
        {

            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
                return null;

            var userAccount = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == request.Email);
            if (userAccount is null || !PasswordHashHandler.VerifyPassword(request.Password, userAccount.PasswordHash))
                return null;

            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];
            var key = _configuration["Jwt:Key"];
            var tokenValidityMins = _configuration.GetValue<int>("Jwt:ExpireMinutes");
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
    }
}
