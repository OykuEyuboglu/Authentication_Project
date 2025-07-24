using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using authentication_project.DTOs.Auth;
using authentication_project.Services.AuthServices;
namespace authentication_project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {

        private readonly ILoginService _loginService;
        private readonly IRegisterService _registerService;
        private readonly IUserService _userService;

        public AuthController(IRegisterService registerService, ILoginService loginService, IUserService userService)
        {
            _registerService = registerService;
            _loginService = loginService;
            _userService = userService;
        }


        [Authorize]
        [HttpGet("allusers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            if (users == null) return NotFound();

            return Ok(users);
        }

        [Authorize]
        [HttpGet("getperson")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var result = await _userService.GetUserProfilAsync(User);
            if (result == null) return NotFound();

            return Ok(result);
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { error = "Geçersiz giriş verisi" });


            var result = await _loginService.LoginAsync(request);
            if (result == null)
                return Unauthorized(new { error = "Email veya şifre hatalı" });

            return Ok(result);
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { error = "Geçersiz kayıt verisi" });

            var result = await _registerService.RegisterAsync(request);
            if (!result)
                return Conflict(new { error = "Bu e-posta zaten kayıtlı" });

            return CreatedAtAction(nameof(GetCurrentUser), new { }, new { message = "Kayıt başarılı" });
        }
    }
}