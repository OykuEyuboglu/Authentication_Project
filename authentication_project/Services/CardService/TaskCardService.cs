using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using authentication_project.Models.CardModels;
using authentication_project.Services.CardServices;
using authentication_project.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using authentication_project.Data.Entities;
using authentication_project.DTOs.Card;

namespace authentication_project.Services
{
    public class TaskCardService : ITaskCardService
    {
        private readonly IMapper _mapper;
        private readonly IHubContext<TaskCardHub> _hubContext;
        private readonly ProjectContext _dbContext;

        public TaskCardService(IMapper mapper, IHubContext<TaskCardHub> hubContext, ProjectContext dbContext)
        {
            _mapper = mapper;
            _hubContext = hubContext;
            _dbContext = dbContext;
        }

        public async Task BroadcastTaskCards(List<CreateTaskCardDTO> dtos)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveTaskCards", dtos);
        }

        public async Task<List<TaskCardModel>> GetAllAsync()
        {
            var entities = await _dbContext.TaskCards.ToListAsync();

            return _mapper.Map<List<TaskCardModel>>(entities);
        }

        public async Task<TaskCardModel> CreateAsync(CreateTaskCardDTO dto)
        {
            var entity = _mapper.Map<TaskCard>(dto);
            entity.CreateDate = DateTime.UtcNow;
            entity.IsActive = true;

            _dbContext.Set<TaskCard>().Add(entity);
            await _dbContext.SaveChangesAsync();

            var model = _mapper.Map<TaskCardModel>(entity);

            var allCards = await GetAllAsync();

            await _hubContext.Clients.All.SendAsync("ReceiveTaskCards", allCards);

            return model;
        }
    }
}
