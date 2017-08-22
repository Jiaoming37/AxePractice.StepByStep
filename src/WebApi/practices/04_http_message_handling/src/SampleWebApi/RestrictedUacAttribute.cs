﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Dependencies;
using System.Web.Http.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SampleWebApi.Services;

namespace SampleWebApi
{
    /*
     * A RestrictedUacAttribute is a filter to eliminate sensitive information to
     * the client. A resource contains management information that is represented
     * by a collection of links. These links will be represented as a array of
     * objects in JSON. And each object must contains an attribute called 
     * "restricted". If it is true, then it should be eliminated if the client
     * is a normal user. If it is false, then the information can be seen by both
     * normal user and administrators.
     * 
     * NOTE. You are free to add non-public members or methods in the class.
     */
    public class RestrictedUacAttribute : ActionFilterAttribute
    {
        #region Please implement the class to pass the test

        readonly string userIdArgumentName;

        /*
         * The attribute takes an argument of the name of the userId parameter in
         * the route. For example, if the request route definition is 
         * 
         * http://www.base.com/user/{userId}/resource/type
         * 
         * Then the userId parameter name in the route is "userId". The attribute
         * will try resolving the parameter and determine the role of the user by
         * passing the parameter to a RoleRepository. And that is why we ask for
         * it.
         */
        public RestrictedUacAttribute(string userIdArgumentName)
        {
            this.userIdArgumentName = userIdArgumentName ?? throw new ArgumentNullException(nameof(userIdArgumentName));
        }


        /*
         * The action filter for ASP.NET web API is async nativelly. So we simply
         * abandon the sync version of OnActionExecuted, instead, we will implement
         * the async version directly.
         * 
         * Please carefully implement the method to pass all the tests.
         */
        public override async Task OnActionExecutedAsync(
            HttpActionExecutedContext context,
            CancellationToken token)
        {
            if(context == null) throw new ArgumentNullException(nameof(context));
            if(!IsSucessStatusCode(context))  return;
            if(context.Response.Content == null) return;
            Dictionary<string, object> actionArguments
                = context.ActionContext?.ActionArguments ?? throw new HttpResponseException(HttpStatusCode.BadRequest);

            var userId = GetUserId(actionArguments);

            var resonse = await context.Response.Content.ReadAsStringAsync();
            var resource = JsonConvert.DeserializeObject<object>(resonse) as JObject;
            if (resource == null) return;

            if(context.Request == null) return;
            var service = ResolveService(context);

            var removeRestrictedInfo = service.RemoveRestrictedInfo(userId, resource);
            if(!removeRestrictedInfo) return;

            context.Response.Content = 
                new ObjectContent<JObject>(resource, context.ActionContext.ControllerContext.Configuration.Formatters.JsonFormatter);
        }

        static RestrictedUacContractService ResolveService(HttpActionExecutedContext context)
        {
            IDependencyScope scope = context.Request.GetDependencyScope();
            if (scope == null) throw new HttpResponseException(HttpStatusCode.InternalServerError);
            object service = scope.GetService(typeof(RestrictedUacContractService));
            if (service == null) throw new HttpResponseException(HttpStatusCode.InternalServerError);
            return (RestrictedUacContractService)service;
        }

        long GetUserId(Dictionary<string, object> actionArguments)
        {
            long userId;
            if (!actionArguments.ContainsKey(userIdArgumentName))
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            try
            {
                userId = (long)actionArguments[userIdArgumentName];
            }
            catch (InvalidCastException)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
            return userId;
        }

        static bool IsSucessStatusCode(HttpActionExecutedContext context)
        {
            if (context.Exception != null) return false;
            if (context.Response == null) return false;
            return context.Response.IsSuccessStatusCode;
        }

        #endregion
    }
}