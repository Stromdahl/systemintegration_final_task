using VehicleHotSpotBackend.Web.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace VehicleHotSpotBackend.Web.Services
{
    public class DrivingDataService
    {
        private readonly IMongoCollection<DrivingDataItem> _drivingDataCollection;

        public DrivingDataService(
            IOptions<VehicleHotSpotDatabaseSettings> vehicleHotSpotDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                vehicleHotSpotDatabaseSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(
                vehicleHotSpotDatabaseSettings.Value.DataBaseName);

            _drivingDataCollection = mongoDatabase.GetCollection<DrivingDataItem>(
                vehicleHotSpotDatabaseSettings.Value.DrivingDataCollectionName);
        }

        public async Task<List<DrivingDataItem>> GetAsync() =>
        await _drivingDataCollection.Find(_ => true).ToListAsync();

        public async Task<DrivingDataItem?> GetAsync(string id) =>
            await _drivingDataCollection.Find(x => x.vin == id).FirstOrDefaultAsync();

        public async Task CreateAsync(DrivingDataItem newDrivingData) =>
            await _drivingDataCollection.InsertOneAsync(newDrivingData);

        public async Task UpdateAsync(string id, DrivingDataItem updatedDrivingData) =>
            await _drivingDataCollection.ReplaceOneAsync(x => x.vin == id, updatedDrivingData);

        public async Task RemoveAsync(string id) =>
            await _drivingDataCollection.DeleteOneAsync(x => x.vin == id);
    }
}
