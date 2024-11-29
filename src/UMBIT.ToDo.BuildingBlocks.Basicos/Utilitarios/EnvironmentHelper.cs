using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMBIT.ToDo.BuildingBlocks.Basicos.Utilitarios
{
    public static class EnvironmentHelper
    {
        public static bool IsProduction()
        {
            ;
            return string.Equals(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"), "Production", StringComparison.OrdinalIgnoreCase);
        }
    }
}
