using authentication_project.Common;
using authentication_project.Data.Contexts;
using authentication_project.Data.Entities;
using authentication_project.DTOs.Auth;
using authentication_project.Handlers;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace authentication_project.Services.AuthServices
{
    public class AuthService(IConfiguration configuration, ProjectContext dbContext, IMapper mapper) : IAuthService
    {
        public async Task<Result<LoginResponseDTO?>> Authenticate(LoginRequestDTO request)
        {
            var result = new Result<LoginResponseDTO?>();

            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                result.Success = false;
                result.Messages.Add("Email or password not exist.");
                return result;
            }

            var userAccount = await dbContext.Users.FirstOrDefaultAsync(x => x.Email == request.Email);

            if (userAccount is null || !PasswordHashHandler.VerifyPassword(request.Password, userAccount.PasswordHash))
            {
                result.Success = false;
                result.Messages.Add("Email or password wrong");
                return result;
            }

            var issuer = configuration["Jwt:Issuer"];
            var audience = configuration["Jwt:Audience"];
            var key = configuration["Jwt:Key"];
            var tokenValidityMins = configuration.GetValue<int>("Jwt:ExpireMinutes");
            var tokenExpiryTimeStamp = DateTime.UtcNow.AddMinutes(tokenValidityMins);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
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

                result.Success = true;
                result.Data = new LoginResponseDTO
                {
                    Username = userAccount.UserName,
                    AccessToken = accessToken,
                    Email = request.Email,
                    ExpiresIn = (int)tokenExpiryTimeStamp.Subtract(DateTime.UtcNow).TotalSeconds,
                    Role = userAccount.RoleId.ToString()
                };

            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Messages.Add("Token error " + ex.Message);
            }

            return result;
        }

        public async Task<Result<LoginResponseDTO?>> LoginAsync(LoginRequestDTO request)
        {
            var result = new Result<LoginResponseDTO?>();

            try
            {
                var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

                if (user == null || !PasswordHashHandler.VerifyPassword(request.Password, user.PasswordHash))
                {
                    result.Success = false;
                    result.Messages.Add("\"Error");
                    return result;
                }

                var tokenResult = await Authenticate(request);


                result.Success = true;
                result.Data = tokenResult.Data;
                result.Messages.Add("Succeed");
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Messages.Add("\"Unexpected error occured: " + ex.Message);
            }

            return result;
        }
        public async Task<Result<bool>> RegisterAsync(RegisterDTO request)
        {
            var result = new Result<bool>();

            try
            {
                if (request.RoleId == null || request.RoleId == 0)
                {
                    request.RoleId = 1;
                }

                var exists = await dbContext.Users.AnyAsync(u => u.Email == request.Email);
                if (exists)
                {
                    result.Success = false;
                    return result;
                }

                var user = mapper.Map<User>(request);

                user.PasswordHash = PasswordHashHandler.HashPassword(request.Password);

                

                dbContext.Users.Add(user);
                await dbContext.SaveChangesAsync();

                result.Success = true;
                result.Messages.Add("\"Successful");
                result.Data = exists;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Messages.Add("\"An error occurred while sign up");
            }
            return result;
        }
    }
}