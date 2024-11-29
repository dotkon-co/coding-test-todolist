using UMBIT.ToDo.BuildingBlocks.Repositorio.Data;

namespace UMBIT.ToDo.Dominio.Entidades.Auth.Token
{
    public class ApiToken : BaseEntity<ApiToken>
    {
        public Guid IdUsuario { get; set; }
        public string Kid { get; set; }
        public string Audience { get; set; }
        public string ApiSecret { get; set; }

        protected override void Validadors(Validator<ApiToken> validator)
        {
        }
    }
}
