using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using authentication_project.Models.CardModels;
using authentication_project.Services.CardServices;
using authentication_project.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using authentication_project.Data.Entities;
using authentication_project.DTOs.Card;
using authentication_project.Profiles;

namespace authentication_project.Services
{
    public class TaskCardService(IHubContext<TaskCardHub> hubContext, ProjectContext dbContext) : MappingProfile, ITaskCardService
    {
        public async Task BroadcastTaskCards(List<CreateTaskCardDTO> dtos)
        {
            await hubContext.Clients.All.SendAsync("ReceiveTaskCards", dtos);
        }

        public async Task<List<TaskCardDto>> GetAllAsync()
        {
            var entities = await dbContext.TaskCards.ToListAsync();
            return mapper.Map<List<TaskCardDto>>(entities);
        }

        public async Task<TaskCardDto> CreateAsync(CreateTaskCardDTO dto)
        {
            var entity = mapper.Map<TaskCard>(dto);
            entity.CreateDate = DateTime.UtcNow;
            entity.IsActive = true;

            dbContext.Set<TaskCard>().Add(entity);
            await dbContext.SaveChangesAsync();

            var model = mapper.Map<TaskCardDto>(entity);

            var allCards = await GetAllAsync();

            await hubContext.Clients.All.SendAsync("ReceiveTaskCards", allCards);

            return model;
        }
    }
}
