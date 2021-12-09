namespace VehicleHotSpotBackend.Web.Models
{
    public class LoginResponse
    {
        public string accessToken { get; set; }
        public Guid id { get; set; }
        public string displayName { get; set; }
    }
}
