using LanApi.Server;
using LanApi.Shared;

var builder = WebApplication.CreateBuilder(args);

// Registriamo i servizi minimi necessari: repository in memoria e strumenti di documentazione.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IDeviceRepository, InMemoryDeviceRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // Swagger UI è utile per esplorare gli endpoint durante le prime milestone.
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Endpoint di ping utilizzato nella Milestone 1.
app.MapGet("/api/ping", () => Results.Ok(new
{
    message = "pong",
    utc = DateTime.UtcNow
}))
.WithName("Ping")
.WithOpenApi();

// Endpoint che restituisce tutti i device registrati.
app.MapGet("/api/devices", (IDeviceRepository repository) => Results.Ok(repository.GetAll()))
   .WithName("GetDevices")
   .WithOpenApi();

// Endpoint che restituisce un singolo device, con gestione dell'HTTP 404.
app.MapGet("/api/devices/{id:int}", (int id, IDeviceRepository repository) =>
{
    var device = repository.GetById(id);
    return device is not null
        ? Results.Ok(device)
        : Results.NotFound(new { message = $"Device con id {id} non trovato." });
})
.WithName("GetDeviceById")
.WithOpenApi();

// Endpoint per registrare un nuovo device.
app.MapPost("/api/devices", (DeviceRegistrationRequest request, IDeviceRepository repository) =>
{
    if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.IpAddress))
    {
        return Results.BadRequest(new { message = "Nome e indirizzo IP sono obbligatori." });
    }

    var created = repository.Add(request);
    return Results.Created($"/api/devices/{created.Id}", created);
})
.WithName("CreateDevice")
.WithOpenApi();

// Endpoint per aggiornare lo stato di un device esistente.
app.MapPut("/api/devices/{id:int}/status", (int id, DeviceStatusUpdate update, IDeviceRepository repository) =>
{
    var updated = repository.UpdateStatus(id, update.Status);
    return updated is not null
        ? Results.Ok(updated)
        : Results.NotFound(new { message = $"Device con id {id} non trovato." });
})
.WithName("UpdateDeviceStatus")
.WithOpenApi();

// TODO Milestone 6: valutare l'aggiunta di un endpoint DELETE per permettere agli studenti di sperimentare un ciclo CRUD completo.

app.Run();
