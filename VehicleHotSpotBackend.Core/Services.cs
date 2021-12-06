using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace VehicleHotSpotBackend.Core
{
    public class Services
    {
        private Configuration _configuration;
        private InMemoryStorage _inMemoryStorage;
        public void InitServices(IConfigurationBuilder builder)
        {
            _configuration = new Configuration(builder);
        }

        public Configuration Configuration
        {
            get { return _configuration; }
        }


        public InMemoryStorage InMemoryStorage => _inMemoryStorage ??= new InMemoryStorage();
    }
}
