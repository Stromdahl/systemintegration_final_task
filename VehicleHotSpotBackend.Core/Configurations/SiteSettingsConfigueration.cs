using Microsoft.Extensions.Configuration;

namespace VehicleHotSpotBackend.Core
{

    public class SiteSettingsConfigueration : AConfigBase
    {
        private readonly string _root;

        public SiteSettingsConfigueration(IConfigurationRoot configurationRoot) : base(configurationRoot)
        {
            _root = "SiteSettings";
        }

        public int AccessTokenTtl
        {
            get { return Convert.ToInt32(base.GetConfigValue(_root, "AccessTokenTTL")); }
        }

        public string StaticAccessToken
        {
            get { return base.GetConfigValue(_root, "StaticAccessToken"); }
        }
    }
}