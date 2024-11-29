namespace UMBIT.ToDo.BuildingBlocks.Core.Seguranca.Models
{
    public class AuthenticationSettings
    {
        public string Issuer { get; set; }
        public string BaseAddress { get; set; }
        public double ExpiresMins { get; set; }
        public string[] Audiences { get; set; }
        public string ChallengeScheme { get; set; }
        public string AuthenticateScheme { get; set; }
    }
}
