using authentication_project.Common;
using authentication_project.Data.Contexts;
using authentication_project.Data.Entities;
using authentication_project.DTOs.Auth;
using authentication_project.DTOs.AuthDTOs;
using authentication_project.DTOs.FilterDTOs;
using authentication_project.Handlers;
using AutoMapper;
using LinqKit;
using System.Data.Entity;
using System.Security.Claims;

namespace authentication_project.Services.UserService
{
    public class UserService(ProjectContext dbContext, IMapper mapper) : IUserService
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

        // TODO: Jenerik hata yönetimi gelsin, API'den dönen hatalar jenerikleşirse kullanıcıya mobilden otomatik hata mesajlarının gösterimi Dio paketi ile kolaylık kazanacaktır.
        public async Task<Result<UserProfilDTO>> GetUserProfilAsync(ClaimsPrincipal user)
        {
            var result = new Result<UserProfilDTO>();

            try
            {
                var email = user.FindFirstValue(ClaimTypes.Email) ?? user.FindFirstValue("email");
                if (email == null)
                {
                    result.Success = false;
                    result.Messages.Add("\"Email not found");
                    return result;
                }

                var filter = new UserFilterDTO
                {
                    Email = email
                };
                var predicate = FilterBuilder(filter);

                var entity = await dbContext.Users.AsExpandable().FirstOrDefaultAsync(predicate);

                if (entity == null)
                {
                    result.Success = false;
                    result.Messages.Add("\"User not found");
                    return result;
                }

                var dto = mapper.Map<UserProfilDTO>(entity);

                result.Success = true;
                result.Data = dto;
                result.Messages.Add("\"Succeed");
                result.Count = 1;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Messages.Add("\"Unexpected error occured: " + ex.Message);
            }

            return result;
        }

        public async Task<Result<bool>> UpdateAsync(int id, UserUpdateDTO dto)
        {
            var result = new Result<bool>();

            try
            {
                var user = await dbContext.Users.FindAsync(id);
                if (user == null)
                {
                    result.Success = false;
                    result.Messages.Add("\"User not found");
                    result.Data = false;
                    return result;
                }

                if (!string.IsNullOrWhiteSpace(dto.PasswordHash))
                    user.PasswordHash = PasswordHashHandler.HashPassword(dto.PasswordHash);

                mapper.Map(dto, user);

                dbContext.Users.Update(user);
                await dbContext.SaveChangesAsync();

                result.Success = true;
                result.Messages.Add("\"update successfully");
                result.Data = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Messages.Add("\"An error occurred while updating");
                result.Data = false;
            }
            return result;

        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            var result = new Result<bool>();

            try
            {
                var user = await dbContext.Users.FindAsync(id);
                if (user == null)
                {
                    result.Success = false;
                    result.Messages.Add("\"User not found");
                    result.Data = false;
                    return result;
                }

                dbContext.Users.Remove(user);
                await dbContext.SaveChangesAsync();

                result.Success = true;
                result.Messages.Add("\"deleting successfully");
                result.Data = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Messages.Add("\"An error occurred while deleting");
                result.Data = false;
            }
            return result;
        }
        public async Task<Result<UserProfilDTO>> GetProfileByIdAsync(int id)
        {
            var result = new Result<UserProfilDTO>();

            try
            {
                var user = await dbContext.Users.FindAsync(id);
                if (user == null)
                {
                    result.Success = false;
                    result.Messages.Add("User not found");
                    return result;
                }

                var dto = mapper.Map<UserProfilDTO>(user);
                result.Success = true;
                result.Data = dto;
                result.Messages.Add("Success");
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Messages.Add("An unexpected error occurred: " + ex.Message);
            }

            return result;
        }

    }
}