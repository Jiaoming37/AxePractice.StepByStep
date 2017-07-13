using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using LocalApi.Routing;

namespace LocalApi
{
    public class HttpServer : HttpMessageHandler
    {
        #region Please implement the following method to pass the test

        readonly HttpConfiguration configuration;

        /*
         * An http server is an HttpMessageHandler that accept request and create response.
         * You can add non-public fields and members for help but you should not modify
         * the public interfaces.
         */

        public HttpServer(HttpConfiguration configuration)
        {
            this.configuration = configuration;
        }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpRoute route = configuration.Routes.GetRouteData(request);
            if (route == null)
            {
                return Task.Run(() => new HttpResponseMessage(HttpStatusCode.NotFound), cancellationToken);
            }

            HttpResponseMessage response = ControllerActionInvoker.InvokeAction(
                route,
                configuration.CachedControllerTypes,
                configuration.DependencyResolver,
                configuration.ControllerFactory);
            return Task.Run(() => response, cancellationToken);
        }

        #endregion
    }
}