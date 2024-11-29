using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation.Results;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using UMBIT.ToDo.BuildingBlocks.Basicos.Notificacoes;
using UMBIT.ToDo.BuildingBlocks.Core.Notificacao.Interfaces;
using UMBIT.ToDo.BuildingBlocks.Message.Bus.MediatorBus;
using UMBIT.ToDo.BuildingBlocks.Message.Messagem;
using UMBIT.ToDo.BuildingBlocksc.WebAPI.Models;

namespace UMBIT.ToDo.BuildingBlocks.WebAPI.Controllers
{

    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        protected readonly IMapper Mapper;
        protected readonly IMediatorBus Mediator;
        protected readonly INotificador Notificador;
        protected readonly IHostingEnvironment Environment;

        protected virtual bool EhValido()
            => !Notificador.TemNotificacoes();

        public BaseController(IServiceProvider serviceProvider)
        {
            Mapper = serviceProvider.GetService<IMapper>()!;
            Mediator = serviceProvider.GetService<IMediatorBus>()!;
            Notificador = serviceProvider.GetService<INotificador>()!;
            Environment = serviceProvider.GetService<IHostingEnvironment>()!;
        }

        protected virtual IActionResult UMBITResponse(object? resposta = null)
        {
            var dadosResposta = RespostaPadrao(resposta);

            if (dadosResposta.Sucesso)
                return Ok(dadosResposta);

            return BadRequest(dadosResposta);
        }

        protected virtual IActionResult UMBITResponse(UMBITMessageResponse resposta)
        {
            var dadosResposta = RespostaPadrao(resposta);

            if (dadosResposta.Sucesso)
                return Ok(dadosResposta);

            return BadRequest(dadosResposta);
        }

        protected virtual ActionResult<T> UMBITCustomResponse<T>(T? resposta = null) where T : class
        {
            var dadosResposta = RespostaPadrao(resposta);

            if (dadosResposta.Sucesso)
                return Ok(dadosResposta);

            return BadRequest(dadosResposta);
        }

        protected virtual ActionResult<T> UMBITResponse<T>(T? resposta = null) where T : class
        {
            var dadosResposta = RespostaPadrao(resposta);

            if (dadosResposta.Sucesso)
                return Ok(dadosResposta);

            return BadRequest(dadosResposta);
        }


        protected virtual ActionResult<TResp> UMBITResponseEntity<TResp, TModel>(TModel? resposta = null)
            where TResp : class
            where TModel : class
        {
            var dadosResposta = resposta != null ? RespostaPadrao<TResp, TModel>(resposta) : RespostaPadrao();

            if (dadosResposta.Sucesso)
                return Ok(dadosResposta.Dados);

            return BadRequest(dadosResposta);
        }

        protected virtual ActionResult<Resposta<TResp>> UMBITResponse<TResp, TModel>(TModel? resposta = null)
            where TResp : class
            where TModel : class
        {
            var dadosResposta = resposta != null ? RespostaPadrao<TResp, TModel>(resposta) : RespostaPadrao();

            if (dadosResposta.Sucesso)
                return Ok(dadosResposta);

            return BadRequest(dadosResposta);
        }

        protected virtual ActionResult<TResp> UMBITResponseEntity<TResp, TModel>(UMBITMessageResponse<TModel>? resposta = null)
    where TResp : class
    where TModel : class
        {
            var dadosResposta = resposta != null ? RespostaPadrao<TResp, TModel>(resposta) : RespostaPadrao();

            if (dadosResposta.Sucesso)
                return Ok(dadosResposta.Dados);

            return BadRequest(dadosResposta);
        }

        protected virtual ActionResult<TResp> UMBITResponse<TResp, TModel>(UMBITMessageResponse<TModel>? resposta = null)
            where TResp : class
            where TModel : class
        {
            var dadosResposta = resposta != null ? RespostaPadrao<TResp, TModel>(resposta) : RespostaPadrao();

            if (dadosResposta.Sucesso)
                return Ok(dadosResposta);

            return BadRequest(dadosResposta);
        }


        protected virtual ActionResult<ICollection<TResp>> UMBITCollectionResponseEntity<TResp, TModel>(IQueryable<TModel>? resposta)
                   where TResp : class
                   where TModel : class
        {
            var dadosResposta = resposta != null ? RespostaCollectionPadrao<TResp, TModel>(resposta) : RespostaPadrao();

            if (dadosResposta.Sucesso)
                return Ok(dadosResposta.Dados);

            return BadRequest(dadosResposta);
        }

        protected virtual ActionResult<Resposta<ICollection<TResp>>> UMBITCollectionResponse<TResp, TModel>(IQueryable<TModel>? resposta)
            where TResp : class
            where TModel : class
        {
            var dadosResposta = resposta != null ? RespostaCollectionPadrao<TResp, TModel>(resposta) : RespostaPadrao();

            if (dadosResposta.Sucesso)
                return Ok(dadosResposta);

            return BadRequest(dadosResposta);
        }

