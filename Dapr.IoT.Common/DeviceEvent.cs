namespace Dapr.IoT.Common;
public record DeviceEvent
(
    Guid Id,
    DateTimeOffset TS,
    double Value,
    DeviceCoordinates Coordinates
);
