using authentication_project.DTOs.Auth;
using authentication_project.DTOs.FilterDTOs;
using authentication_project.Services.AuthServices;
using authentication_project.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace authentication_project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IAuthService authService, IUserService userService) : ControllerBase
    {
        //[Authorize]
        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers([FromQuery] UserFilterDTO filter)
        {
            var users = await userService.GetAllUsersAsync(filter);
            if (users == null || !users.Any()) return NotFound();

            return Ok(users);
        }

        [Authorize]
        [HttpGet("GetUser")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var result = await userService.GetUserProfilAsync(User);
            if (result == null) return NotFound();

            return Ok(result);
        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { error = "Geçersiz giriş verisi" });


            var result = await authService.LoginAsync(request);
            if (result == null)
                return Unauthorized(new { error = "Email veya şifre hatalı" });

            return Ok(result);
        }


        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { error = "Geçersiz kayıt verisi" });

            var result = await authService.RegisterAsync(request);
            if (!result)
                return Conflict(new { error = "Bu e-posta zaten kayıtlı" });

            return CreatedAtAction(nameof(GetCurrentUser), new { }, new { message = "Kayıt başarılı" });
        }
    }
}
