using System.Text.Json;
using Dapr.IoT.Common;
using MQTTnet;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using MQTTnet.Client.Receiving;
using MQTTnet.Extensions.ManagedClient;
using Serilog;

namespace Dapr.Iot.Devices.Temperature;

class Program
{
    static async Task Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();

        MqttClientOptionsBuilder builder = new MqttClientOptionsBuilder()
                                    .WithClientId("Dapr.Iot.Devices.Temperature")
                                    .WithTcpServer("localhost", 707);

        ManagedMqttClientOptions options = new ManagedMqttClientOptionsBuilder()
                                .WithAutoReconnectDelay(TimeSpan.FromSeconds(60))
                                .WithClientOptions(builder.Build())
                                .Build();

        IManagedMqttClient _mqttClient = new MqttFactory().CreateManagedMqttClient();

        _mqttClient.ConnectedHandler = new MqttClientConnectedHandlerDelegate(OnConnected);
        _mqttClient.DisconnectedHandler = new MqttClientDisconnectedHandlerDelegate(OnDisconnected);
        _mqttClient.ConnectingFailedHandler = new ConnectingFailedHandlerDelegate(OnConnectingFailed);

        _mqttClient.ApplicationMessageReceivedHandler = new MqttApplicationMessageReceivedHandlerDelegate(a => {
            Log.Logger.Information("Message recieved: {payload}", a.ApplicationMessage);
        });

        await _mqttClient.StartAsync(options);

        Random rand = new Random(10);

        while (true)
        {
            string fromTemperatureSensor = JsonSerializer.Serialize(
                new DeviceEvent (
                    Id: Guid.Parse("8b8f4142-ac68-4478-bf1c-0ddfd95a5641"), 
                    TS: DateTimeOffset.UtcNow, 
                    Value: rand.Next(0, 1000) * 0.042,
                    Coordinates: new DeviceCoordinates(Latitude: Math.Round(45.647890 + Random.Shared.Next(0, 9) * 0.000001, 4), Longitude: Math.Round(10.264870 + Random.Shared.Next(0, 9) * 0.000001, 4))
                ));
            await _mqttClient.PublishAsync(nameof(Topics.temperature), fromTemperatureSensor);

            Task.Delay(1000).GetAwaiter().GetResult();
        }
    }

    public static void OnConnected(MqttClientConnectedEventArgs obj)
    {
        Log.Logger.Information("Successfully connected.");
    }

    public static void OnConnectingFailed(ManagedProcessFailedEventArgs obj)
    {
        Log.Logger.Warning("Couldn't connect to broker.");
    }

    public static void OnDisconnected(MqttClientDisconnectedEventArgs obj)
    {
        Log.Logger.Information("Successfully disconnected.");
    }
}
