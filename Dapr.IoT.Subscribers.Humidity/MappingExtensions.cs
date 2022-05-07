using Dapr.IoT.Common;

namespace Dapr.IoT.Subscribers.Humidity;

public static class MappingExtensions
{
    public static HumidityModel ToModel(this DeviceEvent deviceEvent, string location)
        => new()
        {
            Id = deviceEvent.Id.ToString(),
            TS = deviceEvent.TS.DateTime,
            Latitude = deviceEvent.Coordinates.Latitude,
            Longitude = deviceEvent.Coordinates.Longitude,
            Location = location,
            Humidity = deviceEvent.Value
        };
}
