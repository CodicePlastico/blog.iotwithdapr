namespace Dapr.IoT.Subscribers.Temperature;

public static class MappingExtensions
{
    public static TemperatureModel ToModel(this DeviceEvent deviceEvent, string location)
        => new()
        {
            Id = deviceEvent.Id.ToString(),
            TS = deviceEvent.TS.DateTime,
            Latitude = deviceEvent.Coordinates.Latitude,
            Longitude = deviceEvent.Coordinates.Longitude,
            Location = location,
            TemperatureInC = deviceEvent.Value,
            TemperatureInF = deviceEvent.Value.ToFarenheit()
        };
    public static double ToFarenheit(this double celsius)
        => (celsius * 9 / 5) + 32;
}
