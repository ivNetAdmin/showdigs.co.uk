
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
        }

        public void Terminating()
        {

        }
    }
}