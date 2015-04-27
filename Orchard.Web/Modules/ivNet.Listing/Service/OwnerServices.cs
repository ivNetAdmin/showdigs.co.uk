
using System.Linq;
using ivNet.Listing.Entities;
using ivNet.Listing.Helpers;
using Orchard;
using Orchard.Security;

namespace ivNet.Listing.Service
{
    public interface IOwnerServices : IDependency
    {
        int GetOwnerIdByKey(string ownerKey);
    }

    public class OwnerServices : BaseService, IOwnerServices
    {
        public OwnerServices(IAuthenticationService authenticationService) 
            : base(authenticationService)
        {
        }

        public int GetOwnerIdByKey(string ownerKey)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                var firstOrDefault = session.CreateCriteria(typeof (Owner))
                    .List<Owner>().FirstOrDefault(x => x.OwnerKey.Equals(ownerKey));
                if (firstOrDefault != null)
                    return firstOrDefault.Id;
            }

            return 0;
        }
    }
}