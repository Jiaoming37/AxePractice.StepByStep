using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Criterion;

namespace Orm.Practice
{
    public class AddressRepositoryQueryOverImpl : RepositoryBase, IAddressRepository
    {
        public AddressRepositoryQueryOverImpl(ISession session) : base(session)
        {
        }

        public Address Get(int id)
        {
            #region Please implement the method

            return Session.QueryOver<Address>().Where(address => address.Id == id).SingleOrDefault();

            #endregion
        }

        public IList<Address> Get(IEnumerable<int> ids)
        {
            #region Please implement the method

            return Session.QueryOver<Address>().WhereRestrictionOn(a => a.Id).IsIn(ids.ToList()).List();

            #endregion
        }

        public IList<Address> GetByCity(string city)
        {
            #region Please implement the method

            return Session.QueryOver<Address>().Where(a => a.City == city).OrderBy(a => a.Id).Asc.List();

            #endregion
        }

        public Task<IList<Address>> GetByCityAsync(string city)
        {
            #region Please implement the method

            return GetByCityAsync(city, CancellationToken.None);

            #endregion
        }

        public Task<IList<Address>> GetByCityAsync(string city, CancellationToken cancellationToken)
        {
            #region Please implement the method

            return Session.QueryOver<Address>().Where(a => a.City == city).OrderBy(a => a.Id).Asc.ListAsync(cancellationToken);

            #endregion
        }

        public IList<KeyValuePair<int, string>> GetOnlyTheIdAndTheAddressLineByCity(string city)
        {
            #region Please implement the method

            return Session.QueryOver<Address>()
                .Where(a => a.City == city)
                .OrderBy(a => a.Id).Asc
                .Select(a => a.Id, a => a.AddressLine1)
                .List<object[]>()
                .Select(a => new KeyValuePair<int, string>((int) a[0], (string) a[1]))
                .ToList();

            #endregion
        }

        public IList<string> GetPostalCodesByCity(string city)
        {
            #region Please implement the method

            return Session.QueryOver<Address>()
                .Where(a => a.City == city)
                .Select(a => a.PostalCode)
                .Select(Projections.Distinct(Projections.Property<Address>(a => a.PostalCode)))
                .List<string>();

            #endregion
        }
    }
}