using MediatR;

namespace UMBIT.ToDo.BuildingBlocks.Repositorio.Application.Events.Operacoes
{
    public class CommitTransactionEvent : INotification
    {
        public Guid TransactionId { get; set; }
        public CommitTransactionEvent(Guid transactionId)
        {
            TransactionId = transactionId;
        }
    }
}
