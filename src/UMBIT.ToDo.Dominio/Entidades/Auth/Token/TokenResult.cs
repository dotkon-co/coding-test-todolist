namespace UMBIT.ToDo.Dominio.Entidades.Auth.Token
{
    public class TokenResult
    {
        public bool EnabledTwoFactor { get; set; }
        public bool EhAdm { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public double ExpiresIn { get; set; }
        public UsuarioToken UsuarioToken { get; set; }

    }
    public class UsuarioToken
    {
        public string Id { get; set; }
        public string Nome { get; set; }
        public IEnumerable<UsuarioClaim> Claims { get; set; }
    }
    public class UsuarioClaim
    {
        public string Value { get; set; }
        public string Type { get; set; }
    }
}
