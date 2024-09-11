using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TaskManagementAPI.DTOs;
using TaskManagementAPI.Services;

namespace TaskManagementAPI.Controllers
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

        // POST /users/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDTO registerDto)
        {
            await _userService.RegisterUserAsync(registerDto);
            return Ok(new { Message = "User registered successfully!" });
        }

        // POST /users/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO loginDto)
        {
            var token = await _userService.AuthenticateUserAsync(loginDto);
            return Ok(new { Token = token });
        }
    }
}
