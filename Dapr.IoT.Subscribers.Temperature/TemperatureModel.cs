namespace Dapr.IoT.Subscribers.Temperature;

public class TemperatureModel
{
    public string Id { get; set; }
    public double TemperatureInC { get; set; }
    public double TemperatureInF { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Location { get; set; }
    public DateTime TS { get; set; }
}
