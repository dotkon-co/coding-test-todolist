using Microsoft.AspNetCore.Identity;

namespace UMBIT.ToDo.Dominio.Entidades.Auth.Basicos
{
    public class Usuario : IdentityUser<Guid>
    {
        public string Nome { get; set; }
        public bool AtualizacaoSenhaRequisitada { get; protected set; }
        public bool AtualizacaoSenhaAprovada { get; protected set; }

        public virtual string ObtenhaIdentificador()
        {
            return $"{Nome} ({UserName})";
        }

        public void SolicitarAtualizacaoDeSenha()
        {
            AtualizacaoSenhaRequisitada = true;
        }

        public void AprovarAtualizacaoDeSenha()
        {
            AtualizacaoSenhaAprovada = true;
        }

        public void AtualizarRequisicoesDeAtualizacao()
        {
            AtualizacaoSenhaRequisitada = false;
            AtualizacaoSenhaAprovada = false;
        }
    }
}