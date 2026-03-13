using authentication_project.Common;
using authentication_project.Data.Contexts;
using authentication_project.Data.Entities;
using authentication_project.DTOs.Card;
using authentication_project.DTOs.FilterDTOs;
using authentication_project.Models.CardModels;
using authentication_project.Services.CardServices;
using AutoMapper;
using LinqKit;
using Microsoft.AspNetCore.SignalR;
using System.Data.Entity;
using System.Net.WebSockets;

namespace authentication_project.Services
{
    public class TaskCardService(IHubContext<MainHub> hubContext, ProjectContext dbContext, IMapper mapper) : ITaskCardService
    {
        #region Helper Methods
        public static ExpressionStarter<TaskCard> FilterBuilder(TaskCardFilterDTO request)
        {
            var filter = PredicateBuilder.New<TaskCard>(f => true);

            if (!string.IsNullOrWhiteSpace(request.barcode))
            {
                filter = filter.And(f => f.Barcode.Contains(request.barcode));
            }
            if (!string.IsNullOrWhiteSpace(request.createUser))
            {
                filter = filter.And(f => f.CreateUser.Contains(request.createUser));
            }
            if (!string.IsNullOrWhiteSpace(request.updateUser))
            {
                filter = filter.And(f => f.UpdateUser.Contains(request.updateUser));
            }
            if (!string.IsNullOrWhiteSpace(request.tailNo))
            {
                filter = filter.And(f => f.TailNo.Contains(request.tailNo));
            }
            if (request.typeId.HasValue)
            {
                filter = filter.And(f => f.TypeId == request.typeId.Value);
            }
            if (request.statusId.HasValue)
            {
                filter = filter.And(f => f.StatusId == request.statusId.Value);
            }
            if (request.isCritical.HasValue)
            {
                filter = filter.And(f => f.IsCritical == request.isCritical.Value);
            }
            if (!string.IsNullOrWhiteSpace(request.description))
            {
                filter = filter.And(f => f.Description.Contains(request.description));
            }

            return filter;
        }

        #endregion
        public async Task BroadcastTaskCards(List<CreateTaskCardDTO> dtos)
        {
            await hubContext.Clients.All.SendAsync("ReceiveTaskCards", dtos);
        }

        public async Task<Result<List<TaskCardFilterDTO>>> GetAllAsync(TaskCardFilterDTO? dto = null)
        {
            var result = new Result<List<TaskCardFilterDTO>>();

            try
            {
                if (dto == null)
                    dto = new TaskCardFilterDTO();

                var predicate = FilterBuilder(dto);
            var query = dbContext.TaskCards.AsExpandable().Where(predicate);
            var taskcards = await query.ToListAsync();
            var dtos = mapper.Map<List<TaskCardFilterDTO>>(taskcards);
                
                
                result.TimeStamp = DateTime.Now;
                result.Data = dtos;
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Messages.Add("Error: " + ex.Message);
            }

            return result;
        }

        public async Task<Result<TaskCardModel>> CreateAsync(CreateTaskCardDTO dto)
        {
            var result = new Result<TaskCardModel?>();
            try
            {
                var entity = mapper.Map<TaskCard>(dto);
                entity.CreateDate = DateTime.UtcNow;
                entity.IsActive = true;

                dbContext.Set<TaskCard>().Add(entity);
                await dbContext.SaveChangesAsync();

                var model = mapper.Map<TaskCardModel>(entity);

                var allCards = await GetAllAsync();

                await hubContext.Clients.All.SendAsync("ReceiveTaskCards", allCards);

                result.Success = true;
                result.Data = model;
                result.Messages.Add("Succeed");
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Messages.Add("Error: " + ex.Message);
            }

            return result;
        }

        public async Task<Result<TaskCardModel?>> GetByIdAsync(int id)
        {
            var result = new Result<TaskCardModel?>();

            try
            {
                var card = await dbContext.TaskCards.FindAsync(id);

                if (card == null)
                {
                    result.Success = false;
                    result.Messages.Add("\"Task Card not found");
                    return result;
                }
                var dto = mapper.Map<TaskCardModel?>(card);

                result.Success = true;
                result.Messages.Add("\"update successfully");
                result.Data = dto;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Messages.Add("\"Error");
            }
            return result;
        }

        public async Task<Result<bool>> UpdateAsync(int id, CreateTaskCardDTO dto)
        {
            var result = new Result<bool>();

            try
            {
                var card = await dbContext.TaskCards.FindAsync(id);
                if (card == null)
                {
                    result.Success = false;
                    result.Messages.Add("\"Task Card not found");
                    result.Data = false;
                    return result;
                }
                mapper.Map(dto, card);
                card.UpdateDate = DateTime.Now;

                dbContext.TaskCards.Update(card);
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

                var card = await dbContext.TaskCards.FindAsync(id);
                if (card == null)
                {
                    result.Success = false;
                    result.Messages.Add("\"Task Card not found");
                    result.Data = false;
                    return result;
                }
                dbContext.TaskCards.Remove(card);
                await dbContext.SaveChangesAsync(); result.Success = true;
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
    }
}
