using MediatR;
using Microsoft.Extensions.DependencyInjection;
using UMBIT.ToDo.BuildingBlocks.Core.Notificacao.Interfaces;
using UMBIT.ToDo.BuildingBlocks.Fabrica.Models;
using UMBIT.ToDo.BuildingBlocks.Message.Messagem;
using UMBIT.ToDo.BuildingBlocks.Message.Messagem.Applications.Commands;
using UMBIT.ToDo.BuildingBlocks.Message.Messagem.Applications.Events;
using UMBIT.ToDo.BuildingBlocks.Message.Messagem.Applications.Query;
using UMBIT.ToDo.BuildingBlocks.Repositorio.Interfaces.Database;
using UMBIT.ToDo.BuildingBlocks.Basicos.Excecoes;

namespace UMBIT.ToDo.BuildingBlocks.Message.Bus.MediatorBus
{
    internal class MediatorBus : IMediatorBus
    {
        private readonly IMediator _mediator;
        private readonly INotificador _notificador;
        private readonly IUnidadeDeTrabalho _unidadeDeTrabalho;

        public MediatorBus(
            IMediator mediator,
            INotificador notificador,
            IUnidadeDeTrabalho unidadeDeTrabalho)
        {
            _mediator = mediator;
            _notificador = notificador;
            _unidadeDeTrabalho = unidadeDeTrabalho;
        }
        public void PublicarEvento<T>(T evento) where T : UMBITEvent
        {
            _ = Task.Run(async () =>
            {
                await FabricaGenerica.ServiceProvider.GetService<IMediator>()!.Publish(evento);
            });
        }

        public async Task<UMBITMessageResponse> EnviarComando<TCommand>(TCommand comando) where TCommand : class, IUMBITCommandRequest<TCommand>
        {
            await _unidadeDeTrabalho.InicieTransacao();
            try
            {
                if (!comando.Validation.IsValid)
                {
                    _notificador.AdicionarNotificacao(comando.Validation);
                    return new UMBITMessageResponse(comando.Validation);
                }

                UMBITMessageResponse result;

                var preCommand = comando.ObtenhaPreAction();
                var postCommand = comando.ObtenhaPostAction();

                if (preCommand != null)
                {
                    await _mediator.Publish(preCommand);
                    HandlePosAction(preCommand);
                }

                result = await _mediator.Send(comando);

                if (result.Result.IsValid && postCommand != null)
                {
                    await _mediator.Publish(postCommand);
                    HandlePosAction(postCommand);
                }

                await _unidadeDeTrabalho.FinalizeTransacao();

                return result;
            }
            catch (Exception ex)
            {
                await _unidadeDeTrabalho.RevertaTransacao();
                throw ex ?? new ExcecaoBasicaUMBIT($"Erro de Comando! {nameof(comando)}");
            }
        }

        public async Task<UMBITMessageResponse<TResp>> EnviarComando<TCommand, TResp>(TCommand comando)
            where TCommand : class, IUMBITCommandRequest<TCommand, TResp>
            where TResp : class
        {
            await _unidadeDeTrabalho.InicieTransacao();
            try
            {
                if (!comando.Validation.IsValid)
                {
                    _notificador.AdicionarNotificacao(comando.Validation);
                    return new UMBITMessageResponse<TResp>(comando.Validation);
                }

                UMBITMessageResponse<TResp> result;

                var preCommand = comando.ObtenhaPreAction();
                var postCommand = comando.ObtenhaPostAction();

                if (preCommand != null)
                {
                    await _mediator.Publish(preCommand);
                    HandlePosAction(preCommand);
                }

                result = await _mediator.Send(comando);

                if (result.Result.IsValid && postCommand != null)
                {
                    await _mediator.Publish(postCommand);
                    HandlePosAction(postCommand);
                }

                await _unidadeDeTrabalho.FinalizeTransacao();

                return result;
            }
            catch (Exception ex)
            {
                await _unidadeDeTrabalho.RevertaTransacao();
                throw ex ?? new ExcecaoBasicaUMBIT($"Erro de Comando! {nameof(comando)}");
            }
        }

        public async Task<UMBITMessageResponse<TResp>> EnviarQuery<TQuery, TResp>(TQuery comando) where TQuery : IUMBITQuery<TResp> where TResp : class
        {
            if (!comando.Validation.IsValid)
                return new UMBITMessageResponse<TResp>(comando.Validation);

            return await _mediator.Send(comando);
        }

        private void HandlePosAction(UMBITAction action)
        {
            if (_notificador.TemNotificacoes()) throw new ExcecaoBasicaUMBIT($"Erro na Action {action.GetType().Name}");
        }

    }
}
