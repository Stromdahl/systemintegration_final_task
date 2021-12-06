using Microsoft.Extensions.Configuration;

namespace VehicleHotSpotBackend.Core
{
    internal class Configuration
    {
        public ConnectionStringsConfiguration ConnectionStrings { get; set; }

        public SiteSettingsConfigueration SiteSettings { get; set; }

        public Configuration(IConfigurationBuilder builder)
        {
            var configuration = builder.Build();
            ConnectionStrings = new ConnectionStringsConfiguration(configuration);
            SiteSettings = new SiteSettingsConfigueration(configuration);
        }
    }
}