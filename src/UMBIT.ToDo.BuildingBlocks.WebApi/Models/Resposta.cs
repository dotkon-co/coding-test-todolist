using System.Text.Json.Serialization;
using UMBIT.ToDo.BuildingBlocks.Basicos.Notificacoes;

namespace UMBIT.ToDo.BuildingBlocksc.WebAPI.Models
{
    public class Resposta
    {
        [JsonPropertyName("sucesso")]
        public bool Sucesso { get; set; }

        [JsonPropertyName("dados")]
        public object? Dados { get; set; }

        [JsonPropertyName("totalCount")]
        public int? TotalCount { get; set; }

        [JsonPropertyName("erros")]
        public IEnumerable<NotificacaoPadrao> Erros { get; set; }

        [JsonPropertyName("erros_sistema")]
        public IEnumerable<ErroSistema> ErrosSistema { get; set; }
    }
    public class Resposta<T>
    {
        [JsonPropertyName("sucesso")]
        public bool Sucesso { get; set; }

        [JsonPropertyName("dados")]
        public T Dados { get; set; }

        [JsonPropertyName("totalCount")]
        public int TotalCount { get; set; }

        [JsonPropertyName("erros")]
        public IEnumerable<NotificacaoPadrao> Erros { get; set; }

        [JsonPropertyName("erros_sistema")]
        public List<ErroSistema> ErrosSistema { get; set; }
    }
}
