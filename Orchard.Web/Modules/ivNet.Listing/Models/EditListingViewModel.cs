
namespace ivNet.Listing.Models
{
    public class EditListingViewModel
    {
        public string Package { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Town { get; set; }
        public string Postcode { get; set; }

        public string Rooms { get; set; }
        public string Theatres { get; set; }
        public string TagList { get; set; }
        public string Images { get; set; }
    }
}