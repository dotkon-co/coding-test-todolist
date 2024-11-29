using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UMBIT.ToDo.BuildingBlocks.Core.Notificacao.Interfaces;
using UMBIT.ToDo.BuildingBlocks.Repositorio.Interfaces;
using UMBIT.ToDo.BuildingBlocks.Repositorio.Interfaces.Database;

namespace UMBIT.ToDo.BuildingBlocks.Repositorio.EF
{
    public class Repositorio<T> : RepositorioDeLeitura<T>, IRepositorio<T> where T : class
    {
        private readonly INotificador Notificador;

        public Repositorio(DbContext contexto, INotificador notificador) : base(contexto)
        {
            Notificador = notificador;
        }

        public virtual async Task<IEnumerable<T>> Filtrar(Expression<Func<T, bool>> predicado)
        {
            return await Db
                .AsNoTracking()
                .Where(predicado)
                .ToListAsync();
        }

        public virtual async Task Adicionar(T objeto)
        {
            if (objeto is IBaseEntity baseEntity && !baseEntity.Validate.IsValid)
            {
                Notificador.AdicionarNotificacao(baseEntity.Validate);
                return;
            }

            await Db.AddAsync(objeto);
        }

        public virtual async Task AdicionarTodos(List<T> objetos)
        {
            foreach (T objeto in objetos)
            {
                if (objeto is IBaseEntity baseEntity && !baseEntity.Validate.IsValid)
                {
                    Notificador.AdicionarNotificacao(baseEntity.Validate);
                    return;
                }
            }

            await Db.AddRangeAsync(objetos);
        }

        public virtual void Atualizar(T objeto)
        {
            if (objeto is IBaseEntity baseEntity && !baseEntity.Validate.IsValid)
            {
                Notificador.AdicionarNotificacao(baseEntity.Validate);
                return;
            }

            var result = Db.Update(objeto);
            Contexto.Entry(objeto).State = result.State;
        }

        public virtual void Remover(T objeto)
        {
            var result = Db.Remove(objeto);
            Contexto.Entry(objeto).State = result.State;
        }
        public virtual void RemoverTodos(List<T> objetos)
        {
            Db.RemoveRange(objetos);
        }

        protected void MiddlewareDeRepositorio(Action method)
        {
            try
            {
                method();
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro no processamento do banco de dados. Contate o administrador.", ex);
            }
        }

        protected TRes MiddlewareDeRepositorio<TRes>(Func<TRes> method)
        {
            try
            {
                return method();
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro no processamento do banco de dados. Contate o administrador.", ex);
            }
        }
    }
}
