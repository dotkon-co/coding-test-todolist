using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using UMBIT.ToDo.BuildingBlocks.Basicos.Excecoes;

namespace UMBIT.ToDo.BuildingBlocks.Basicos.Notificacoes
{
    public class ErroSistema : NotificacaoPadrao
    {

        [JsonPropertyName("excecao")]
        public string Excecao { get; set; }

        [JsonPropertyName("excecaoInterna")]
        public string ExcecaoInterna { get; set; }

        [JsonPropertyName("rastreamentoPilha")]
        public string RastreamentoPilha { get; set; }

        [JsonPropertyName("metodoCodigoFonte")]
        public string MetodoCodigoFonte { get; set; }

        [JsonPropertyName("linhaCodigoFonte")]
        public int LinhaCodigoFonte { get; set; }

        [JsonPropertyName("nomeArquivoFonte")]
        public string NomeArquivoFonte { get; set; }

        [JsonPropertyName("objetoManipulado")]
        public object ObjetoManipulado { get; set; }

        public ErroSistema()
        {

        }

        public ErroSistema(
            string mensagemErro,
            [CallerMemberName] string metodoCodigoFonte = "",
            [CallerLineNumber] int linhaCodigoFonte = 0,
            [CallerFilePath] string nomeArquivoFonte = "") : base(mensagemErro)
        {
            Mensagem = mensagemErro;
            MetodoCodigoFonte = metodoCodigoFonte;
            LinhaCodigoFonte = linhaCodigoFonte;
            NomeArquivoFonte = nomeArquivoFonte;
        }

        public ErroSistema(
           string mensagemErro,
           object objetoManipulado,
           [CallerMemberName] string metodoCodigoFonte = "",
           [CallerLineNumber] int linhaCodigoFonte = 0,
           [CallerFilePath] string nomeArquivoFonte = "") : base(mensagemErro)
        {
            Mensagem = mensagemErro;
            ObjetoManipulado = objetoManipulado;
            MetodoCodigoFonte = metodoCodigoFonte;
            LinhaCodigoFonte = linhaCodigoFonte;
            NomeArquivoFonte = nomeArquivoFonte;
        }

        public ErroSistema(
            string titulo,
            string mensagemErro,
            object objetoManipulado,
            [CallerMemberName] string metodoCodigoFonte = "",
            [CallerLineNumber] int linhaCodigoFonte = 0,
            [CallerFilePath] string nomeArquivoFonte = "") : base(mensagemErro)
        {
            Titulo = titulo;
            Mensagem = mensagemErro;
            ObjetoManipulado = objetoManipulado;
            MetodoCodigoFonte = metodoCodigoFonte;
            LinhaCodigoFonte = linhaCodigoFonte;
            NomeArquivoFonte = nomeArquivoFonte;
        }

        public ErroSistema(
            string mensagemErro,
            Exception excecao) : base(mensagemErro)
        {
            Mensagem = mensagemErro;
            Excecao = excecao?.Message;
            RastreamentoPilha = excecao?.StackTrace;
            ExcecaoInterna = excecao?.InnerException?.Message;

            var st = new StackTrace(excecao, true);
            var frame = st.GetFrame(0);

            LinhaCodigoFonte = frame.GetFileLineNumber();
            NomeArquivoFonte = frame.GetFileName();
            MetodoCodigoFonte = excecao is ExcecaoBasicaUMBIT UMBITEx ? UMBITEx.MetodoCodigoFonte : frame.GetMethod().Name;
        }

        public ErroSistema(
            string mensagemErro,
            object objetoManipulado,
            Exception excecao) : base(mensagemErro)
        {
            Mensagem = mensagemErro;
            ObjetoManipulado = objetoManipulado;
            Excecao = excecao?.Message;
            ExcecaoInterna = excecao?.InnerException?.Message;
            RastreamentoPilha = excecao?.StackTrace;

            var st = new StackTrace(excecao, true);
            var frame = st.GetFrame(0);

            LinhaCodigoFonte = frame.GetFileLineNumber();
            NomeArquivoFonte = frame.GetFileName();
            MetodoCodigoFonte = excecao is ExcecaoBasicaUMBIT UMBITEx ? UMBITEx.MetodoCodigoFonte : frame.GetMethod().Name;
        }

        public ErroSistema(
            string titulo,
            string mensagemErro,
            Exception excecao) : base(mensagemErro)
        {
            Titulo = titulo;
            Mensagem = mensagemErro;
            Excecao = excecao?.Message;
            ExcecaoInterna = excecao?.InnerException?.Message;
            RastreamentoPilha = excecao?.StackTrace;

            var st = new StackTrace(excecao, true);
            var frame = st.GetFrame(0);

            LinhaCodigoFonte = frame.GetFileLineNumber();
            NomeArquivoFonte = frame.GetFileName();
            MetodoCodigoFonte = excecao is ExcecaoBasicaUMBIT UMBITEx ? UMBITEx.MetodoCodigoFonte : frame.GetMethod().Name;
        }

        public ErroSistema(
            string titulo,
            string mensagemErro,
            object objetoManipulado,
            Exception excecao) : base(mensagemErro)
        {
            Titulo = titulo;
            Mensagem = mensagemErro;
            ObjetoManipulado = objetoManipulado;
            Excecao = excecao?.Message;
            ExcecaoInterna = excecao?.InnerException?.Message;
            RastreamentoPilha = excecao?.StackTrace;


            var st = new StackTrace(excecao, true);
            var frame = st.GetFrame(0);

            LinhaCodigoFonte = frame.GetFileLineNumber();
            NomeArquivoFonte = frame.GetFileName();
            MetodoCodigoFonte = excecao is ExcecaoBasicaUMBIT UMBITEx ? UMBITEx.MetodoCodigoFonte : frame.GetMethod().Name;
        }


        public override string ToString()
            => $"{Mensagem} [Metodo: {MetodoCodigoFonte}; Linha: {LinhaCodigoFonte}; File: {NomeArquivoFonte}]";


    }
}
