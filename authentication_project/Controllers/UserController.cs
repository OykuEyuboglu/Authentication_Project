using Microsoft.AspNetCore.Mvc;
using authentication_project.Services.UserService;

namespace authentication_project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _userService.GetAllUsersAsync(null);

            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _userService.GetProfileByIdAsync(id);

            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result.Data);
        }
    }
}
