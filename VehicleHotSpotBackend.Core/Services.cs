

namespace VehicleHotSpotBackend.Core
{
    public class Services
    {
        private InMemoryStorage? _inMemoryStorage;

        public InMemoryStorage InMemoryStorage => _inMemoryStorage ??= new InMemoryStorage();
    }
}
