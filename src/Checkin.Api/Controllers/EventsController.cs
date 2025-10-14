using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Checkin.Api.Models;
using Checkin.Api.Services;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Checkin.Api.Enums;

namespace Checkin.Api.Controllers
{
    [ApiController]
    [Route("/api/v1/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly EventsService _service;

        public EventsController(EventsService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _service.GetPagedEvents(pageNumber, pageSize);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var evt = await _service.GetEventById(id);
            return evt == null ? NotFound() : Ok(evt);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(Event evt)
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            Console.WriteLine("User role: " + role);
            if (role != UserRole.Organizer.ToString() && role != UserRole.Admin.ToString())
            {
                return Forbid();
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Forbid();
            }
            evt.OrganizerId = int.Parse(userId);

            var created = await _service.CreateEvent(evt);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, Event evt)
        {
            if (id != evt.Id) return BadRequest();
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if(userId != evt.OrganizerId.ToString())
            {
                return Forbid();
            }

            var updated = await _service.UpdateEvent(evt);
            return updated ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            if (role != UserRole.Organizer.ToString() && role != UserRole.Admin.ToString())
            {
                return Forbid();
            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var evt = await _service.GetEventById(id);
            if (evt == null) return NotFound();
            if(userId != evt.OrganizerId.ToString())
            {
                return Forbid();
            }
            var deleted = await _service.DeleteEvent(id);
            return deleted ? NoContent() : NotFound();
        }

    }
}