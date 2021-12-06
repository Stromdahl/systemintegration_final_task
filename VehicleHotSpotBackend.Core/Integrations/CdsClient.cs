using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using System.Threading.Tasks;
using RestSharp;
using VehicleHotSpot.Entity.Cds;

namespace VehicleHotSpotBackend.Core.Integrations
{
    
    public class CdsClient
    {
        private readonly IRestClient _restClient;

        public CdsClient()
        {
            string baseUrl = "https://kyhdev.hiqcloud.net/api/cds/";
            string version = "v1.0";

            _restClient = new RestClient(baseUrl + version);
        }

        public LoginResponse Login(string userName, string password)
        {
            var request = 
                new RestRequest($"user/authenticate?userName={HttpUtility.UrlEncode(userName)}&pwd={HttpUtility.UrlEncode(password)}",
                Method.GET);
            return Execute<LoginResponse>(request);
        }

        private T? Execute<T>(IRestRequest request)
        {
            var response = _restClient.Execute(request);

            if (response.IsSuccessful)
            {
                var result = JsonConvert.DeserializeObject<T>(response.Content);
                return (T)(object)result;
            }

            return default;
        }
    }
}
