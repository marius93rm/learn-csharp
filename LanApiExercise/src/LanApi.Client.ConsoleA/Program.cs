using System.Net.Http.Json;
using LanApi.Shared;
using Microsoft.Extensions.Configuration;

// Lettura configurazione condivisa.
var configuration = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: true)
    .AddEnvironmentVariables(prefix: "LANAPI_")
    .Build();

var options = configuration.GetSection("Api").Get<ApiClientOptions>() ?? new ApiClientOptions();

using var httpClient = new HttpClient
{
    BaseAddress = new Uri(options.BaseUrl),
    Timeout = TimeSpan.FromSeconds(options.TimeoutSeconds)
};

Console.WriteLine("=== LanApi.Client.ConsoleA ===");
Console.WriteLine($"Server configurato: {httpClient.BaseAddress}");

await RegisterAndListAsync();

async Task RegisterAndListAsync()
{
    Console.WriteLine("\nRegistrazione di un nuovo device di prova...");
    var request = new DeviceRegistrationRequest
    {
        Name = "Client A - Laptop di prova",
        IpAddress = "192.168.1.150"
    };

    try
    {
        var response = await httpClient.PostAsJsonAsync("/api/devices", request);
        if (response.IsSuccessStatusCode)
        {
            var created = await response.Content.ReadFromJsonAsync<DeviceDto>();
            Console.WriteLine(created is not null
                ? $"Device registrato con id {created.Id} e stato iniziale {created.Status}."
                : "Registrazione completata ma nessun payload ricevuto.");
        }
        else
        {
            Console.WriteLine($"Registrazione fallita con codice {(int)response.StatusCode} {response.StatusCode}.");
            var errorContent = await response.Content.ReadAsStringAsync();
            if (!string.IsNullOrWhiteSpace(errorContent))
            {
                Console.WriteLine($"Dettagli errore: {errorContent}");
            }
        }
    }
    catch (HttpRequestException ex)
    {
        Console.WriteLine($"Errore di rete durante la registrazione: {ex.Message}");
        return;
    }

    Console.WriteLine("\nRecupero elenco device registrati...");
    var devices = await FetchDevicesAsync(httpClient);
    if (devices.Count == 0)
    {
        Console.WriteLine("Non sono stati restituiti device dal server.");
    }
    else
    {
        foreach (var device in devices)
        {
            Console.WriteLine($" - [{device.Id}] {device.Name} @ {device.IpAddress} => {device.Status}");
        }
    }
}

static async Task<IReadOnlyList<DeviceDto>> FetchDevicesAsync(HttpClient httpClient)
{
    try
    {
        var devices = await httpClient.GetFromJsonAsync<List<DeviceDto>>("/api/devices");
        // TODO Milestone 3: arricchisci questa funzione gestendo ad esempio l'ordinamento per nome o l'esclusione dei device offline.
        return devices ?? new List<DeviceDto>();
    }
    catch (HttpRequestException ex)
    {
        Console.WriteLine($"Errore nel recupero dei device: {ex.Message}");
        return Array.Empty<DeviceDto>();
    }
}
