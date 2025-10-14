using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Checkin.Api.Data;
using Checkin.Api.Models;

namespace Checkin.Api.Controllers
{
    [ApiController]
    [Route("/api/v1/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EventsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> getAll()
        {
            var eventList = await _context.Events.ToListAsync();
            return Ok(eventList);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var evt = await _context.Events.FindAsync(id);
            if (evt == null) return NotFound();
            return Ok(evt);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Event evt)
        {
            _context.Events.Add(evt);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = evt.Id }, evt);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Event evt)
        {
            if (id != evt.Id) return BadRequest();

            _context.Entry(evt).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var evt = await _context.Events.FindAsync(id);
            if (evt == null) return NotFound();

            _context.Events.Remove(evt);
            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
}