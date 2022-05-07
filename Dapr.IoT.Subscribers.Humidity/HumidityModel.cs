namespace Dapr.IoT.Subscribers.Humidity;

public class HumidityModel
{
    public string Id { get; set; }
    public double Humidity { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Location { get; set; }
    public DateTime TS { get; set; }
}
