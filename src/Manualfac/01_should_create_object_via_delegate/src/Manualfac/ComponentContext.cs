﻿using System;
using System.Collections.Generic;

namespace Manualfac
{
    public class ComponentContext : IComponentContext
    {
        readonly IDictionary<Type, object> registrations;

        public ComponentContext(IDictionary<Type, object> registrations)
        {
            this.registrations = registrations;
        }

        #region Please modify the following code to pass the test

        /*
         * A ComponentContext is used to resolve a component. Since the component
         * is created by the ContainerBuilder, it brings all the registration
         * information. 
         * 
         * You can add non-public member functions or member variables as you like.
         */
        public object ResolveComponent(Type type)
        {
            if (!registrations.ContainsKey(type))
            {
                throw new DependencyResolutionException();
            }
            return registrations[type];
        }

        #endregion
    }
}