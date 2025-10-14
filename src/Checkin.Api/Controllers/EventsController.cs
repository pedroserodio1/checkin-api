using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Checkin.Api.Models;
using Checkin.Api.Services;

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
        public async Task<IActionResult> Create(Event evt)
        {
            var created = await _service.CreateEvent(evt);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Event evt)
        {
            if (id != evt.Id) return BadRequest();
            var updated = await _service.UpdateEvent(evt);
            return updated ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteEvent(id);
            return deleted ? NoContent() : NotFound();
        }

    }
}