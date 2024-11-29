using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.Json;
using UMBIT.ToDo.BuildingBlocks.Basicos.Excecoes;
using UMBIT.ToDo.BuildingBlocks.Basicos.Notificacoes;
using UMBIT.ToDo.BuildingBlocksc.ASPNet.Basicos.Exececoes;
using static System.Collections.Specialized.BitVector32;
using static UMBIT.ToDo.BuildingBlocksc.ASPNet.Bootstrapper.ContextConfigurate;

namespace UMBIT.ToDo.BuildingBlocksc.ASPNet.Controllers
{
    public abstract class ASPBaseController : Controller
    {
        protected AuthSessionContext _authSessionContext { get; set; }
        protected ASPBaseController(AuthSessionContext authSessionContext)
        {
            _authSessionContext = authSessionContext;
        }

        protected ActionResult MiddlewareDeRetorno(Func<ActionResult> retorno, string action = null, string controller = null)
        {
            try
            {
                var res = retorno();

                return res;
            }
            catch (ExcecaoServicoExterno ex)
            {
                TempData["Notifications"] = ex.APIReposta?.Erros;

            }
            catch (ExcecaoBasicaUMBIT ex)
            {
                TempData["Notifications"] = new List<NotificacaoPadrao>
                {
                    new NotificacaoPadrao { Titulo ="Erro Generico!", Mensagem = ex.Message },
                };
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }

            return TrateResultado(action, controller);
        }
        protected async Task<ActionResult> MiddlewareDeRetorno(Func<Task<ActionResult>> retorno, string action = null, string controller = null)
        {
            try
            {
                var res = await retorno();

                return res;
            }
            catch (ExcecaoServicoExterno ex)
            {
                TempData["Notifications"] = JsonSerializer.Serialize(ex.APIReposta?.Erros.ToList());

            }
            catch (ExcecaoBasicaUMBIT ex)
            {
                TempData["Notifications"] = new List<NotificacaoPadrao>
                {
                    new NotificacaoPadrao { Titulo ="Erro Generico!", Mensagem = ex.Message },
                };

            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }

            return TrateResultado(action, controller);
        }

        private ActionResult TrateResultado(string action = null, string controller = null)
        {
            return RedirectToAction(action ?? RouteData.Values["action"].ToString(), controller ?? RouteData.Values["controller"].ToString());

        }
    }

}
