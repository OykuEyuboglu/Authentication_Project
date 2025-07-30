using authentication_project.DTOs.Card;
using authentication_project.Services.CardServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class TaskCardController : ControllerBase
{
    private readonly ITaskCardService _taskCardService;
    private readonly IHubContext<TaskCardHub> _hubContext;

    public TaskCardController(ITaskCardService taskCardService, IHubContext<TaskCardHub> hubContext)
    {
        _taskCardService = taskCardService;
        _hubContext = hubContext;
    }

    [HttpGet("getTaskCards")]
    public async Task<IActionResult> GetAll()
    {
        var cards = await _taskCardService.GetAllAsync();
        return Ok(cards);
    }

    [HttpPost("addTaskCards")]
    public async Task<IActionResult> Create([FromBody] CreateTaskCardDTO createDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var createdCard = await _taskCardService.CreateAsync(createDto);
        var allCards = await _taskCardService.GetAllAsync();
        await _hubContext.Clients.All.SendAsync("ReceiveTaskCards", allCards);

        return CreatedAtAction(nameof(GetAll), new { id = createdCard.id }, createdCard);
    }
}
