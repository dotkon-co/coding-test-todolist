using AutoMapper;
using UMBIT.Nexus.Auth.Contrato;
using UMBIT.ToDo.Dominio.Application.Commands.Usuarios;
using UMBIT.ToDo.Dominio.Entidades.Auth.Basicos;

namespace UMBIT.ToDo.API.Mapeadores
{
    public class UsuarioMapper : Profile
    {
        public UsuarioMapper()
        {
            CreateMap<AtualizarUsuarioRequestDTO, AtualizarUsuarioCommand>()
                .ForMember(dst => dst.IdUsuario, opt => opt.MapFrom(src => src.IdUsuario))
                .ForMember(dst => dst.NomeUsuario, opt => opt.MapFrom(src => src.Nome));

            CreateMap<AtualizarDadosDeUsuarioRequestDTO, AtualizarDadosDeUsuarioCommand>()
                .ForMember(dst => dst.Nome, opt => opt.MapFrom(src => src.Nome))
                .ForMember(dst => dst.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dst => dst.Senha, opt => opt.MapFrom(src => src.Password))
                .ForMember(dst => dst.ConfirmarSenha, opt => opt.MapFrom(src => src.ConfirmPassword));

            CreateMap<Usuario, UsuarioResponseDTO>()
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.Nome, opt => opt.MapFrom(src => src.Nome))
                .ForMember(dst => dst.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dst => dst.Ativo, opt => opt.MapFrom(src => src.EmailConfirmed))
                .ForMember(dst => dst.AtualizacaoSenhaRequisitada, opt => opt.MapFrom(src => src.AtualizacaoSenhaRequisitada))
                .ForMember(dst => dst.AtualizacaoSenhaAprovada, opt => opt.MapFrom(src => src.AtualizacaoSenhaAprovada));
        }
    }
}
