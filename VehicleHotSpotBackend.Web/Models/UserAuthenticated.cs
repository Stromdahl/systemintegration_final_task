namespace VehicleHotSpotBackend.Web.Models
{
    public class UserAuthenticated : UserItem
    {
        public string? accessToken { get; set; }
    }
}