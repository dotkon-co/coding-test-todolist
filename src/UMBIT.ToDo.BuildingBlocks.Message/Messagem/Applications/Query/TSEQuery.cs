using MediatR;
using FluentValidation.Results;
using FluentValidation;
using UMBIT.ToDo.BuildingBlocks.Message.Messagem;


namespace UMBIT.ToDo.BuildingBlocks.Message.Messagem.Applications.Query
{
    public interface IUMBITQuery<TResponse> : IRequest<UMBITMessageResponse<TResponse>> where TResponse : class
    {
        public DateTime Timestamp { get; set; }

        public ValidationResult Validation { get; }
    }
    public abstract class UMBITQuery<TQuery, TResponse> : UMBITMensagem, IUMBITQuery<TResponse> where TQuery : UMBITQuery<TQuery, TResponse> where TResponse : class
    {
        public DateTime Timestamp { get; set; }

        private ValidatorQuery<TQuery> Validator { get; set; }
        public ValidationResult Validation
        {
            get
            {
                {
                    var instancia = this as TQuery;
                    return instancia != null ? Validator.Validate(instancia) : new ValidationResult();
                }
            }
        }

        public UMBITQuery()
        {
            Timestamp = DateTime.Now;

            Validator = new ValidatorQuery<TQuery>();

            Validadors(Validator);
        }
        protected abstract void Validadors(ValidatorQuery<TQuery> validator);
    }
    public class ValidatorQuery<T> : AbstractValidator<T> where T : class
    {
        public ValidatorQuery()
        {
        }
    }
}
