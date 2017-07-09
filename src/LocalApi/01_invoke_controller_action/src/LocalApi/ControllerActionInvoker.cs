using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;

namespace LocalApi
{
    static class ControllerActionInvoker
    {
        public static HttpResponseMessage InvokeAction(ActionDescriptor actionDescriptor)
        {
            HttpController httpController = actionDescriptor.Controller;
            var actionName = actionDescriptor.ActionName;

            Type type = httpController.GetType();
            MethodInfo methodInfo = type
                .GetMethods(BindingFlags.Instance | BindingFlags.Public)
                .SingleOrDefault(method => method.Name.Equals(actionName, StringComparison.OrdinalIgnoreCase));

            if (methodInfo == null)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }

            try
            {
                return (HttpResponseMessage) methodInfo.Invoke(httpController, null);
            }
            catch
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }
    }
}