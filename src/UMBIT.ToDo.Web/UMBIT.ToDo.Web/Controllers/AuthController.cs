using Microsoft.AspNetCore.Mvc;
using UMBIT.Nexus.Auth.Contrato;
using UMBIT.ToDo.BuildingBlocksc.ASPNet.Controllers;
using UMBIT.ToDo.Web.services;
using static UMBIT.ToDo.BuildingBlocksc.ASPNet.Bootstrapper.ContextConfigurate;

namespace UMBIT.ToDo.Web.Controllers
{
    public class AuthController : ASPBaseController
    {
        private IServicoAuth _serviceAuth { get; set; }
        public AuthController(IServicoAuth serviceAuth, AuthSessionContext authSessionContext) : base(authSessionContext)
        {
            _serviceAuth = serviceAuth;
        }
        public async Task<IActionResult> Login()
        {
            return await MiddlewareDeRetorno(async() =>
            {
                if (!(await _serviceAuth.CheckAuth()).Configured)
                {
                    return RedirectToAction(nameof(CreateAdministrator));
                }

                return View();
            });
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDTO loginRequestDTO)
        {
            return await MiddlewareDeRetorno(async () =>
             {
                 loginRequestDTO.Audience = "web";
                 var tokenResponse = await _serviceAuth.Login(loginRequestDTO);

                 _authSessionContext.SetAuthContext(tokenResponse);

                 return RedirectToAction("Index", "Home");
             });
        }


        public async Task<IActionResult> Logout()
        {
            return await MiddlewareDeRetorno(async () =>
            {
                var tokenResponse = await _serviceAuth.Logout();

                _authSessionContext.RemoveAuthContext();

                return RedirectToAction(nameof(Login));

            });
        }

        public IActionResult CreateAdministrator()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAdministratorAsync(AdicionarAdministradorRequestDTO adicionarAdministradorRequestDTO)
        {
            return await MiddlewareDeRetorno(async () =>
            {
                await _serviceAuth.AdicionarAdministrador(adicionarAdministradorRequestDTO);
                return RedirectToAction(nameof(Login));
            }, nameof(CreateAdministrator));

        }
        public IActionResult CreateUsuario()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateUsuario(AdicionarUsuarioRequestDTO adicionarUsuario)
        {
            return await MiddlewareDeRetorno(async () =>
            {
                await _serviceAuth.AdicionarUsuario(adicionarUsuario);
                return RedirectToAction(nameof(Login));

            }, nameof(CreateUsuario));

        }
    }
}
