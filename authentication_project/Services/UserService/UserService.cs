using authentication_project.Common;
using authentication_project.Data.Contexts;
using authentication_project.Data.Entities;
using authentication_project.DTOs.Auth;
using authentication_project.DTOs.FilterDTOs;
using authentication_project.Profiles;
using LinqKit;
using System.Data.Entity;
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
        public async Task<List<UserProfilDTO>> GetAllUsersAsync(UserFilterDTO filter)
        {
            var predicate = FilterBuilder(filter);
            if(!predicate.IsStarted)
            {
                // Ortak bir Result sınıfı geliştirilip onunla birlikte verimi. Hata döndürme
                //if (result.IsFailure)
                //{
                //    Console.WriteLine(result.Error);
                //}
            }

            var query = dbContext.Users.AsQueryable();
            query.Where(predicate);

            var users = await query.ToListAsync();

            var mappedData = mapper.Map<List<UserProfilDTO>>(users);
            return mappedData;
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
