## Intro
I progetti `DaprMqtt.Gates` e `DaprMqtt.Motors` contengono le cartelle `Components` con la configurazione del database `mongo`

## Commands
- Aprire `.\Dapr.IoT.Broker`
- Eseguire il comando `dotnet run`

- Aprire `.\Dapr.Iot.Devices.Humidity`
- Eseguire il comando `dotnet run`

- Aprire `.\Dapr.Iot.Devices.Temperature`
- Eseguire il comando `dotnet run`

- Aprire `.\Dapr.IoT.GeoLocation`
- Eseguire il comando `dapr run --app-id "dapr-iot-geolocation" --app-port 5002 --dapr-http-port 5012 --components-path "./components" dotnet run`

- Aprire `.\Dapr.IoT.Subscribers.Temperature`
- Eseguire il comando `dapr run --app-id "dapr-iot-temperature" --app-port 5000 --dapr-http-port 5010 --components-path "./components" dotnet run`

- Aprire `.\Dapr.IoT.Subscribers.Humidity`
- Eseguire il comando `dapr run --app-id "dapr-iot-humidity" --app-port 5001 --dapr-http-port 5011 --components-path "./components" dotnet run`

- Recupero stato gate `curl http://localhost:5010/v1.0/invoke/dapr-iot-temperature/method/gate/8b8f4142-ac68-4478-bf1c-0ddfd95a5641`
  che in teoria non ci serve, perchè possiamo usare la get-by-id built-in => `curl http://localhost:5010/v1.0/state/mongo_state/8b8f4142-ac68-4478-bf1c-0ddfd95a5641`
- Recupero stato gate `curl http://localhost:5010/v1.0/invoke/dapr-iot-humidity/method/motor/8b8f4142-ac68-4478-bf1c-0ddfd95a5641`
  che in teoria non ci serve, perchè possiamo usare la get-by-id built-in =>  `curl http://localhost:5011/v1.0/state/mongo_state/8b8f4142-ac68-4478-bf1c-0ddfd95a5641`

\\
## Tye
- `tye run`
- `http://localhost:8000`
- `tye init`
- Aggiunto blocco 
  ```
  extensions:
    - name: dapr
  ```           docker run -d --hostname my-rabbit --name some-rabbit rabbitmq:3
docker exec some-rabbit rabbitmq-plugins enable rabbitmq_management