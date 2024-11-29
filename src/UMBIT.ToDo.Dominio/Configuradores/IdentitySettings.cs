namespace UMBIT.ToDo.Dominio.Configuradores
{
    public class IdentitySettings
    {
        public int RequiredLength { get; set; } = 6;
        public int RequiredUniqueChars { get; set; } = 1;
        public int DefaultLockoutTimeSpan { get; set; } = 5;
        public int MaxFailedAccessAttempts { get; set; } = 5;

        public bool RequireDigit { get; set; } = true;
        public bool RequireLowercase { get; set; } = true;
        public bool RequireUppercase { get; set; } = true;
        public bool TwoFactorEnabled { get; set; } = false;
        public bool AllowedForNewUsers { get; set; } = true;
        public bool RequireUniqueEmail { get; set; } = true;
        public bool RequireConfirmedEmail { get; set; } = false;
        public bool RequireNonAlphanumeric { get; set; } = true;
        public string AllowedUserNameCharacters { get; set; } = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@";
    }
}
