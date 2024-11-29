using Microsoft.EntityFrameworkCore;
using UMBIT.ToDo.BuildingBlocks.Repositorio.Interfaces.Database;

namespace UMBIT.ToDo.BuildingBlocks.Repositorio.EF
{
    public class RepositorioDeLeitura<T> : IRepositorioDeLeitura<T> where T : class
    {
        protected DbContext Contexto { get; private set; }
        protected DbSet<T> Db { get; private set; }


        public RepositorioDeLeitura(DbContext contexto)
        {
            Contexto = contexto;
            Db = Contexto.Set<T>();
        }

        public T? Carregar(T objeto)
        {
            var entity = Db.Attach(objeto) as T;

            return entity;
        }

        public IQueryable<T> Query()
        {
            return Db
                .AsNoTracking()
                .AsQueryable();
        }

    }
}
