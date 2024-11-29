using System.Reflection;
using UMBIT.ToDo.BuildingBlocks.Basicos.Excecoes;

namespace UMBIT.ToDo.BuildingBlocks.Basicos.Utilitarios
{
    public static class ProjetoAssemblyHelper
    {
        private static bool EhInicializada { get; set; }
        public static Assembly ProjetoDominio { get; private set; } = null;
        public static Assembly ProjetoContrato { get; private set; } = null;
        public static Assembly ProjetoInterface { get; private set; } = null;
        public static Assembly ProjetoInfraestrutura { get; private set; } = null;
        public static string NameProjetoDominio { get; private set; } = null;
        public static string NameProjetoContrato { get; private set; } = null;
        public static string NameProjetoInterface { get; private set; } = null;
        public static string NameProjetoInfraestrutura { get; private set; } = null;

        public static void Inicialize(
            string projetoDominio,
            string projetoContrato,
            string projetoInterface,
            string projetoInfraestrutura)
        {
            if (EhInicializada) { throw new ExcecaoBasicaUMBIT("Helper de asssembly de projeto já iniciado!"); }

            ProjetoDominio = Assembly.Load(projetoDominio);
            ProjetoContrato = Assembly.Load(projetoContrato);
            ProjetoInterface = Assembly.Load(projetoInterface);
            ProjetoInfraestrutura = Assembly.Load(projetoInfraestrutura);

            NameProjetoDominio = projetoDominio;
            NameProjetoContrato = projetoContrato;
            NameProjetoInterface = projetoInterface;
            NameProjetoInfraestrutura = projetoInfraestrutura;

        }

        public static Assembly[] ObtenhaAppAssemblys()
        {
            return new List<Assembly>()
            {
                ProjetoDominio,
                ProjetoContrato,
                ProjetoInterface,
                ProjetoInfraestrutura,
            }.ToArray();
        }
    }
}
