﻿using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;

namespace LocalApi
{
    static class ControllerActionInvoker
    {
<<<<<<< HEAD
        #region Please modify the code to pass the test

        /*
         * Now we will create a framework just like the ASP.NET WebApi. We called
         * it LocalApi. The core part of the Api framework is generating response.
         * And the most important type for generating response is the HttpController.
         * In this practice, we try invoking the specified action of the controller
         * to generate a response.
         * 
         * The class to invoke controller action is called a ControllerActionInvoker,
         * and the entry point is called InvokeAction. It accepts an actionDescriptor
         * which contains the instance of the controller (currently we have no idea
         * where it comes from), the name of the action to invoke. For simplicity,
         * we assume that all actions contains no parameter.
         */

=======
>>>>>>> fe282a8e04facf0c0f0edf76fcf0532928286751
        public static HttpResponseMessage InvokeAction(ActionDescriptor actionDescriptor)
        {
            HttpController httpController = actionDescriptor.Controller;
            var actionName = actionDescriptor.ActionName;

            MethodInfo methodInfo = httpController.GetType()
                .GetMethod(actionName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);

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
<<<<<<< HEAD

        #endregion
=======
>>>>>>> fe282a8e04facf0c0f0edf76fcf0532928286751
    }
}