using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Checkin.Api.Data;
using Checkin.Api.Models;
using Checkin.Api.Common;

namespace Checkin.Api.Services
{
    public class EventsService
    {
        private readonly AppDbContext _context;

        public EventsService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Event> CreateEvent(Event evt)
        {
            _context.Events.Add(evt);
            await _context.SaveChangesAsync();
            return evt;
        }

        public async Task<Event?> GetEventById(int id)
        {
            return await _context.Events.FindAsync(id);
        }

        public async Task<PageResult<Event>> GetPagedEvents(int pageNumber, int pageSize)
        {
            var total = await _context.Events.CountAsync();
            var items = await _context.Events
                                      .OrderBy(e => e.Date)
                                      .Skip((pageNumber - 1) * pageSize)
                                      .Take(pageSize)
                                      .ToListAsync();

            return new PageResult<Event>
            {
                Items = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItems = total
            };
        }

        public async Task<bool> UpdateEvent(Event evt)
        {
            var exists = await _context.Events.AnyAsync(e => e.Id == evt.Id);
            if (!exists) return false;

            _context.Entry(evt).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteEvent(int id)
        {
            var evt = await _context.Events.FindAsync(id);
            if (evt == null) return false;

            _context.Events.Remove(evt);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetTotalCount()
        {
            return await _context.Events.CountAsync();
        }

    }
}