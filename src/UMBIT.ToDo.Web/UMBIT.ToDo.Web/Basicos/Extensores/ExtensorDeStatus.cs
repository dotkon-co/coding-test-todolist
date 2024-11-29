using UMBIT.ToDo.Web.Basicos.Enumerador;

namespace UMBIT.ToDo.Web.Basicos.Extensores
{
    public static class ExtensorDeStatus
    {
        public static string GetStatus(this EnumeradorStatus enumeradorStatus)
        {
            return enumeradorStatus.ToString(); 
        } 
    }
}
