using FluentValidation;
using FluentValidation.Results;
using MediatR;
using UMBIT.ToDo.BuildingBlocks.Message.Messagem;

namespace UMBIT.ToDo.BuildingBlocks.Message.Messagem.Applications.Commands
{
    public interface IUMBITCommandRequest<TCommand> : IRequest<UMBITMessageResponse> where TCommand : class, IUMBITCommandRequest<TCommand>
    {
        public DateTime Timestamp { get; set; }
        public ValidationResult Validation { get; }
        public PreActionBase<TCommand> ObtenhaPreAction();
        public PostActionBase<TCommand> ObtenhaPostAction();
    }
    public interface IUMBITCommandRequest<TCommand, TResponse> : IRequest<UMBITMessageResponse<TResponse>> where TResponse : class where TCommand : class, IUMBITCommandRequest<TCommand, TResponse>
    {
        public DateTime Timestamp { get; set; }
        public ValidationResult Validation { get; }
        public PreActionBase<TCommand, TResponse> ObtenhaPreAction();
        public PostActionBase<TCommand, TResponse> ObtenhaPostAction();
    }
    public abstract class UMBITCommand<TCommand> : UMBITMensagem, IUMBITCommandRequest<TCommand> where TCommand : class, IUMBITCommandRequest<TCommand>
    {
        public ValidationResult Validation
        {
            get
            {
                {
                    var instancia = this as TCommand;
                    return instancia != null ? Validator.Validate(instancia) : new ValidationResult();
                }
            }
        }
        public DateTime Timestamp { get; set; }
        private ValidatorCommand<TCommand> Validator { get; set; }
        public UMBITCommand()
        {
            Timestamp = DateTime.Now;

            Validator = new ValidatorCommand<TCommand>();

            Validadors(Validator);
        }

        protected abstract void Validadors(ValidatorCommand<TCommand> validator);

        public PreActionBase<TCommand> ObtenhaPreAction() => new PreAction((this as TCommand)!);
        public PostActionBase<TCommand> ObtenhaPostAction() => new PostAction((this as TCommand)!);


        public class PreAction : PreActionBase<TCommand>
        {
            public PreAction(TCommand command) : base(command)
            {
            }
        }

        public class PostAction : PostActionBase<TCommand>
        {
            public PostAction(TCommand command) : base(command)
            {
            }
        }


    }
    public abstract class UMBITCommand<TCommand, TResponse> : UMBITMensagem, IUMBITCommandRequest<TCommand, TResponse> where TCommand : class, IUMBITCommandRequest<TCommand, TResponse> where TResponse : class
    {
        public ValidationResult Validation
        {
            get
            {
                {
                    var instancia = this as TCommand;
                    return instancia != null ? Validator.Validate(instancia) : new ValidationResult();
                }
            }
        }
        public DateTime Timestamp { get; set; }
        private ValidatorCommand<TCommand> Validator { get; set; }

        public UMBITCommand()
        {
            Timestamp = DateTime.Now;

            Validator = new ValidatorCommand<TCommand>();

            Validadors(Validator);
        }
        protected abstract void Validadors(ValidatorCommand<TCommand> validator);

        public PreActionBase<TCommand, TResponse> ObtenhaPreAction() => new PreAction((this as TCommand)!);
        public PostActionBase<TCommand, TResponse> ObtenhaPostAction() => new PostAction((this as TCommand)!);

        public class PreAction : PreActionBase<TCommand, TResponse>
        {
            public PreAction(TCommand command) : base(command)
            {
            }
        }

        public class PostAction : PostActionBase<TCommand, TResponse>
        {
            public PostAction(TCommand command) : base(command)
            {
            }
        }
    }

    public abstract class PreActionBase<TCommand> : UMBITAction where TCommand : class, IUMBITCommandRequest<TCommand>
    {
        public TCommand Command { get; }
        public PreActionBase(TCommand command)
        {
            Command = command;
        }

    }
    public abstract class PostActionBase<TCommand> : UMBITAction where TCommand : class, IUMBITCommandRequest<TCommand>
    {
        public TCommand Command { get; }
        public PostActionBase(TCommand command)
        {
            Command = command;
        }
    }

    public abstract class PreActionBase<TCommand, TResponse> : UMBITAction where TCommand : class, IUMBITCommandRequest<TCommand, TResponse> where TResponse : class
    {
        public TCommand Command { get; }
        public PreActionBase(TCommand command)
        {
            Command = command;
        }

    }
    public abstract class PostActionBase<TCommand, TResponse> : UMBITAction where TCommand : class, IUMBITCommandRequest<TCommand, TResponse> where TResponse : class
    {
        public TCommand Command { get; }
        public PostActionBase(TCommand command)
        {
            Command = command;
        }
    }
    public class ValidatorCommand<T> : AbstractValidator<T> where T : class
    {
        public ValidatorCommand()
        {
        }
    }
}
