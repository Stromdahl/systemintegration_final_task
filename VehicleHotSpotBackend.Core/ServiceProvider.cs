

namespace VehicleHotSpotBackend.Core
{
    public class ServiceProvider
    {
        private static Services _services;
        public static Services Current => _services ??= new Services();

    }
}
