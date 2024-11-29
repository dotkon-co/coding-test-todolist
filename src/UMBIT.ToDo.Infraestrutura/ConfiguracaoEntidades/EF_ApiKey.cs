using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UMBIT.ToDo.BuildingBlocks.Repositorio.EF;
using UMBIT.ToDo.Dominio.Entidades.Auth.Token;

namespace UMBIT.ToDo.Infraestrutura.ConfiguracaoEntidades
{
    public class EF_ApiKey : CoreEntityConfigurate<ApiToken>
    {
        public override void ConfigureEntidade(EntityTypeBuilder<ApiToken> builder)
        {
        }
    }
}
