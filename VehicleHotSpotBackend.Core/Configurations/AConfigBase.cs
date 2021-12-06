using Microsoft.Extensions.Configuration;

namespace VehicleHotSpotBackend.Core
{
    public class AConfigBase
    {
        protected IConfigurationRoot _configurationRoot;

        protected AConfigBase(IConfigurationRoot configurationRoot)
        {
            _configurationRoot = configurationRoot;
        }

        protected string GetConfigValue(string root, string subpath)
        {
            return GetConfigValue(new string[2] { root, subpath });
        }

        private string GetConfigValue(string[] paths)
        {
            var p = string.Empty;
            foreach ( var path in paths)
            {
                if (p.Length > 0)
                {
                    p = $"{p}:";
                }

                p = $"{p}{path}";
            }

            return _configurationRoot[p];
        }
    }
}