        protected virtual ActionResult<ICollection<TResp>> UMBITCollectionResponseEntity<TResp, TModel>(UMBITMessageResponse<IQueryable<TModel>>? resposta)
            where TResp : class
            where TModel : class
        {
            var dadosResposta = resposta != null && resposta.Dados != null ? RespostaCollectionPadrao<TResp, TModel>(resposta) : RespostaPadrao();

            if (dadosResposta.Sucesso)
                return Ok(dadosResposta.Dados);

            return BadRequest(dadosResposta);
        }

        protected virtual ActionResult<ICollection<TResp>> UMBITCollectionResponse<TResp, TModel>(UMBITMessageResponse<IQueryable<TModel>>? resposta)
            where TResp : class
            where TModel : class
        {
            var dadosResposta = resposta != null && resposta.Dados != null ? RespostaCollectionPadrao<TResp, TModel>(resposta) : RespostaPadrao();

            if (dadosResposta.Sucesso)
                return Ok(dadosResposta);

            return BadRequest(dadosResposta);
        }

        private Resposta RespostaPadrao(UMBITMessageResponse resposta)
        {
            var dadosResposta = new Resposta();
            dadosResposta.Sucesso = EhValido() && resposta.Result.IsValid;
            dadosResposta.Erros = ObterErros();

            if (Environment.IsDevelopment())
                dadosResposta.ErrosSistema = Notificador.ObterErrosSistema();

            return dadosResposta;
        }
        private Resposta RespostaPadrao<TResp, TModel>(UMBITMessageResponse<TModel> resposta)
            where TResp : class
            where TModel : class
        {
            var dadosResposta = new Resposta();
            dadosResposta.Sucesso = EhValido() && resposta.Result.IsValid;
            dadosResposta.Dados = Mapper.Map<TResp>(resposta.Dados);
            dadosResposta.Erros = ObterErros();

            if (Environment.IsDevelopment())
                dadosResposta.ErrosSistema = Notificador.ObterErrosSistema();

            return dadosResposta;
        }
        private Resposta RespostaPadrao<TResp, TModel>(TModel resposta)
            where TResp : class
            where TModel : class
        {
            var dadosResposta = new Resposta();
            dadosResposta.Sucesso = EhValido();
            dadosResposta.Erros = ObterErros();
            dadosResposta.Dados = Mapper.Map<TResp>(resposta);

            if (Environment.IsDevelopment())
                dadosResposta.ErrosSistema = Notificador.ObterErrosSistema();

            return dadosResposta;
        }
        private Resposta RespostaPadrao(object? resposta = null)
        {
            var validation = resposta as ValidationResult ?? new ValidationResult();

            var dadosResposta = new Resposta();
            dadosResposta.Sucesso = EhValido() && validation.IsValid;
            dadosResposta.Dados = resposta is ValidationResult ? null : resposta;
            dadosResposta.Erros = ObterErros();

            if (Environment.IsDevelopment())
                dadosResposta.ErrosSistema = Notificador.ObterErrosSistema();

            return dadosResposta;
        }
        private Resposta RespostaCollectionPadrao<TResp, TModel>(IQueryable<TModel> resposta)
        {
            var dadosResposta = new Resposta();
            dadosResposta.Sucesso = EhValido();
            dadosResposta.Dados = resposta.ProjectTo<TResp>(Mapper.ConfigurationProvider);
            dadosResposta.TotalCount = resposta.Count();
            dadosResposta.Erros = ObterErros();

            if (Environment.IsDevelopment())
                dadosResposta.ErrosSistema = Notificador.ObterErrosSistema();

            return dadosResposta;
        }
        private Resposta RespostaCollectionPadrao<TResp, TModel>(UMBITMessageResponse<IQueryable<TModel>> resposta)
            where TResp : class
            where TModel : class
        {
            var dadosResposta = new Resposta();
            dadosResposta.Sucesso = EhValido() && resposta.Result.IsValid;
            dadosResposta.Dados = resposta.Dados != null ? resposta.Dados.ProjectTo<TResp>(Mapper.ConfigurationProvider) : null;
            dadosResposta.TotalCount = resposta.Dados != null ? resposta.Dados.Count() : null;
            dadosResposta.Erros = ObterErros();

            if (Environment.IsDevelopment())
                dadosResposta.ErrosSistema = Notificador.ObterErrosSistema();

            return dadosResposta;
        }
        private List<NotificacaoPadrao> ObterErros()
        {
            var erros = new List<NotificacaoPadrao>();

            if (!EhValido())
                erros.AddRange(Notificador.ObterNotificacoes());

            return erros;
        }
    }

}
