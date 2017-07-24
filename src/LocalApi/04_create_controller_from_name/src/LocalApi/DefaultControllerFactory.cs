using System;
using System.Collections.Generic;
using System.Linq;

namespace LocalApi
{
    class DefaultControllerFactory : IControllerFactory
    {
        public HttpController CreateController(
            string controllerName,
            ICollection<Type> controllerTypes,
            IDependencyResolver resolver)
        {
             /*
             * The controller factory will create controller by its name. It will search
             * form the controllerTypes collection to get the correct controller type,
             * then create instance from resolver.
             */

            #region Please modify the following code to pass the test.

            Type type = null;
            try
            {
                type = controllerTypes.SingleOrDefault(t => t.Name.Equals(controllerName, StringComparison.OrdinalIgnoreCase));
            }
            catch
            {
                throw new ArgumentException();
            }

            return type == null ? null : (HttpController)resolver.GetService(type);

            #endregion
        }
    }
}