## Intro
I progetti 
- `Dapr.IoT.Subscribers.Humidity`
- `Dapr.IoT.Subscribers.Temperature`
- `Dapr.IoT.GeoLocation` 

contengono le cartelle `Components` con la configurazione 
- del database `MongoDB`
- della cache `Redis`
- della connessione al broker `MQTT`

## Comandi
- Aprire `.\Dapr.IoT.Broker`
- Eseguire il broker MQTT con il comando `dotnet run`

- Aprire `.\Dapr.Iot.Devices.Humidity`
- Eseguire il simulatore del sensore di umidità con il comando `dotnet run`

- Aprire `.\Dapr.Iot.Devices.Temperature`
- Eseguire il simulatore del sensore di temperatura con il comando `dotnet run`

- Aprire `.\Dapr.IoT.GeoLocation`
- Eseguire il comando `dapr run --app-id "dapr-iot-geolocation" --app-port 5002 --dapr-http-port 5012 --components-path "./components" dotnet run`

- Aprire `.\Dapr.IoT.Subscribers.Temperature`
- Eseguire il comando `dapr run --app-id "dapr-iot-temperature" --app-port 5000 --dapr-http-port 5010 --components-path "./components" dotnet run`

- Aprire `.\Dapr.IoT.Subscribers.Humidity`
- Eseguire il comando `dapr run --app-id "dapr-iot-humidity" --app-port 5001 --dapr-http-port 5011 --components-path "./components" dotnet run`

- Recupero stato humidity 
  `curl http://localhost:5011/v1.0/invoke/dapr-iot-humidity/method/humidity/device/8b8f4142-ac68-4478-bf1c-0ddfd95a5641`
  che in teoria non ci serve, perchè possiamo usare la get-by-id built-in => 
  `curl http://localhost:5011/v1.0/state/mongo_state/8b8f4142-ac68-4478-bf1c-0ddfd95a5641`
- Recupero stato temperature 
  `curl http://localhost:5010/v1.0/invoke/dapr-iot-temperature/method/temperature/device/8b8f4142-ac68-4478-bf1c-0ddfd95a5641`
  che in teoria non ci serve, perchè possiamo usare la get-by-id built-in =>  
  `curl http://localhost:5010/v1.0/state/mongo_state/8b8f4142-ac68-4478-bf1c-0ddfd95a5641`
  
## Dashboard
- Per consultare la dashboard di Dapr eseguire il comando `dapr dashboard` e aprire da browser l'indirizzo `http://localhost:8080`
- Per consultare la dashbaord di Zipkin aprire da browser l'indirizzo `http://localhost:9411`
