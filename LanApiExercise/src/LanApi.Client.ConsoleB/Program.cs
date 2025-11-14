using System.Net.Http.Json;
using LanApi.Shared;
using Microsoft.Extensions.Configuration;

var configuration = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: true)
    .AddEnvironmentVariables(prefix: "LANAPI_")
    .Build();

var options = configuration.GetSection("Api").Get<ApiClientOptions>() ?? new ApiClientOptions();
var pollSeconds = configuration.GetSection("Monitoring").GetValue("PollIntervalSeconds", 5);
var iterations = configuration.GetSection("Monitoring").GetValue("InitialIterations", 3);

using var httpClient = new HttpClient
{
    BaseAddress = new Uri(options.BaseUrl),
    Timeout = TimeSpan.FromSeconds(options.TimeoutSeconds)
};

Console.WriteLine("=== LanApi.Client.ConsoleB ===");
Console.WriteLine($"Modalità monitoraggio, interrogazione ogni {pollSeconds} secondi per {iterations} iterazioni...");

await RunMonitoringAsync(httpClient, pollSeconds, iterations);
await TryUpdateDeviceStatusAsync(httpClient);

static async Task RunMonitoringAsync(HttpClient httpClient, int pollSeconds, int iterations)
{
    for (var i = 1; i <= iterations; i++)
    {
        Console.WriteLine($"\n--- Iterazione #{i} ---");
        try
        {
            var devices = await httpClient.GetFromJsonAsync<List<DeviceDto>>("/api/devices") ?? new List<DeviceDto>();
            if (devices.Count == 0)
            {
                Console.WriteLine("Nessun device registrato al momento.");
            }
            else
            {
                foreach (var device in devices)
                {
                    var lastSeen = device.LastSeenUtc?.ToLocalTime().ToString("G") ?? "mai";
                    Console.WriteLine($"[{device.Id}] {device.Name} => {device.Status} (ultima attività: {lastSeen})");
                }
            }
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Il server non è raggiungibile: {ex.Message}");
        }

        if (i < iterations)
        {
            await Task.Delay(TimeSpan.FromSeconds(pollSeconds));
        }
    }

    // TODO Milestone 5: trasformare il monitoraggio in un loop infinito gestendo la cancellazione via input tastiera o CTRL+C.
}

static async Task TryUpdateDeviceStatusAsync(HttpClient httpClient)
{
    Console.WriteLine("\nAggiornamento di stato di esempio (device 1 => Online)...");
    var update = new DeviceStatusUpdate { Status = DeviceStatus.Online };

    try
    {
        var response = await httpClient.PutAsJsonAsync("/api/devices/1/status", update);
        if (response.IsSuccessStatusCode)
        {
            var device = await response.Content.ReadFromJsonAsync<DeviceDto>();
            Console.WriteLine(device is not null
                ? $"Device aggiornato: {device.Name} ora è {device.Status}."
                : "Aggiornamento completato ma risposta vuota.");
        }
        else
        {
            Console.WriteLine($"Aggiornamento non riuscito (HTTP {(int)response.StatusCode}).");
        }
    }
    catch (HttpRequestException ex)
    {
        Console.WriteLine($"Errore di rete durante l'aggiornamento: {ex.Message}");
    }

    // TODO Milestone 5: chiedi all'utente quale device aggiornare e quale stato impostare, gestendo eventuali errori 404.
    // TODO Milestone 5: sperimenta il supporto ai timeout personalizzati leggendo un valore aggiuntivo da configurazione.
}
