using Microsoft.AspNetCore.Mvc;
using UMBIT.Nexus.Auth.Contrato;
using UMBIT.ToDo.BuildingBlocksc.ASPNet.Basicos.Exececoes;
using UMBIT.ToDo.BuildingBlocksc.ASPNet.Controllers;
using UMBIT.ToDo.Web.services;
using static UMBIT.ToDo.BuildingBlocksc.ASPNet.Bootstrapper.ContextConfigurate;

namespace UMBIT.ToDo.Web.Controllers
{
    public class UsuariosController : ASPBaseController
    {
        private IServicoUser _serviceUser { get; set; }
        public UsuariosController(IServicoUser serviceUser, AuthSessionContext authSessionContext) : base(authSessionContext)
        {
            _serviceUser = serviceUser;
        }
        public async Task<IActionResult> Index()
        {
            var result = await _serviceUser.GetUsuarios();

            return View(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUsuario(Guid id)
        {
            return await MiddlewareDeRetorno(async () =>
            {
                await _serviceUser.RemoverUsuario(id);

                return Json(new { success = true, message = "Usuário removido com sucesso." });
            });
        }

        [HttpPost]
        public async Task<IActionResult> Aviso(Guid id)
        {
            return await MiddlewareDeRetorno(async () =>
            {
                try
                {
                    await _serviceUser.AviseUsuario(id);
                }
                catch (ExcecaoServicoExterno ex)
                {
                    return Json(new { success = false, message = ex.APIReposta?.Erros.First().Mensagem });
                }

                return Json(new { success = true, message = "Usuário avisado com sucesso." });
            });
        }

    }
}
