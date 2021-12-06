using Microsoft.AspNetCore.Mvc;
using VehicleHotSpotBackend.Core.Integrations;
using VehicleHotSpot.Entity.Cds;
using VehicleHotSpotBackend.Core;


namespace VehicleHotSpotBackend.Web.Controllers
{
    [Route("api/cdsauthenticate")]
    [ApiController]
    public class CdsAuthenticateController : ControllerBase
    {
        private readonly CdsClient _cdsClient;
        public CdsAuthenticateController()
        {
            _cdsClient = new CdsClient();
        }

        [HttpGet]
        [Route("login")]
        public ActionResult<LoginResponse> LoginCds(string userName, string pwd)
        {
            var result = _cdsClient.Login(userName, pwd);
            if (result == null)
            {
                var inMemoryStorage = new InMemoryStorage();
                inMemoryStorage.AddToken(result.AccessToken, result.Id);

                Core.ServiceProvider.Current.InMemoryStorage.AddToken(result.AccessToken, result.Id);
                return new UnauthorizedResult();
            }
            return new OkObjectResult(result);
        }
    }
}
