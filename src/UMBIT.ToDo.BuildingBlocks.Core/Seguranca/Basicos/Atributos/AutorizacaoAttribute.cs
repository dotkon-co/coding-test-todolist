using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection;
using UMBIT.ToDo.BuildingBlocks.Basicos.Utilitarios;
using UMBIT.ToDo.BuildingBlocks.Core.Seguranca.Interfaces;
using UMBIT.ToDo.BuildingBlocks.Core.Seguranca.Models;
using UMBIT.ToDo.BuildingBlocks.Basicos.Excecoes;

namespace UMBIT.ToDo.BuildingBlocks.Core.Seguranca.Basicos.Atributos
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AutorizacaoAttribute : Attribute, IAuthorizationFilter
    {

        private bool ApenasADM { get; set; } = false;
        private List<IPermissao> Permissoes { get; set; } = new();

        public AutorizacaoAttribute()
        {
        }
        public AutorizacaoAttribute(bool apenasAdm = false)
        {
            ApenasADM = apenasAdm;
        }
        public AutorizacaoAttribute(Type TPermissao, Type TEnum, params int[] indentificadoresDePermissao)
        {
            try
            {
                Permissoes = new List<IPermissao>();

                var permissoes = (from m in TPermissao.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public)
                                  where m.FieldType.IsAssignableTo(TPermissao)
                                  select m into f
                                  select f.GetValue(null)).Cast<IPermissao>();

                foreach (var identificadorDePermissao in indentificadoresDePermissao)
                {
                    var permissao = permissoes.Single(t => t.Identificador == identificadorDePermissao);
                    Permissoes.Add(permissao);
                }
            }
            catch (Exception ex)
            {
                throw new ExcecaoBasicaUMBIT("Erro de inicialização do atributo de autorização, Verifique se não existem permissões duplicadas, falta de Permissão declarada no Enumerador de Permissão e afins!", ex);
            }
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            try
            {
                var contextoPrincipal = new ContextoPrincipal(context.HttpContext);
                var principal = contextoPrincipal.ObtenhaPrincipal();

                if (ApenasADM && principal != null && principal.EhAdministrador()) return;

                bool isAuthorized = principal != null && PossuiPermissoesRequeridas(principal);

                if (!isAuthorized && EnvironmentHelper.IsProduction())
                    context.Result = new ForbidResult();
            }
            catch (Exception ex)
            {
                throw new ExcecaoBasicaUMBIT("Erro na autorização", ex);
            }
        }

        private bool PossuiPermissoesRequeridas(Principal principal)
        {
            foreach (var permissao in Permissoes)
                if (principal.PossuiPermissaoDeAcesso(permissao))
                    return true;

            return false;
        }
    }
}
