using AutoMapper;
using UMBIT.Nexus.Auth.Contrato;
using UMBIT.ToDo.Dominio.Application.Commands.Autenticacao;
using UMBIT.ToDo.Dominio.Entidades.Auth.Basicos;
using UMBIT.ToDo.Dominio.Entidades.Auth.Configuracao;
using UMBIT.ToDo.Dominio.Entidades.Auth.Token;

namespace UMBIT.ToDo.API.Mapeadores
{
    public class AutenticacaoMapper : Profile
    {
        public AutenticacaoMapper()
        {
            //REQUESTS
            CreateMap<AdicionarAdministradorRequestDTO, AdicionarAdministradorCommand>()
                .ForMember(dst => dst.Nome, opt => opt.MapFrom(src => src.Nome))
                .ForMember(dst => dst.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dst => dst.Senha, opt => opt.MapFrom(src => src.Password))
                .ForMember(dst => dst.ConfirmarSenha, opt => opt.MapFrom(src => src.ConfirmPassword));

            CreateMap<AdicionarUsuarioRequestDTO, CadastrarUsuarioCommand>()
                .ForMember(dst => dst.Nome, opt => opt.MapFrom(src => src.Nome))
                .ForMember(dst => dst.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dst => dst.Senha, opt => opt.MapFrom(src => src.Password))
                .ForMember(dst => dst.ConfirmarSenha, opt => opt.MapFrom(src => src.ConfirmPassword));

            CreateMap<LoginRequestDTO, AutenticarUsuarioCommand>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User))
                .ForMember(dest => dest.Senha, opt => opt.MapFrom(src => src.Password))
                .ForMember(dest => dest.Audience, opt => opt.MapFrom(src => src.Audience));

            CreateMap<RefreshTokenRequestDTO, AtualizarSessaoDeUsuarioCommand>()
                .ForMember(dest => dest.AccessToken, opt => opt.MapFrom(src => src.Token))
                .ForMember(dest => dest.RefreshToken, opt => opt.MapFrom(src => src.RefreshToken));

            CreateMap<ResetarSenhaRequestDTO, AlterarSenhaCommand>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Senha, opt => opt.MapFrom(src => src.Password))
                .ForMember(dest => dest.ConfirmarSenha, opt => opt.MapFrom(src => src.ConfirmPassword));

            CreateMap<SolicitarAlteracaoSenhaRequestDTO, SolicitarAlteracaoSenhaCommand>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));

            //RESPONSES
            CreateMap<StatusDeConfiguracao, AuthStatusResponseDTO>()
                .ForMember(dest => dest.Configured, opt => opt.MapFrom(src => src.Configurado));

            CreateMap<Usuario, UserStatusResponseDTO>()
                .ForMember(dest => dest.Ativo, opt => opt.MapFrom(src => src.EmailConfirmed))
                .ForMember(dest => dest.RefreshPasswordRequested, opt => opt.MapFrom(src => src.AtualizacaoSenhaRequisitada))
                .ForMember(dest => dest.RefreshPasswordApproved, opt => opt.MapFrom(src => src.AtualizacaoSenhaAprovada));

            CreateMap<TokenResult, TokenResponseDTO>();
            CreateMap<UsuarioToken, UsuarioTokenResponseDTO>();
            CreateMap<UsuarioClaim, ClaimTokenResponseDTO>();
        }
    }
}
