var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddDapr(); // <====
builder.Services.AddEndpointsApiExplorer();  // <====

var app = builder.Build();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();