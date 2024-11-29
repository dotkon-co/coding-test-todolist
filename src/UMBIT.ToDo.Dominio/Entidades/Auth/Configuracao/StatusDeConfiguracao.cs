namespace UMBIT.ToDo.Dominio.Entidades.Auth.Configuracao
{
    public class StatusDeConfiguracao
    {
        public bool Configurado { get; private set; }

        public StatusDeConfiguracao(bool configurado)
        {
            Configurado = configurado;
        }
    }
}
