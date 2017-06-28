using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SimpleSolution.WebApp
{
    public class ResourceController : ApiController
    {
        [HttpGet]
        [HttpPut]
        [HttpPost]
        [HttpDelete]
        public HttpResponseMessage Get()
        {
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}