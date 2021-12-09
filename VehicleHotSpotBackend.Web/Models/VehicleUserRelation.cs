namespace VehicleHotSpotBackend.Web.Models
{
    public class VehicleUserRelation
    {
        public Guid customerId { get; set; }
        public string? vin { get; set; }
        public string? regNo { get; set; }
    }
}
