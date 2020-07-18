using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GodelTech.Microservices.Swagger.Tests.Utils
{
    /// <summary>
    /// properly handles HTTP/2 requests
    /// </summary>
    public class ResponseVersionHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);
            response.Version = request.Version;

            return response;
        }
    }
}