﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using ivNet.Listing.Entities;
using ivNet.Listing.Helpers;
using ivNet.Listing.Models;
using Newtonsoft.Json;
using Orchard;
using Orchard.Security;

namespace ivNet.Listing.Service
{
    public interface IOwnerServices : IDependency
    {
        int GetOwnerIdByKey(string ownerKey);
        void AddListing(int ownerId, EditListingViewModel model);
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

        public void AddListing(int ownerId, EditListingViewModel model)
        {            
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {                   
                    // get existing entities
                    var owner = session.CreateCriteria(typeof (Owner))
                        .List<Owner>().FirstOrDefault(x => x.Id.Equals(ownerId));

                    var category = session.CreateCriteria(typeof(Category))
                       .List<Category>().FirstOrDefault(x => x.Id.Equals(Convert.ToInt32(model.Category)));

                    var package = session.CreateCriteria(typeof(PaymentPackage))
                       .List<PaymentPackage>().FirstOrDefault(x => x.Id.Equals(Convert.ToInt32(model.Package)));

                    var addressDetailKey = CustomStringHelper.BuildKey(new[] { model.Address1, model.Postcode });

                    // get potentially new entities and save them
                    // address
                    var address = session.CreateCriteria(typeof(AddressDetail))
                        .List<AddressDetail>().FirstOrDefault(x => x.AddressDetailKey.Equals(addressDetailKey)) ??
                                  new AddressDetail();

                    address.Address1 = model.Address1;
                    address.Address2 = model.Address2;
                    address.Town = model.Town;
                    address.Postcode = model.Postcode;
                    address.IsActive = 1;
                    address.AddressDetailKey = addressDetailKey;

                    SetAudit(address);
                    session.SaveOrUpdate(address);                    

                    // location
                    var location = session.CreateCriteria(typeof(Location))
                       .List<Location>().FirstOrDefault(x => 
                           x.Postcode.ToLowerInvariant().Equals(model.Postcode.ToLowerInvariant())) ??
                                 new Location();

                    // get geolocation from postcode
                    //var requestUri = string.Format("http://maps.googleapis.com/maps/api/geocode/xml?address={0}&sensor=false", Uri.EscapeDataString(model.Postcode));
                    //var request = WebRequest.Create(requestUri);
                    //var response = request.GetResponse();
                    //var xdoc = XDocument.Load(response.GetResponseStream());

                    location.Postcode = model.Postcode;
                    location.IsActive = 1;
                    
                    SetAudit(location);
                    session.SaveOrUpdate(location);        

                    // listing
                    var listing = session.CreateCriteria(typeof(ListingDetail))
                       .List<ListingDetail>().FirstOrDefault(x =>
                           x.ListingKey.Equals(addressDetailKey)) ??
                                 new ListingDetail();

                    if (listing.Id == 0) listing.Init();

                    listing.ExpiraryDate = DateTime.Now;

                    listing.Description = model.Description;
                    listing.AddressDetail = address;
                    listing.Owner = owner;
                    listing.PaymentPackage = package;
                    listing.Category = category;
                    listing.Location = location;

                    listing.ListingKey = addressDetailKey;
                    listing.IsActive = 1;

                    // rooms
                    var rooms = JsonConvert.DeserializeObject<List<RoomViewModel>>(model.Rooms);               
                    foreach (var roomViewModel in rooms)
                    {
                        var room = new Room {Description = roomViewModel.Description, Type = roomViewModel.RoomType};
                        SetAudit(room);
                        room.IsActive = 1;
                        session.SaveOrUpdate(room);       
                        listing.Rooms.Add(room);
                    }

                    // theatres
                    var theatres = JsonConvert.DeserializeObject<List<TheatreViewModel>>(model.Theatres);
                    foreach (var theatreViewModel in theatres)
                    {
                        var theatre = new Theatre
                        {
                            Name = theatreViewModel.Name,
                            Town = theatreViewModel.Town,
                            Distance = theatreViewModel.Distance,
                            Transport = theatreViewModel.Transport
                        };
                        SetAudit(theatre);
                        theatre.IsActive = 1;
                        session.SaveOrUpdate(theatre);       
                        listing.Theatres.Add(theatre);
                    }

                    // images
                    var images = JsonConvert.DeserializeObject<List<ImageViewModel>>(model.Images);
                    foreach (var imageViewModel in images)
                    {
                        var image = new Image { Alt = "Owner Image", 
                            ThumbUrl = imageViewModel.File,
                                                LargeUrl = imageViewModel.File,
                                                DisplayOrder = images.IndexOf(imageViewModel)
                        };
                        SetAudit(image);
                        image.IsActive = 1;
                        session.SaveOrUpdate(image);       
                        listing.Images.Add(image);
                    }

                    // tags
                    var taglist = model.TagList.Split(',');
                    foreach (var strTag in taglist)
                    {
                        var tag = new Tag { Name = strTag };
                        SetAudit(tag);
                        tag.IsActive = 1;
                        session.SaveOrUpdate(tag);       
                        listing.Tags.Add(tag);
                    }

                    SetAudit(listing);
                    listing.IsActive = 1;
                    session.SaveOrUpdate(listing); 

                    transaction.Commit();
                }
            }                                                
        }
    }
}