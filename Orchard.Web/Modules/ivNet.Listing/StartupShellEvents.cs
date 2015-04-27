
using AutoMapper;
using ivNet.Listing.Entities;
using ivNet.Listing.Models;
using Orchard.Environment;

namespace ivNet.Listing
{
    public class StartupShellEvents : IOrchardShellEvents
    {
        public void Activated()
        {
            #region models->entities

            Mapper.CreateMap<RegistrationViewModel, Owner>();
            Mapper.CreateMap<RegistrationViewModel, AddressDetail>();
            Mapper.CreateMap<RegistrationViewModel, ContactDetail>();

            #endregion

            #region entities->models
            
            Mapper.CreateMap<ListingDetail,ListingDetailViewModel>()
                .ForMember(v => v.PostCode, m => m.MapFrom(e => e.AddressDetail.Postcode))
                .ForMember(v => v.PaymentPackageName, m => m.MapFrom(e => e.PaymentPackage.Name))                
                .ForMember(v => v.Address, m => m.MapFrom(e => 
                    string.Format("{0}{1} {2}",
                    e.AddressDetail.Address1,
                    string.IsNullOrEmpty(e.AddressDetail.Address2) ? string.Empty : string.Format(" {0}", e.AddressDetail.Address2),
                    e.AddressDetail.Town
                    )));

            Mapper.CreateMap<Category, ListingCategoryViewModel>();
            Mapper.CreateMap<PaymentPackage, ListingPackageViewModel>();

            #endregion
        }

        public void Terminating()
        {

        }
    }
}