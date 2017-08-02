using System;
using LocalApi;

namespace Manualfac.LocalApiIntegration
{
    public class ManualfacDependencyResolver : IDependencyResolver
    {
        readonly Container rootScope;

        #region Please implement the following class

        /*
         * We should create a manualfac dependency resolver so that we can integrate it
         * to LocalApi.
         * 
         * You can add a public/internal constructor and non-public fields if needed.
         */

        public ManualfacDependencyResolver(Container rootScope)
        {
            this.rootScope = rootScope;
        }

        public void Dispose()
        {
            rootScope.Dispose();
        }

        public object GetService(Type type)
        {
            return rootScope.Resolve(type);
        }

        public IDependencyScope BeginScope()
        {
            return new ManualfacDependencyScope(rootScope.BeginLifetimeScope());
        }

        #endregion
    }
}