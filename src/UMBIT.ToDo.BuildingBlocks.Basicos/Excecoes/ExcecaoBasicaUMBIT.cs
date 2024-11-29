using System.Runtime.CompilerServices;

namespace UMBIT.ToDo.BuildingBlocks.Basicos.Excecoes
{
    public class ExcecaoBasicaUMBIT : Exception
    {
        public string Mensagem { get; private set; }
        public string MetodoCodigoFonte { get; private set; }
        public Exception ExcecaoInterna { get; private set; }
        public ExcecaoBasicaUMBIT(string mensagem) : base(mensagem)
        {
            Mensagem = mensagem;
        }
        public ExcecaoBasicaUMBIT(
            string mensagem,
            Exception ex,
            [CallerMemberName] string metodoCodigoFonte = "") : base(mensagem, ex)
        {
            ExcecaoInterna = ex;
            MetodoCodigoFonte = metodoCodigoFonte;
            Mensagem = ex.GetType() == typeof(ExcecaoBasicaUMBIT) ? ((ExcecaoBasicaUMBIT)ex).Mensagem : mensagem;
        }

        public override string ToString()
        {
            return Mensagem;
        }
    }
}
