using authentication_project.DTOs.Auth;
using authentication_project.DTOs.AuthDTOs;
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
            if (!result.Success)
                return Conflict(new { error = result.Messages });

            return CreatedAtAction(nameof(GetCurrentUser), new { }, new { message = "Kayıt başarılı" });
        }

        //[Authorize]
        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers([FromQuery] UserFilterDTO filter)
        {
            filter ??= new UserFilterDTO();

            var result = await userService.GetAllUsersAsync(filter);

            if (!result.Success || result.Data == null || !result.Data.Any())
                return BadRequest(result);

            var users = result.Data;
            return Ok(users);
        }

        [Authorize]
        [HttpGet("GetUser")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var result = await userService.GetUserProfilAsync(User);
            if (result == null) return BadRequest(result);

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserUpdateDTO dto)
        {
            
            var result = await userService.UpdateAsync(id, dto);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await userService.DeleteAsync(id);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
