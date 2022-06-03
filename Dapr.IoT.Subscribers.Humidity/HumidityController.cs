using Dapr.Client;
using Dapr.IoT.Common;
using Microsoft.AspNetCore.Mvc;

namespace Dapr.IoT.Subscribers.Humidity;

[ApiController]
public class HumidityController : ControllerBase
{
    public const string StateStoreName = "mongo_state";
    public const string EventStoreStoreName = "mongo_eventstore";
    public const string PubSubState = "mqtt_state";
    public const string PubSubEventStore = "mqtt_eventstore";
    private readonly ILogger<HumidityController> logger;

    public HumidityController(ILogger<HumidityController> logger)
    {
        this.logger = logger;
    }

    [Topic(PubSubEventStore, nameof(Topics.humidity))]
    [HttpPost("humidity_subscribe_eventstore")]
    public async Task<ActionResult> EventStore(DeviceEvent deviceEvent, [FromServices] DaprClient daprClient)
    {
        logger.LogInformation("Saving humidity device event..");
        await daprClient.SaveStateAsync(EventStoreStoreName, Guid.NewGuid().ToString(), deviceEvent);
        logger.LogInformation($"Humidity device event saved!");
        return new OkResult();
    }

    [Topic(PubSubState, nameof(Topics.humidity))]
    [HttpPost("humidity_subscribe_state")]
    public async Task<ActionResult> SaveState(DeviceEvent deviceEvent, [FromServices] DaprClient daprClient)
    {
        logger.LogInformation("Enter humidity device start");

        var state = await daprClient.GetStateEntryAsync<HumidityModel>(StateStoreName, deviceEvent.Id.ToString());
        if (state.Value is null)
        {
            var location = await daprClient.InvokeMethodAsync<GeoLocationResponse>(HttpMethod.Get, "dapr-iot-geolocation", $"tolocation/{deviceEvent.Coordinates.Latitude}/{deviceEvent.Coordinates.Longitude}");
            state.Value = deviceEvent.ToModel(location.Location);
        }
        else
        {
            state.Value.Humidity = deviceEvent.Value;
            state.Value.TS = deviceEvent.TS.DateTime;
        }
        await state.SaveAsync();

        logger.LogInformation($"State of humidity device {deviceEvent.Id} updated!");
        return new OkResult();
    }

    [HttpGet("humidity/device/{id}")]
    public async Task<ActionResult<DeviceEvent>> Get([FromRoute] string id, [FromServices] DaprClient daprClient)
    {
        logger.LogInformation($"Get state of humidity device with {id}.");
        var state = await daprClient.GetStateEntryAsync<HumidityModel>(StateStoreName, id);
        return state.Value is null
            ? NotFound()
            : Ok(state.Value);
    }
}