using Checkin.Api.Utils;
using Checkin.Api.Data;
using Checkin.Api.Models;
using Checkin.Api.Common;
using Microsoft.EntityFrameworkCore;

namespace Checkin.Api.Services
{
    public class UserService
    {
        private readonly AppDbContext _context;
        private readonly PasswordHasher _hasher;

        public UserService(AppDbContext context, PasswordHasher hasher)
        {
            _context = context;
            _hasher = hasher;
        }

        public async Task<User> CreateUser(User user)
        {
            if (await _context.Users.AnyAsync(u => u.Username == user.Username || u.Email == user.Email))
            {
                throw new Exception("Username or Email already exists.");
            }

            var (hash, salt) = _hasher.HashPassword(user.Password);
            user.PasswordHash = hash;
            user.Salt = salt;
            user.RegisteredAt = DateTime.UtcNow;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User?> GetUserById(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<PageResult<User>> GetPagedUsers(int pageNumber, int pageSize)
        {
            var total = await _context.Users.CountAsync();
            var items = await _context.Users
                                      .OrderBy(e => e.RegisteredAt)
                                      .Skip((pageNumber - 1) * pageSize)
                                      .Take(pageSize)
                                      .ToListAsync();

            return new PageResult<User>
            {
                Items = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItems = total
            };
        }

        public async Task<bool> UpdateUser(User user)
        {
            var exists = await _context.Users.AnyAsync(u => u.Id == user.Id);
            if (!exists) return false;

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
        
        public async Task<int> GetTotalCount()
        {
            return await _context.Users.CountAsync();
        }
        

    }
}