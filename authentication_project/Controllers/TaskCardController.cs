using authentication_project.DTOs.Card;
using authentication_project.Services.CardServices;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class TaskCardController : ControllerBase
{
    private readonly ITaskCardService _taskCardService;

    public TaskCardController(ITaskCardService taskCardService)
    {
        _taskCardService = taskCardService;
    }

    [HttpGet("getTaskCards")]
    public async Task<IActionResult> GetAll()
    {
        var cards = await _taskCardService.GetAllAsync();
        return Ok(cards);
    }

    [HttpPost("addTaskCard")]
    public async Task<IActionResult> Create([FromBody] CreateTaskCardDTO createDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var createdCard = await _taskCardService.CreateAsync(createDto);
        return CreatedAtAction(nameof(GetAll), new { id = createdCard.Id }, createdCard);
    }
}
