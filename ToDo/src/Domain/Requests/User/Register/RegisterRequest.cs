namespace Domain.Requests.User.Register
{
    public class RegisterRequest
    {
        public string Name { get; set; } = null!;
        public string User { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
