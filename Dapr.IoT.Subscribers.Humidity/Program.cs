var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddDapr(); // <====

var app = builder.Build();

app.UseRouting();
// app.UseCloudEvents();      // <==== NO SE NON VOGLIO UTILIZZARE IL FORMATO DEGLI EVENTI DI TIPO https://cloudevents.io/

app.UseEndpoints(endpoints =>
{
    endpoints.MapSubscribeHandler();
    endpoints.MapControllers();
});

app.Run();