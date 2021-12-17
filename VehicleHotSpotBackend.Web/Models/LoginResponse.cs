namespace VehicleHotSpotBackend.Web.Models
{
    public class LoginResponse
    {
        public string accessToken { get; set; }
        public Guid customerId { get; set; }
        public string displayName { get; set; }
    }
}
