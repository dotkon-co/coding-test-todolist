using System.Text.Json.Serialization;

namespace UMBIT.ToDo.BuildingBlocks.Basicos.Notificacoes
{
    public class NotificacaoPadrao
    {
        [JsonPropertyName("titulo")]
        public string Titulo { get; set; } = string.Empty;

        [JsonPropertyName("mensagem")]
        public string Mensagem { get; set; } = string.Empty;
        public NotificacaoPadrao()
        {

        }
        public NotificacaoPadrao(string mensagem)
        {
            Mensagem = mensagem;
        }

        public NotificacaoPadrao(string titulo, string mensagem)
        {
            Titulo = titulo;
            Mensagem = mensagem;
        }
    }
}