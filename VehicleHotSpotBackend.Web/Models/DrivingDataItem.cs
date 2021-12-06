using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace VehicleHotSpotBackend.Web.Models
{
    public class DrivingDataItem
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("vin")]
        public string vin { get; set; }
        public int energyConsumption { get; set; }
        public int meanEnergyConsumption { get; set; }
        public string? startTime { get; set; }
        public string? stopTime { get; set; }
        public double distance { get; set; }
        public double meanSpeed { get; set; }

        public string? date { get; set; }
    }
}
