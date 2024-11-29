using FluentValidation.Results;


namespace UMBIT.ToDo.BuildingBlocks.Message.Messagem
{
    [Serializable]
    public class UMBITMessageResponse
    {
        public ValidationResult Result { get; set; }

        public UMBITMessageResponse()
        {
            Result = new ValidationResult();
        }

        public UMBITMessageResponse(ValidationResult result)
        {
            Result = result;
        }

        internal void AdicionarErro(string mensagem, string propriedade = "")
        {
            Result.Errors.Add(new ValidationFailure(propriedade, mensagem));
        }
    }


    [Serializable]
    public class UMBITMessageResponse<T> : UMBITMessageResponse where T : class
    {
        public T? Dados { get; set; }

        public UMBITMessageResponse()
        {
            Result = new ValidationResult();
        }
        public UMBITMessageResponse(ValidationResult validationResult)
        {
            Result = validationResult;
        }
        public void SetDados(T? result)
        {
            Dados = result;
        }
    }
}
