using UMBIT.ToDo.BuildingBlocks.Basicos.Excecoes;
using UMBIT.ToDo.BuildingBlocksc.WebAPI.Models;

namespace UMBIT.ToDo.BuildingBlocksc.ASPNet.Basicos.Exececoes
{
    public class ExcecaoServicoExterno : ExcecaoBasicaUMBIT
    {
        public Resposta? APIReposta { get; set; }
        public ExcecaoServicoExterno(string mensagem, Resposta? resposta = null) : base(mensagem)
        {
            APIReposta = resposta;
        }
    }
}
