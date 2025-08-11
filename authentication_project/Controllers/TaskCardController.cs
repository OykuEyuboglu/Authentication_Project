using authentication_project.Common;
using authentication_project.DTOs.Card;
using authentication_project.Services.CardServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

[ApiController]
[Route("api/[controller]")]
public class TaskCardController(ITaskCardService taskCardService, IHubContext<MainHub> hubContext) : ControllerBase
{
    [HttpGet("getTaskCards")]
    public async Task<IActionResult> GetAll()
    {
        var cards = await taskCardService.GetAllAsync();
        return Ok(cards);
    }

    [HttpPost("addTaskCards")]
    public async Task<IActionResult> Create([FromBody] CreateTaskCardDTO createDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var createdCard = await taskCardService.CreateAsync(createDto);
        var allCards = await taskCardService.GetAllAsync();

        await hubContext.Clients.All.SendAsync("ReceiveTaskCards", allCards);

        return CreatedAtAction(nameof(GetAll), new { id = createdCard.Data?.id }, createdCard);
    }

    [HttpGet("getTaskCards/{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await taskCardService.GetByIdAsync(id);

        if (result == null)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] CreateTaskCardDTO dto)
    {
        var result = await taskCardService.UpdateAsync(id, dto);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await taskCardService.DeleteAsync(id);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }
}
