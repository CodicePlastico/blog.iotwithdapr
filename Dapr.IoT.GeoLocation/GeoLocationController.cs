using Dapr.Client;
using Microsoft.AspNetCore.Mvc;

namespace Dapr.IoT.GeoLocation.Controllers;

[ApiController]
public class GeoLocationController : ControllerBase
{
    public const string Cache = "geolocation_cache";
    private readonly ILogger<GeoLocationController> logger;

    public GeoLocationController(ILogger<GeoLocationController> logger)
    {
        this.logger = logger;
    }
    
    [HttpGet("tolocation/{latitude}/{longitude}")]
    public async Task<IActionResult> Get([FromRoute] string latitude, 
                                         [FromRoute] string longitude, 
                                         [FromServices] DaprClient daprClient)
    {
        var key = $"{latitude}|{longitude}";
        var locationEntry = await daprClient.GetStateEntryAsync<string>(Cache, key);
        if (locationEntry?.Value is null)
        {
            logger.LogInformation($"The key for {latitude} and {longitude} is missing: location should be retrieved!");

            await daprClient.SaveStateAsync<string>(Cache, key, "Lumezzane");
            locationEntry = await daprClient.GetStateEntryAsync<string>(Cache, key);
        }
        else
        {
            logger.LogInformation($"The key for {latitude} and {longitude} is {locationEntry.Key}: object cached!");
        }

        return Ok(new { Location = locationEntry.Value });
    }
}
