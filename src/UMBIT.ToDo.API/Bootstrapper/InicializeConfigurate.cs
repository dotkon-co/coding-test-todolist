using UMBIT.ToDo.BuildingBlocks.Basicos.Utilitarios;

namespace UMBIT.ToDo.API.Bootstrapper
{
    public static class InicializeConfigurate
    {
        public static void Inicialize()
        {
            ProjetoAssemblyHelper.Inicialize(
                "UMBIT.ToDo.Dominio",
                "UMBIT.ToDo.Contrato",
                "UMBIT.ToDo.API",
                "UMBIT.ToDo.Infraestrutura");
        }
    }
}
