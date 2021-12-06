using Microsoft.Extensions.Configuration;

namespace VehicleHotSpotBackend.Core
{
    public class ConnectionStringsConfiguration : AConfigBase
    {
        private readonly string _root;

        public ConnectionStringsConfiguration(IConfigurationRoot configurationRoot) : base(configurationRoot)
        {
            _root = "ConnectionStrings";
        }

        public string TrashVacDbConnectionString
        {
            get { return base.GetConfigValue(_root, "TrashVacDbConnectionString"); }
        }
    }
}