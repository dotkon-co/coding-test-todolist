using FluentValidation;
using UMBIT.ToDo.BuildingBlocks.Message.Messagem.Applications.Query;

namespace UMBIT.ToDo.Dominio.Application.Queries.Tokens
{
    public class ObterChavesQuery : UMBITQuery<ObterChavesQuery, ICollection<string>>
    {
        public string KId { get; private set; }

        protected ObterChavesQuery() { }

        public ObterChavesQuery(string kid)
        {
            KId = kid;
        }

        protected override void Validadors(ValidatorQuery<ObterChavesQuery> validator)
        {
            validator
                .RuleFor(cmd => cmd.KId)
                .NotEmpty().WithMessage("KId é obrigatório");
        }
    }
}
