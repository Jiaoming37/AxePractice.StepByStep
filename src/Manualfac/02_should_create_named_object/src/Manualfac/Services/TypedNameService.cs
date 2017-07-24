using System;

namespace Manualfac.Services
{
    class TypedNameService : Service, IEquatable<TypedNameService>
    {
        readonly Type serviceType;
        readonly string name;

        #region Please modify the following code to pass the test

        /*
         * This class is used as a key for registration by both type and name.
         */

        public TypedNameService(Type serviceType, string name)
        {
            this.serviceType = serviceType;
            this.name = name;
        }

        public bool Equals(TypedNameService other)
        {
            if (other == null) return false;
            return serviceType == other.serviceType && name == other.name;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is TypedNameService)) return false;
            return this.Equals((TypedNameService) obj);
        }

        public override int GetHashCode()
        {
            return serviceType.GetHashCode() ^ name.GetHashCode();
        }

        #endregion
    }
}