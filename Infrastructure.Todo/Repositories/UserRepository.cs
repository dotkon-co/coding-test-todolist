using Core.Todo.Entities;
using Core.Todo.Repositories;
using Infrastructure.Todo.Context;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Todo.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly TodoContext _context;
        private bool _disposed = false;

        public UserRepository(TodoContext context)
        {
            _context = context;
        }

        public async Task<User> Create(User entity)
        {
            if (entity.Password is not null) 
            {
                using var hmac = new HMACSHA512();
                byte[] passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(entity.Password));

                entity.PasswordHash = passwordHash;
                entity.PasswordSalt = hmac.Key;
                await _context.Users.AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            return entity;
        }

        public async Task Delete(long id)
        {
            User user = await _context.Users.FindAsync(id);
            if (user is not null) 
            { 
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<User>> GetAll() =>
            await _context.Users.ToListAsync();

        public async Task<User> GetById(long id) =>
            await _context.Users.FindAsync(id);

        public async Task<User> Update(User entity)
        {
            User user = await _context.Users.FindAsync(entity.Id);

            user.Id = entity.Id;
            user.Email = entity.Email;
            user.Name = entity.Name;
            user.IsActive = entity.IsActive;
            
            return user;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
