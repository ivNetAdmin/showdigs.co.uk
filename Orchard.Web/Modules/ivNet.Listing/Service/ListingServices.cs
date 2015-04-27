﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ivNet.Listing.Entities;
using ivNet.Listing.Helpers;
using ivNet.Listing.Models;
using NHibernate.Mapping;
using Orchard;
using Orchard.Security;

namespace ivNet.Listing.Service
{
    public interface IListingServices : IDependency
    {
        IEnumerable<ListingDetailViewModel> GetListings(string eMail);
        IEnumerable<ListingCategoryViewModel> GetListingCategories();
        IEnumerable<ListingPackageViewModel> GetListingPackages();
    }

    public class ListingServices : BaseService, IListingServices
    {
        public ListingServices(IAuthenticationService authenticationService) 
            : base(authenticationService)
        {
        }

        public IEnumerable<ListingDetailViewModel> GetListings(string eMail)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                var listingDetailList = session.CreateCriteria(typeof (ListingDetail))
                    .List<ListingDetail>().Where(x => x.Owner.ContactDetail.Email.Equals(eMail));

                return (from listingDetail in listingDetailList
                        let listingDetailViewModel = new ListingDetailViewModel()
                        select MapperHelper.Map(listingDetailViewModel, listingDetail)).ToList();

            }            
        }

        public IEnumerable<ListingCategoryViewModel> GetListingCategories()
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                var listingCategoryList = session.CreateCriteria(typeof (Category))
                    .List<Category>().OrderBy(x => x.Name);

                return (from listingCategory in listingCategoryList
                        let listingCategoryViewModel = new ListingCategoryViewModel()
                        select MapperHelper.Map(listingCategoryViewModel, listingCategory)).ToList();

            }       
        }

        public IEnumerable<ListingPackageViewModel> GetListingPackages()
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                var listingPackageList = session.CreateCriteria(typeof(PaymentPackage))
                    .List<PaymentPackage>().OrderBy(x => x.Name);

                return (from listingPackage in listingPackageList
                        let listingPackageViewModel = new ListingPackageViewModel()
                        select MapperHelper.Map(listingPackageViewModel, listingPackage)).ToList();

            }
        }
        
    }
}