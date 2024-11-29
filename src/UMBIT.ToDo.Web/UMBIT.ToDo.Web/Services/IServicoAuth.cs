using Refit;
using UMBIT.Nexus.Auth.Contrato;

namespace UMBIT.ToDo.Web.services
{
    public interface IServicoAuth
    {
        [Get("/auth-status")]
        Task<AuthStatusResponseDTO> CheckAuth();

        [Post("/adicionar-administrador")]
        Task AdicionarAdministrador([Body] AdicionarAdministradorRequestDTO adicionarAdministradorRequestDTO);

        [Post("/adicionar-usuario")]
        Task AdicionarUsuario([Body] AdicionarUsuarioRequestDTO adicionarUsuarioRequestDTO);

        [Post("/login")]
        Task<TokenResponseDTO> Login([Body] LoginRequestDTO loginRequestDTO);

        [Post("/logout")]
        Task<TokenResponseDTO> Logout();

    }
}
