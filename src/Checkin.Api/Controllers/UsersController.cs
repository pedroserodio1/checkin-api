using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Checkin.Api.Models;
using Checkin.Api.Services;
using Checkin.Api.DTO;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Checkin.Api.Controllers
{
    [ApiController]
    [Route("/api/v1/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserService _service;

        public UsersController(UserService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            if (role != "Admin")
            {
                return Forbid();
            }
            var result = await _service.GetPagedUsers(pageNumber, pageSize);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            if (role != "Admin")
            {
                return Forbid();
            }
            var user = await _service.GetUserById(id);
            return user == null ? NotFound() : Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            var created = await _service.CreateUser(user);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, User user)
        {
            if (id != user.Id) return BadRequest();
            var updated = await _service.UpdateUser(user);
            return updated ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            if (role != "Admin")
            {
                return Forbid();
            }
            var deleted = await _service.DeleteUser(id);
            return deleted ? NoContent() : NotFound();
        }

    }
}