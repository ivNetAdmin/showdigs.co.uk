
using AutoMapper;
using ivNet.Listing.Entities;
using ivNet.Listing.Models;

namespace ivNet.Listing.Helpers
{
    public class MapperHelper
    {
        #region models->entities

        public static Owner Map(Owner entity, RegistrationViewModel viewModel)
        {
            return Mapper.Map(viewModel, entity);
        }
        public static AddressDetail Map(AddressDetail entity, RegistrationViewModel viewModel)
        {
            return Mapper.Map(viewModel, entity);
        }

        public static ContactDetail Map(ContactDetail entity, RegistrationViewModel viewModel)
        {
            return Mapper.Map(viewModel, entity);
        }

        #endregion
    }
}