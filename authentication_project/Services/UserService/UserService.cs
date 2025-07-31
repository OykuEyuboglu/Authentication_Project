using authentication_project.Common;
using authentication_project.Data.Contexts;
using authentication_project.Data.Entities;
using authentication_project.DTOs.Auth;
using authentication_project.DTOs.FilterDTOs;
using authentication_project.Profiles;
using LinqKit;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Data.Entity;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;

namespace authentication_project.Services.UserService
{
    public class UserService(ProjectContext dbContext) : MappingProfile, IUserService
    {
        #region Helper Methods
        public static ExpressionStarter<User> FilterBuilder(UserFilterDTO request)
        {
            var filter = PredicateBuilder.New<User>(f => true);

            if (!string.IsNullOrWhiteSpace(request.Email))
                filter = filter.And(f => f.Email.Contains(request.Email));
            if (!string.IsNullOrWhiteSpace(request.Username))
                filter = filter.And(f => f.UserName.Contains(request.Username));
            if (request.RoleId.HasValue)
                filter = filter.And(u => u.RoleId == request.RoleId.Value);

            return filter;
        }
        #endregion
        public async Task<Result<List<UserProfilDTO>>> GetAllUsersAsync(UserFilterDTO filter)
        {
            var result = new Result<List<UserProfilDTO>>();
            try
            {
                filter = new UserFilterDTO();

                var predicate = FilterBuilder(filter);

                var query = dbContext.Users.AsExpandable().Where(predicate);
                var users = query.ToList();

                var mappedData = mapper.Map<List<UserProfilDTO>>(users);

                if (!predicate.IsStarted)
                {
                    result.TimeStamp = DateTime.Now;
                    result.Messages.Add("\"Filtre belirtilmedi");
                    return result;
                }

                result.TimeStamp = DateTime.Now;
                result.Data = mappedData;
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Messages.Add($"server error:\n{ex.Message}");
                result.TimeStamp = DateTime.Now;
            }

            return result;
        }

        public async Task<UserProfilDTO> GetUserProfilAsync(ClaimsPrincipal user)
        {
            var email = user.FindFirstValue(ClaimTypes.Email) ?? user.FindFirstValue("email");
            if (email == null) return null;
            // TODO: Jenerik hata yönetimi gelsin, API'den dönen hatalar jenerikleşirse kullanıcıya mobilden otomatik hata mesajlarının gösterimi Dio paketi ile kolaylık kazanacaktır.

            var entity = await dbContext.Users.FirstOrDefaultAsync(x => x.Email == email);
            if (entity == null) throw new ArgumentException("No records found!");

            return new UserProfilDTO
            {
                Email = entity.Email,
                Username = entity.UserName,
            };
        }
    }
}
