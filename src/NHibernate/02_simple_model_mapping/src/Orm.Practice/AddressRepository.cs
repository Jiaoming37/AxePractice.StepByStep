using System;
using System.Linq;
using NHibernate;

namespace Orm.Practice
{
    public class AddressRepository
    {
        readonly ISession session;

        public AddressRepository(ISession session)
        {
            if (session == null)
            {
                throw new ArgumentNullException(nameof(session));
            }
            this.session = session;
        }

        public Address Get(int id)
        {
            return session.Query<Address>().FirstOrDefault(a => a.Id == id);
        }
    }
}