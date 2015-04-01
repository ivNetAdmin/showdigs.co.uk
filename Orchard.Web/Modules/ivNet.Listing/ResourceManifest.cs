
using Orchard.UI.Resources;

namespace ivNet.Listing
{
    public class ResourceManifest : IResourceManifestProvider
    {
        public void BuildManifests(ResourceManifestBuilder builder)
        {
            var manifest = builder.Add();

            manifest.DefineScript("jQuery.Validate").SetUrl("jquery.validate.min.js").SetVersion("1.0").SetDependencies("jQueryUI");
            manifest.DefineScript("jQuery.Validate.Unobtrusive").SetUrl("jquery.validate.unobtrusive.min.js").SetVersion("1.0").SetDependencies("jQuery.Validate");

            manifest.DefineScript("ivNet.Listing.Registration").SetUrl("ivNet/listing.registration.min.js").SetVersion("1.0").SetDependencies("jQuery.Validate.Unobtrusive");
            
        }
    }
}