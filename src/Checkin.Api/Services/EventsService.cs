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
                                      .Include(e => e.Organizer) // inclui dados do organizador
                                      .Select(e => new Event // projeta apenas os campos necessários
                                      {
                                          Id = e.Id,
                                          Name = e.Name,
                                          Location = e.Location,
                                          Date = e.Date,
                                          Time = e.Time,
                                          Description = e.Description,
                                          Capacity = e.Capacity,
                                          CreatedAt = e.CreatedAt,
                                          UpdatedAt = e.UpdatedAt,
                                          OrganizerId = e.OrganizerId,
                                          Organizer = new User
                                          {
                                              Id = e.Organizer!.Id,
                                              Username = e.Organizer.Username,
                                              FullName = e.Organizer.FullName,
                                              Email = e.Organizer.Email
                                          }
                                      })
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

        public async Task<List<Event>> getEventByUserId(int userId)
        {
            return await _context.Events
                                 .Where(e => e.OrganizerId == userId)
                                 .Include(e => e.Organizer)
                                 .Select(e => new Event // projeta apenas os campos necessários
                                 {
                                     Id = e.Id,
                                     Name = e.Name,
                                     Location = e.Location,
                                     Date = e.Date,
                                     Time = e.Time,
                                     Description = e.Description,
                                     Capacity = e.Capacity,
                                     CreatedAt = e.CreatedAt,
                                     UpdatedAt = e.UpdatedAt,
                                     OrganizerId = e.OrganizerId,
                                     Organizer = new User
                                     {
                                         Id = e.Organizer!.Id,
                                         Username = e.Organizer.Username,
                                         FullName = e.Organizer.FullName,
                                         Email = e.Organizer.Email
                                     }
                                 })
                                 .ToListAsync();
        } 

        public async Task<int> GetTotalCount()
        {
            return await _context.Events.CountAsync();
        }

    }
}