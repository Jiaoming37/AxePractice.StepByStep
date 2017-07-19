using System;

namespace Manualfac.Activators
{
    class DelegatedInstanceActivator : IInstanceActivator
    {
        readonly Func<IComponentContext, object> func;

        #region Please modify the following code to pass the test

        /*
         * We have create an interface for activators so that we can extend them easily.
         * Please migrate the delegate activator to this class.
         * 
         * No public members are allowed to create.
         */

        public DelegatedInstanceActivator(Func<IComponentContext, object> func)
        {
            this.func = func;
        }

        public object Activate(IComponentContext componentContext)
        {
            return func(componentContext);
        }

        #endregion
    }
}