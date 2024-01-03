using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Newtonsoft.Json;

namespace Ata.DeloSled.Client
{
    public class RpcBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
    {
        private readonly HttpClient _httpClient;

        public RpcBehavior(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {

            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = NullValueHandling.Include,
                TypeNameHandling = TypeNameHandling.All
            };

            string json = JsonConvert.SerializeObject(request, settings);
            StringContent jsonContent = new StringContent(json, Encoding.UTF8, "application/json");

            HttpRequestMessage message = new(HttpMethod.Post, "/rpc")
            {
                Content = jsonContent
            };
            message.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

            string jsonResponse = await (await _httpClient.SendAsync(message, cancellationToken))
                .Content.ReadAsStringAsync(cancellationToken);

            TResponse responseObject = JsonConvert.DeserializeObject<TResponse>(jsonResponse, settings);

            return responseObject;
        }
    }
}