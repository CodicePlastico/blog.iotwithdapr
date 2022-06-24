namespace Dapr.IoT.Subscribers.Temperature;

[ApiController]
public class TemperatureController : ControllerBase
{
    public const string StateStoreName = "mongo_state";
    public const string EventStoreStoreName = "mongo_eventstore";
    public const string PubSubState = "mqtt_state";
    public const string PubSubEventStore = "mqtt_eventstore";
    private readonly ILogger<TemperatureController> logger;

    public TemperatureController(ILogger<TemperatureController> logger)
    {
        this.logger = logger;
    }

    [Topic(PubSubEventStore, nameof(Topics.temperature))]
    [HttpPost("temperature_subscribe_eventstore")]
    public async Task<ActionResult> EventStore(DeviceEvent deviceEvent, [FromServices] DaprClient daprClient)
    {
        logger.LogInformation("Saving temperature device event..");
        await daprClient.SaveStateAsync(EventStoreStoreName, Guid.NewGuid().ToString(), deviceEvent);
        logger.LogInformation($"Temperature device event saved!");
        return new OkResult();
    }

    [Topic(PubSubState, nameof(Topics.temperature))]
    [HttpPost("temperature_subscribe_state")]
    public async Task<ActionResult> SaveState(DeviceEvent deviceEvent, [FromServices] DaprClient daprClient)
    {
        logger.LogInformation("Enter temperature device start");

        var state = await daprClient.GetStateEntryAsync<TemperatureModel>(StateStoreName, deviceEvent.Id.ToString());
        if (state.Value is null)
        {
            var location = await daprClient.InvokeMethodAsync<GeoLocationResponse>(HttpMethod.Get, "dapr-iot-geolocation", $"tolocation/{deviceEvent.Coordinates.Latitude}/{deviceEvent.Coordinates.Longitude}");
            state.Value = deviceEvent.ToModel(location.Location);
        }
        else
        {
            state.Value.TemperatureInC = deviceEvent.Value;
            state.Value.TemperatureInF = deviceEvent.Value.ToFarenheit();
            state.Value.TS = deviceEvent.TS.DateTime;
        }
        await state.SaveAsync();

        logger.LogInformation("State of temperature device {deviceEventId} updated!", deviceEvent.Id);
        return new OkResult();
    }

    [HttpGet("temperature/device/{id}")]
    public async Task<ActionResult<DeviceEvent>> Get([FromRoute]string id, [FromServices] DaprClient daprClient)
    {
        logger.LogInformation("Get state of temperature device with {id}.", id);
        var state = await daprClient.GetStateEntryAsync<TemperatureModel>(StateStoreName, id);
        return state.Value is null 
            ? NotFound()
            : Ok(state.Value);
    }
}