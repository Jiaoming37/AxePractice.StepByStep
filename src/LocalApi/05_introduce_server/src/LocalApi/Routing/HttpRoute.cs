using System;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace LocalApi.Routing
{
    public class HttpRoute
    {
        public HttpRoute(string controllerName, string actionName, HttpMethod methodConstraint) : 
            this(controllerName, actionName, methodConstraint, null)
        {
        }

        #region Please modifies the following code to pass the test

        /*
         * You can add non-public helper method for help, but you cannot change public
         * interfaces.
         */

        void Validate(string controllerName, string actionName, HttpMethod methodConstraint)
        {
            if(controllerName == null) throw new ArgumentNullException(nameof(controllerName));
            if(actionName == null) throw new ArgumentNullException(nameof(actionName));
            if(methodConstraint == null) throw new ArgumentNullException(nameof(methodConstraint));

            const string letters = @"^[a-zA-Z][a-zA-Z0-9]*$";
            if(!Regex.IsMatch(controllerName, letters) || !Regex.IsMatch(actionName, letters)) throw new ArgumentException();
        }

        public HttpRoute(string controllerName, string actionName, HttpMethod methodConstraint, string uriTemplate)
        {
            Validate(controllerName, actionName, methodConstraint);
            ControllerName = controllerName;
            ActionName = actionName;
            MethodConstraint = methodConstraint;
            UriTemplate = uriTemplate;
        }

        #endregion

        public string ControllerName { get; }
        public string ActionName { get; }
        public HttpMethod MethodConstraint { get; }
        public string UriTemplate { get; }

        public bool IsMatch(Uri uri, HttpMethod method)
        {
            if (uri == null) { throw new ArgumentNullException(nameof(uri)); }
            if (method == null) { throw new ArgumentNullException(nameof(method)); }
            string path = uri.AbsolutePath.TrimStart('/');
            return path.Equals(UriTemplate, StringComparison.OrdinalIgnoreCase) &&
                   method == MethodConstraint;
        }
    }
}