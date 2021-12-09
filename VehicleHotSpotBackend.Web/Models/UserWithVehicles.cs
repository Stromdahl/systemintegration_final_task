namespace VehicleHotSpotBackend.Web.Models
{
    public class UserWithVehicles:UserItem
    {
        public UserWithVehicles()
        {
            vehicles = new List<VehicleItem>();
        }
        public List<VehicleItem> vehicles { get; set;}
    }
}
