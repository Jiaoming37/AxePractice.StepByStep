using System;
using NHibernate;

namespace Orm.Practice
{
    public abstract class RepositoryBase
    {
        protected ISession Session { get; }

        protected RepositoryBase(ISession session)
        {
            if (session == null)
            {
                throw new ArgumentNullException(nameof(session));
            }
            Session = session;
        }
    }
}