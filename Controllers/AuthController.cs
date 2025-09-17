using LibrarySystemApi.Dtos.AuthoDtos;
using LibrarySystemApi.Models;
using LibrarySystemApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystemApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;
        const string memberRole = "Member";
        const string adminRole = "Admin";
        const string librarianRole = "Librarian";

        [HttpPost("register/member")]
        public async Task<ActionResult> RegisterMember([FromBody] RegisterUserDto request)
        {
            var user = await _authService.RegisterAsync(request, memberRole);
            if(user == null)
                return BadRequest(new {message="User already exists!"});

            return Ok(new
            {
                user.Id,
                user.Email,
                user.Role
            });
        }
        [Authorize(Roles = adminRole)]
        [HttpPost("register/admin")]
        public async Task<ActionResult> RegisterAdmin([FromBody] RegisterUserDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var user = await _authService.RegisterAsync(request, adminRole);
            if (user == null)
                return BadRequest(new { message = "User already exists!" });

            return Ok(new
            {
                user.Id,
                user.Email,
                user.Role
            });
        }
        [HttpPost("register/librarian")]
        public async Task<ActionResult> Register([FromBody] RegisterUserDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var user = await _authService.RegisterAsync(request, librarianRole);
            if (user == null)
                return BadRequest(new { message = "User already exists!" });

            return Ok(new
            {
                user.Id,
                user.Email,
                user.Role
            });
        }

        [HttpPost("login")]
        public async Task<ActionResult<TokenResponseDto>> Login([FromBody] LoginDto request)
        {
            var tokenResponse = await _authService.LoginAsync(request);
            if (tokenResponse == null)
                return BadRequest(new {message = "Invalid email or password" });
            return Ok(tokenResponse);
        }

        [Authorize]
        [HttpGet("Authenticated")]
        public IActionResult AuthEndpoint()
        {
            return Ok("You have been authenticated");
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("Admin-only")]
        public IActionResult AdminOnlyEndpoint()
        {
            return Ok("You have the role of admin");
        }

    }
}
