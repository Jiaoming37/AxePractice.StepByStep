using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace HandleResponsePractice
{
    public class MessageHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return base.SendAsync(request, cancellationToken)
                .ContinueWith(t =>
                {
                    HttpResponseMessage response = t.Result;
                    MediaTypeWithQualityHeaderValue mediaTypeWithQualityHeaderValue = request.Headers.Accept.FirstOrDefault();
                    if (mediaTypeWithQualityHeaderValue != null)
                    {
                        response.Content.Headers.ContentType = mediaTypeWithQualityHeaderValue;
                    }
                    return response;
                }, TaskContinuationOptions.OnlyOnRanToCompletion);
        }
    }
}