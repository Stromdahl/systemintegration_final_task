namespace VehicleHotSpotBackend.Web.Models
{
    public class VehicleStatusItem: VehiclePositionItem
    {
        public int batteryPercentage { get; set; }
        public float milage { get; set; }
        public float[]? tirePressure { get; set; }
        public bool locked { get; set; }
        public bool alarmArmed { get; set; }
    }
}
