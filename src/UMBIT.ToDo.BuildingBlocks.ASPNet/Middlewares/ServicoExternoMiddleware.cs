using System.Text;
using System.Text.Json;
using UMBIT.ToDo.BuildingBlocks.Basicos.Excecoes;
using UMBIT.ToDo.BuildingBlocksc.ASPNet.Basicos.Exececoes;
using UMBIT.ToDo.BuildingBlocksc.WebAPI.Models;
using static UMBIT.ToDo.BuildingBlocksc.ASPNet.Bootstrapper.ContextConfigurate;

namespace UMBIT.ToDo.BuildingBlocksc.ASPNet.Middlewares
{
    public class ServicoExternoMiddleware : DelegatingHandler
    {
        private readonly AuthSessionContext _authSessionContext;
        public ServicoExternoMiddleware(AuthSessionContext authSessionContext)
        {
            _authSessionContext = authSessionContext;
        }

        protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            TrateRequest(request);

            return TrateResposta(base.Send(request, cancellationToken));
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            TrateRequest(request);
            return TrateResposta(await base.SendAsync(request, cancellationToken));
        }

        private HttpResponseMessage TrateResposta(HttpResponseMessage response)
        {
            try
            {

                var stringResposta = response?.Content?.ReadAsStringAsync().Result;

                if (TenteRepostaPadrao(stringResposta, out Resposta respotaPadrao))
                {
                    if (!respotaPadrao.Sucesso)
                    {

                        var ex = new ExcecaoServicoExterno("Falha na resposta padrão!", respotaPadrao);

                        throw ex;
                    }

                    var contentString = JsonSerializer.Serialize(respotaPadrao.Dados);
                    response.Content = new StringContent(contentString, Encoding.UTF8, "application/json");
                    return response;

                }
                else if (response.IsSuccessStatusCode)
                    return TenteRepostaComum(response, stringResposta);

                throw new Exception($"Falha no uso do serviço, {response.RequestMessage?.RequestUri?.AbsolutePath} respondeu {response.ReasonPhrase}.");
            }
            catch (ExcecaoServicoExterno ex)
            {
                throw ex;
            }
            catch (ExcecaoBasicaUMBIT ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro no uso do serviço, algo de errado aconteceu em {response.RequestMessage?.RequestUri?.AbsolutePath}.", ex);
            }
        }

        private HttpRequestMessage TrateRequest(HttpRequestMessage request)
        {
            if (_authSessionContext.EhAutenticado)
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _authSessionContext.ObterToken());

            return request;
        }

        private bool TenteRepostaPadrao(string httpResponseMessage, out Resposta resposta)
        {
            try
            {
                resposta = JsonSerializer.Deserialize<Resposta>(httpResponseMessage);
                return resposta != null;
            }
            catch { resposta = null; return false; }
        }
        private HttpResponseMessage TenteRepostaComum(HttpResponseMessage httpResponseMessage, string stringResposta)
        {
            try
            {
                httpResponseMessage.Content = new StringContent(stringResposta, Encoding.UTF8, "application/json");
                return httpResponseMessage;
            }

            catch { return null; }
        }

    }
}
