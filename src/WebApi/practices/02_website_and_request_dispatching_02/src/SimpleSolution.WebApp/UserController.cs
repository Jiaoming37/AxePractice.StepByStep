using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SimpleSolution.WebApp
{
    public class UserController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage GetUserById(int id)
        {
            return Request.Text(HttpStatusCode.OK, $"user(id={id}) found");
        }

        [HttpGet]
        public HttpResponseMessage GetUserByName(string name)
        {
            return Request.Text(HttpStatusCode.OK, $"user(name={name}) found");
        }

        [HttpPut]
        public HttpResponseMessage UpdateUserById(int id, string name)
        {
            return Request.Text(HttpStatusCode.OK, $"user(id={id})'s name updated to {name}");
        }

        [HttpGet]
        public HttpResponseMessage GetUserDependets()
        {
            return Request.Text(HttpStatusCode.OK, "This will get all users's dependents");
        }

        [HttpGet]
        public HttpResponseMessage GetUserDependetById(int id)
        {
            return Request.Text(HttpStatusCode.OK, $"This will get user(id={id})'s dependents");
        }
    }
}