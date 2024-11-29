using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using UMBIT.ToDo.BuildingBlocks.Basicos.Utilitarios;
using UMBIT.ToDo.BuildingBlocks.Fabrica.Models;
using UMBIT.ToDo.BuildingBlocks.Repositorio.Application.Events.Operacoes.Base;
using UMBIT.ToDo.BuildingBlocks.Repositorio.Data;
using UMBIT.ToDo.BuildingBlocks.Repositorio.Interfaces;

namespace UMBIT.ToDo.BuildingBlocks.Repositorio.EF
{

    public abstract class BaseContext<T> : DbContext where T : DbContext
    {
        public DbSet<TrackEvent> SetTrackEvent { get; set; }
        public BaseContext(DbContextOptions<T> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TrackEvent>().HasKey(e => e.Id);

            try
            {
                if (ProjetoAssemblyHelper.ProjetoInfraestrutura != null)
                {
                    var configuratos = ProjetoAssemblyHelper.ProjetoInfraestrutura.GetTypes()
                                                                                  .Where(t =>
                                                                                        t != null &&
                                                                                        t.Namespace != null &&
                                                                                        t.BaseType != null &&
                                                                                        t.IsClass &&
                                                                                        t.BaseType.IsGenericType &&
                                                                                        (t.BaseType?.GetGenericTypeDefinition() == typeof(CoreEntityConfigurate<>) ||
                                                                                        t.BaseType?.GetGenericTypeDefinition() == typeof(CoreEntityConfigurateCustom<>)));

                    foreach (var type in configuratos)
                    {
                        Console.Write($"Configurando o modelo de {type.Name}\n");

                        dynamic? instanciaDeConfiguracao = Activator.CreateInstance(type);
                        if (instanciaDeConfiguracao != null)
                            modelBuilder.ApplyConfiguration(instanciaDeConfiguracao);
                    }

                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }



            base.OnModelCreating(modelBuilder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                int result = 0;

                var domainEvents = new List<DomainEvent>();

                ChangeTracker.DetectChanges();
                var entries = ChangeTracker
                    .Entries<IBaseEntity>()
                    .Where(entry =>
                       entry.State == EntityState.Added ||
                       entry.State == EntityState.Modified ||
                       entry.State == EntityState.Deleted);

                if (entries.Any() && Database.CurrentTransaction == null)
                    return await ManipuleContextoNaoTransacional(entries, cancellationToken);
                else if (entries.Any())
                    return await ManipuleContextoTransacional(entries, cancellationToken);
                else
                {
                    result = await base.SaveChangesAsync(cancellationToken);

                    ChangeTracker.Clear();

                    return result;
                }

            }
            catch (DbUpdateException ex)
            {
                ChangeTracker.Clear();
                throw new Exception("Falha ao executar dbupdate", ex);

            }
            catch (Exception ex)
            {
                ChangeTracker.Clear();
                throw new Exception("Falha genérica ao tentar salvar alterações", ex);
            }
        }

        private async Task<int> ManipuleContextoTransacional(IEnumerable<EntityEntry<IBaseEntity>> entries, CancellationToken cancellationToken)
        {
            int result = 0;
            var entitysDataEvent = new List<(IBaseEntity dataOrigin, IBaseEntity? dataEdited, EntityState entityState, string assembly, string type)>();

            foreach (var entry in entries)
            {
                var now = DateTime.Now;
                entry.Property(entidade => entidade.DataAtualizacao).CurrentValue = now;

                if (entry.State == EntityState.Added)
                {
                    entry.Property(entidade => entidade.DataCriacao).CurrentValue = now;

                    entitysDataEvent.Add((entry.Entity, null, EntityState.Added, entry.Entity.GetType()!.Assembly!.FullName!, entry.Entity.GetType()!.FullName!));

                }
                else if (entry.State == EntityState.Modified)
                {
                    var dbValues = (await entry.GetDatabaseValuesAsync())!.ToObject();

                    entitysDataEvent.Add((entry.Entity, null, EntityState.Modified, entry.Entity.GetType()!.Assembly!.FullName!, entry.Entity.GetType()!.FullName!));
                }
                else
                {
                    entitysDataEvent.Add((entry.Entity, null, EntityState.Deleted, entry.Entity.GetType()!.Assembly!.FullName!, entry.Entity.GetType()!.FullName!));
                }
            }

            Console.Write($"Numero de Eventos => {entitysDataEvent.Count} / {Database.CurrentTransaction!.TransactionId}");

            if (entitysDataEvent.Any())
            {
                var options = new JsonSerializerOptions()
                {
                    WriteIndented = true,
                    ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles,
                };

                await SetTrackEvent.AddRangeAsync(entitysDataEvent.Select(t => new TrackEvent
                (Database.CurrentTransaction!.TransactionId,
                JsonSerializer.Serialize(t.dataOrigin as object, options),
                JsonSerializer.Serialize(t.dataEdited as object, options),
                (int)t.entityState,
                t.assembly,
                t.type
                )));

                result = await base.SaveChangesAsync(cancellationToken);

                ChangeTracker.Clear();
            }
            else
            {
                result = await base.SaveChangesAsync(cancellationToken);

                ChangeTracker.Clear();
            }

            return result;

        }
        private async Task<int> ManipuleContextoNaoTransacional(IEnumerable<EntityEntry<IBaseEntity>> entries, CancellationToken cancellationToken)
        {
            int result = 0;
            var domainEvents = new List<DomainEvent>();

            foreach (var entry in entries)
            {
                var now = DateTime.Now;
                entry.Property(entidade => entidade.DataAtualizacao).CurrentValue = now;

                if (entry.State == EntityState.Added)
                {
                    entry.Property(entidade => entidade.DataCriacao).CurrentValue = now;

                    if (ValidaDomainEvent(entry.Entity.ObtenhaEventoAdicao(entry.Entity), out DomainEvent @event))
                        domainEvents.Add(@event);

                }
                else if (entry.State == EntityState.Modified)
                {
                    var dbValues = (await entry.GetDatabaseValuesAsync())!.ToObject();

                    if (ValidaDomainEvent(entry.Entity.ObtenhaEventoEdicao((dbValues as IBaseEntity)!, entry.Entity), out DomainEvent @event))
                        domainEvents.Add(@event);
                }
                else
                {
                    if (ValidaDomainEvent(entry.Entity.ObtenhaEventoRemocao(entry.Entity), out DomainEvent @event))
                        domainEvents.Add(@event);
                }
            }
            Console.Write($"Numero de Eventos => {domainEvents.Count} / {Database.CurrentTransaction!.TransactionId}");

            if (domainEvents.Any())
            {
                result = await base.SaveChangesAsync(cancellationToken);

                ChangeTracker.Clear();

                var mediator = FabricaGenerica.ServiceProvider.GetService<IMediator>();
                foreach (var evento in domainEvents)
                    _ = Task.Run(() =>
                    {
                        try
                        {
                            mediator!.Publish(evento!, cancellationToken);
                        }
                        catch (Exception ex)
                        {
                            Console.Write(ex);
                        }
                    });
            }
            else
            {
                result = await base.SaveChangesAsync(cancellationToken);

                ChangeTracker.Clear();
            }

            return result;
        }
        private bool ValidaDomainEvent(DomainEvent domainEvent, out DomainEvent @event)
        {
            if (domainEvent == null)
            {
                @event = null; return false;
            }
            @event = domainEvent;
            return true;
        }
    }
}
