using Checkin.Api.Auth;
using Checkin.Api.Common;
using Checkin.Api.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Checkin.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        // Define your actions here
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO request)
        {
            var user = await _authService.Authenticate(request.Username, request.Password);
            if (user == null)
            {
                return Unauthorized();
            }
            // Generate JWT token
            var token = _authService.GenerateJwtToken(user);
            return Ok(new { Token = token });
        }
    }
}