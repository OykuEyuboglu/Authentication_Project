using authentication_project.Services.CardServices;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;

public class MainHub : Hub
{
    private readonly ITaskCardService _taskCardService;
    private readonly IMapper _mapper;

    public MainHub(ITaskCardService taskCardService)
    {
        _taskCardService = taskCardService;
    }

    public async Task RequestCards()
    {
        Console.WriteLine("SignalR: RequestCards çağrıldı.");

        var cards = await _taskCardService.GetAllAsync();
        await Clients.Caller.SendAsync("ReceiveTaskCards", cards);
    }
}



