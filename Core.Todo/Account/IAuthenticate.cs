using Core.Todo.Entities;

namespace Core.Todo.Account
{
    public interface IAuthenticate
    {
        Task<bool> AuthenticateAsync(string email, string password);
        Task<bool> UserExists(string email);
        public string GenerateToken(long id, string email);
        public Task<User> GetUserByEmail(string email);
    }
}
