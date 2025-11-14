# Esercizio guidato: Server Web API C# e client LAN

## 1. Introduzione
Benvenuto in **LanApiExercise**, un percorso guidato per imparare a creare un piccolo ecosistema composto da un **server ASP.NET Core Minimal API** e da **due client console C#** che dialogano con il server tramite HTTP all'interno della stessa rete locale (LAN). L'obiettivo è didattico: ogni milestone introduce nuovi concetti, con spiegazioni teoriche, esempi pratici e TODO pensati per stimolare l'apprendimento attivo.

Al termine dell'esercizio avrai preso confidenza con:

- la creazione di un progetto Web API in .NET 8;
- la configurazione delle porte e degli URL di ascolto di Kestrel per lavorare sia su `localhost` sia su IP di rete locale;
- l'utilizzo di `HttpClient` nei client console per consumare endpoint REST;
- la serializzazione JSON con `System.Text.Json` e la gestione di codici di stato HTTP comuni;
- l'organizzazione del lavoro in milestone progressive con attività di refactoring e TODO mirati.

## 2. Prerequisiti
Per seguire il percorso ti serviranno:

- **.NET SDK 8.0** (o versione successiva) installato sulla tua macchina. Verifica con `dotnet --version`.
- Un editor di testo o IDE a tua scelta (Visual Studio Code, Visual Studio, JetBrains Rider o simili).
- Familiarità di base con C# e concetti HTTP (metodi, URL, codici di stato).
- Conoscenza essenziale dei comandi `dotnet`:
  - `dotnet build` per compilare la solution;
  - `dotnet run --project <percorso>` per eseguire server o client;
  - `dotnet test` non viene usato direttamente in questo esercizio.

## 3. Panoramica architetturale
Il progetto simula un **monitoraggio di device in LAN**. Il server espone API REST per registrare, elencare e aggiornare lo stato dei device. Due client console fungono da esempi pratici:

- `LanApi.Client.ConsoleA`: registra un device e stampa la lista.
- `LanApi.Client.ConsoleB`: esegue polling periodico e prova ad aggiornare lo stato di un device.

Gli elementi chiave:

- **HTTP**: protocollo usato dai client per parlare col server.
- **Endpoint REST**: URL come `/api/devices` che rappresentano risorse.
- **JSON**: formato di scambio dati, gestito tramite `System.Text.Json`.
- **localhost vs IP locale**: `localhost` punta alla macchina stessa; l'IP locale (es. `192.168.1.10`) è necessario per consentire l'accesso da altri dispositivi nella LAN.
- **Porta**: numero (es. `5087`) che identifica l'endpoint su cui il server è in ascolto.

## 4. Struttura della solution
```
LanApiExercise.sln
src/
  LanApi.Server/
  LanApi.Client.ConsoleA/
  LanApi.Client.ConsoleB/
  LanApi.Shared/
```

- **LanApi.Server**: server Minimal API. Qui trovi gli endpoint REST, la configurazione Kestrel e un repository in memoria.
- **LanApi.Client.ConsoleA**: client console che mostra come registrare un device e leggere la lista.
- **LanApi.Client.ConsoleB**: client console che simula un pannello di monitoraggio con polling periodico e aggiornamenti di stato.
- **LanApi.Shared**: libreria condivisa con DTO, enum e opzioni comuni.

Abbiamo scelto **Minimal API** perché riduce il codice cerimoniale, è perfetta per un esempio didattico e permette di concentrarsi subito sugli endpoint.

## 5. Milestone step-by-step
### Milestone 1 – Server minimo in locale
**Obiettivo**: avviare un server Web API minimale raggiungibile su `localhost`.

File principali:
- `src/LanApi.Server/Program.cs`
- `src/LanApi.Server/appsettings.json`

Esempio di codice (estratto da `Program.cs`):
```csharp
app.MapGet("/api/ping", () => Results.Ok(new
{
    message = "pong",
    utc = DateTime.UtcNow
}))
.WithName("Ping")
.WithOpenApi();
```

Comandi utili:
```bash
dotnet run --project src/LanApi.Server
```
Visita `http://localhost:5087/api/ping` da browser o via `curl`:
```bash
curl http://localhost:5087/api/ping
```
Risposta attesa:
```json
{ "message": "pong", "utc": "2024-01-01T12:34:56.789Z" }
```

### Milestone 2 – API di dominio base
**Obiettivo**: introdurre il dominio del monitoraggio device con DTO condivisi e operazioni CRUD minime.

File coinvolti:
- `src/LanApi.Shared/DeviceDto.cs`
- `src/LanApi.Shared/DeviceStatus.cs`
- `src/LanApi.Shared/DeviceRegistrationRequest.cs`
- `src/LanApi.Shared/DeviceStatusUpdate.cs`
- `src/LanApi.Server/InMemoryDeviceRepository.cs`
- `src/LanApi.Server/Program.cs`

Gli endpoint principali:
- `GET /api/devices`
- `GET /api/devices/{id}`
- `POST /api/devices`
- `PUT /api/devices/{id}/status`

Esempio di `POST` via `curl`:
```bash
curl -X POST http://localhost:5087/api/devices \
  -H "Content-Type: application/json" \
  -d '{"name":"Stampante","ipAddress":"192.168.1.50"}'
```

Codici HTTP gestiti:
- **200 OK** per letture e aggiornamenti.
- **201 Created** per la creazione di un device.
- **400 Bad Request** quando i dati non sono validi.
- **404 Not Found** quando l'id non esiste.

### Milestone 3 – Client console base
**Obiettivo**: creare il primo client console (`LanApi.Client.ConsoleA`).

File chiave:
- `src/LanApi.Client.ConsoleA/Program.cs`
- `src/LanApi.Client.ConsoleA/appsettings.json`

Estratto importante:
```csharp
var configuration = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: true)
    .AddEnvironmentVariables(prefix: "LANAPI_")
    .Build();
```

Il client registra un device di prova e poi legge l'elenco.

**TODO**:
```csharp
// TODO Milestone 3: arricchisci questa funzione gestendo ad esempio l'ordinamento per nome o l'esclusione dei device offline.
```
Il codice funziona già, ma lo studente può migliorare la funzione `FetchDevicesAsync` per ordinare o filtrare i risultati.

Comandi:
```bash
dotnet run --project src/LanApi.Client.ConsoleA
```
Assicurati che il server sia in esecuzione prima di avviare il client.

### Milestone 4 – Server raggiungibile in LAN
**Obiettivo**: configurare il server per essere raggiungibile da altri PC nella stessa rete.

Concetti:
- Differenza tra `localhost` e l'indirizzo IP della macchina (es. `192.168.1.10`).
- Binding di Kestrel tramite `appsettings.json` o variabili d'ambiente.
- Firewall: assicurati che la porta (es. `5087`) sia aperta per connessioni in ingresso.

Passi pratici:
1. Trova il tuo IP locale (Windows: `ipconfig`, macOS/Linux: `ifconfig` o `ip addr`).
2. Modifica `appsettings.json` del server:
   ```json
   "Kestrel": {
     "Endpoints": {
       "Http": {
         "Url": "http://0.0.0.0:5087"
       }
     }
   }
   ```
   In questo modo il server ascolta su tutte le interfacce di rete.
3. Avvia il server e dal client di un altro PC richiama `http://IP_TUO_PC:5087/api/ping`.
4. Aggiorna i client console cambiando `BaseUrl` in `appsettings.json`:
   ```json
   "BaseUrl": "http://192.168.1.10:5087"
   ```

### Milestone 5 – Secondo client e funzionalità avanzate
**Obiettivo**: esplorare scenari più dinamici con `LanApi.Client.ConsoleB`.

File coinvolti:
- `src/LanApi.Client.ConsoleB/Program.cs`
- `src/LanApi.Client.ConsoleB/appsettings.json`

Funzioni principali:
- `RunMonitoringAsync`: esegue un numero configurabile di iterazioni di polling.
- `TryUpdateDeviceStatusAsync`: aggiorna lo stato del device con id `1`.

TODO presenti nel codice:
```csharp
// TODO Milestone 5: trasformare il monitoraggio in un loop infinito gestendo la cancellazione via input tastiera o CTRL+C.
// TODO Milestone 5: chiedi all'utente quale device aggiornare e quale stato impostare, gestendo eventuali errori 404.
// TODO Milestone 5: sperimenta il supporto ai timeout personalizzati leggendo un valore aggiuntivo da configurazione.
```

Questi TODO offrono spunti per rendere il client più interattivo e robusto. Il codice attuale funziona già eseguendo un numero finito di iterazioni, evitando loop infiniti.

Comando di esecuzione:
```bash
dotnet run --project src/LanApi.Client.ConsoleB
```

### Milestone 6 – Refactoring e miglioramenti (opzionale)
**Obiettivo**: proporre ulteriori estensioni.

Suggerimenti:
- Creare un progetto `LanApi.Shared` (già fornito) come base per estendere i DTO.
- Migliorare la configurazione introducendo `appsettings.Development.json` personalizzati.
- Spostare la logica HTTP client in una classe dedicata (es. `ApiClientService`).
- Implementare un endpoint `DELETE /api/devices/{id}` lato server (vedi TODO in `Program.cs`).
- Aggiungere logging personalizzato lato server e lato client.

## 6. Configurazione per la rete locale
1. **Trovare l'IP**: su Windows usa `ipconfig`, su Linux/Mac `ifconfig` o `ip addr`. Cerca l'indirizzo IPv4 della scheda di rete in uso (es. `192.168.x.x`).
2. **Aggiornare Kestrel**: imposta `"Url": "http://0.0.0.0:5087"` in `appsettings.json` del server oppure usa la variabile d'ambiente `ASPNETCORE_URLS`.
3. **Firewall**: assicurati che la porta 5087 sia aperta per connessioni in ingresso (impostazione manuale, non gestita dal codice).
4. **Aggiornare i client**: modifica `appsettings.json` dei client o imposta l'ambiente `LANAPI_Api__BaseUrl`.
5. **Test**: da un altro PC nella LAN esegui `curl http://IP_SERVER:5087/api/ping` per verificare la connettività.

## 7. Come affrontare i TODO
- Procedi milestone dopo milestone, assicurandoti che tutto compili (`dotnet build`).
- Per ogni TODO leggi il commento: indica il contesto e la milestone di riferimento.
- Implementa una modifica alla volta e testa immediatamente il risultato.
- Verifica gli endpoint con `curl` o con gli stessi client console.
- Esempio di check per la Milestone 3: avvia il server, esegui `LanApi.Client.ConsoleA`, verifica che compaiano i device e che il TODO sia stato completato (ad esempio ordinamento alfabetico).

## 8. Possibili estensioni
- Aggiungere autenticazione fittizia (API key in header).
- Integrare logging strutturato con `Serilog` (solo come esercizio opzionale).
- Creare una semplice UI web o WPF che sfrutta le stesse API.
- Implementare un servizio di background nel server che simula il ping reale dei device e aggiorna `LastSeenUtc`.
- Introdurre test automatici con `xUnit` per il repository in memoria.

## 9. File e codice di riferimento
Di seguito trovi tutti i file principali con il relativo contenuto, pronti all'uso.

---

// File: LanApiExercise.sln
```
Microsoft Visual Studio Solution File, Format Version 12.00
# Visual Studio Version 17
VisualStudioVersion = 17.8.34330.188
MinimumVisualStudioVersion = 10.0.40219.1
Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "LanApi.Server", "src\LanApi.Server\LanApi.Server.csproj", "{17F30D83-BE26-48D3-8B0A-90EA591D82FC}"
EndProject
Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "LanApi.Client.ConsoleA", "src\LanApi.Client.ConsoleA\LanApi.Client.ConsoleA.csproj", "{4F272095-9B90-44A5-8916-7B6D192B6E5C}"
EndProject
Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "LanApi.Client.ConsoleB", "src\LanApi.Client.ConsoleB\LanApi.Client.ConsoleB.csproj", "{323F3E81-7E41-445E-9AC7-957C3F4A44A7}"
EndProject
Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "LanApi.Shared", "src\LanApi.Shared\LanApi.Shared.csproj", "{94E8D510-0D11-4906-AA7B-F5F12D1937F6}"
EndProject
Global
    GlobalSection(SolutionConfigurationPlatforms) = preSolution
        Debug|Any CPU = Debug|Any CPU
        Release|Any CPU = Release|Any CPU
    EndGlobalSection
    GlobalSection(ProjectConfigurationPlatforms) = postSolution
        {17F30D83-BE26-48D3-8B0A-90EA591D82FC}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
        {17F30D83-BE26-48D3-8B0A-90EA591D82FC}.Debug|Any CPU.Build.0 = Debug|Any CPU
        {17F30D83-BE26-48D3-8B0A-90EA591D82FC}.Release|Any CPU.ActiveCfg = Release|Any CPU
        {17F30D83-BE26-48D3-8B0A-90EA591D82FC}.Release|Any CPU.Build.0 = Release|Any CPU
        {4F272095-9B90-44A5-8916-7B6D192B6E5C}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
        {4F272095-9B90-44A5-8916-7B6D192B6E5C}.Debug|Any CPU.Build.0 = Debug|Any CPU
        {4F272095-9B90-44A5-8916-7B6D192B6E5C}.Release|Any CPU.ActiveCfg = Release|Any CPU
        {4F272095-9B90-44A5-8916-7B6D192B6E5C}.Release|Any CPU.Build.0 = Release|Any CPU
        {323F3E81-7E41-445E-9AC7-957C3F4A44A7}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
        {323F3E81-7E41-445E-9AC7-957C3F4A44A7}.Debug|Any CPU.Build.0 = Debug|Any CPU
        {323F3E81-7E41-445E-9AC7-957C3F4A44A7}.Release|Any CPU.ActiveCfg = Release|Any CPU
        {323F3E81-7E41-445E-9AC7-957C3F4A44A7}.Release|Any CPU.Build.0 = Release|Any CPU
        {94E8D510-0D11-4906-AA7B-F5F12D1937F6}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
        {94E8D510-0D11-4906-AA7B-F5F12D1937F6}.Debug|Any CPU.Build.0 = Debug|Any CPU
        {94E8D510-0D11-4906-AA7B-F5F12D1937F6}.Release|Any CPU.ActiveCfg = Release|Any CPU
        {94E8D510-0D11-4906-AA7B-F5F12D1937F6}.Release|Any CPU.Build.0 = Release|Any CPU
    EndGlobalSection
    GlobalSection(SolutionProperties) = preSolution
        HideSolutionNode = FALSE
    EndGlobalSection
EndGlobal
```

// File: src/LanApi.Server/LanApi.Server.csproj
```xml
<Project Sdk="Microsoft.NET.Sdk.Web">
  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>
  <ItemGroup>
    <ProjectReference Include="..\LanApi.Shared\LanApi.Shared.csproj" />
  </ItemGroup>
</Project>
```

// File: src/LanApi.Server/Program.cs
```csharp
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
```

// File: src/LanApi.Server/IDeviceRepository.cs
```csharp
using LanApi.Shared;

namespace LanApi.Server;

/// <summary>
/// Contratto semplice che incapsula le operazioni del dominio di monitoraggio device.
/// </summary>
public interface IDeviceRepository
{
    IEnumerable<DeviceDto> GetAll();

    DeviceDto? GetById(int id);

    DeviceDto Add(DeviceRegistrationRequest request);

    DeviceDto? UpdateStatus(int id, DeviceStatus status);
}
```

// File: src/LanApi.Server/InMemoryDeviceRepository.cs
```csharp
using System.Collections.Concurrent;
using LanApi.Shared;

namespace LanApi.Server;

/// <summary>
/// Implementazione in memoria pensata per la didattica: non usa un database reale ma tiene tutto in RAM.
/// </summary>
public class InMemoryDeviceRepository : IDeviceRepository
{
    private readonly ConcurrentDictionary<int, DeviceDto> _storage = new();
    private int _currentId;

    public InMemoryDeviceRepository()
    {
        // Inseriamo un paio di device di esempio per rendere l'esercizio immediatamente esplorabile.
        Add(new DeviceRegistrationRequest { Name = "Sensore Temperatura", IpAddress = "192.168.1.21" });
        Add(new DeviceRegistrationRequest { Name = "Access Point Ufficio", IpAddress = "192.168.1.3" });
    }

    public DeviceDto Add(DeviceRegistrationRequest request)
    {
        var id = Interlocked.Increment(ref _currentId);
        var device = new DeviceDto
        {
            Id = id,
            Name = request.Name.Trim(),
            IpAddress = request.IpAddress.Trim(),
            Status = DeviceStatus.Unknown,
            LastSeenUtc = null
        };

        _storage[id] = device;
        return device;
    }

    public IEnumerable<DeviceDto> GetAll()
    {
        return _storage.Values
            .OrderBy(d => d.Id)
            .ToList();
    }

    public DeviceDto? GetById(int id)
    {
        return _storage.TryGetValue(id, out var device) ? device : null;
    }

    public DeviceDto? UpdateStatus(int id, DeviceStatus status)
    {
        if (!_storage.TryGetValue(id, out var existing))
        {
            return null;
        }

        var updated = existing with
        {
            Status = status,
            LastSeenUtc = DateTime.UtcNow
        };

        _storage[id] = updated;
        return updated;
    }
}
```

// File: src/LanApi.Server/appsettings.json
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "Kestrel": {
    "Endpoints": {
      "Http": {
        "Url": "http://localhost:5087"
      }
    }
  }
}
```

// File: src/LanApi.Server/Properties/launchSettings.json
```json
{
  "$schema": "http://json.schemastore.org/launchsettings.json",
  "profiles": {
    "LanApi.Server": {
      "commandName": "Project",
      "dotnetRunMessages": true,
      "launchBrowser": false,
      "applicationUrl": "http://localhost:5087",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    }
  }
}
```

// File: src/LanApi.Shared/LanApi.Shared.csproj
```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>
</Project>
```

// File: src/LanApi.Shared/DeviceStatus.cs
```csharp
namespace LanApi.Shared;

/// <summary>
/// Rappresenta lo stato logico di un device monitorato nella rete locale.
/// </summary>
public enum DeviceStatus
{
    /// <summary>
    /// Il device è stato appena registrato ma non è ancora stato contattato.
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// Il device è raggiungibile e risponde alle richieste di monitoraggio.
    /// </summary>
    Online = 1,

    /// <summary>
    /// Il device non è attualmente raggiungibile.
    /// </summary>
    Offline = 2,

    /// <summary>
    /// Il device è in manutenzione e non deve essere contattato.
    /// </summary>
    Maintenance = 3
}
```

// File: src/LanApi.Shared/DeviceDto.cs
```csharp
using System.Text.Json.Serialization;

namespace LanApi.Shared;

/// <summary>
/// Contratto condiviso per rappresentare un device monitorato dal server.
/// </summary>
public record DeviceDto
{
    /// <summary>
    /// Identificatore numerico progressivo assegnato dal server.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Nome descrittivo assegnato al device dall'utente.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Indirizzo IP o hostname locale che identifica il device nella rete.
    /// </summary>
    public required string IpAddress { get; init; }

    /// <summary>
    /// Stato corrente del device.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public DeviceStatus Status { get; init; }

    /// <summary>
    /// Informazione opzionale sull'ultimo contatto registrato dal server.
    /// </summary>
    public DateTime? LastSeenUtc { get; init; }
}
```

// File: src/LanApi.Shared/DeviceRegistrationRequest.cs
```csharp
using System.ComponentModel.DataAnnotations;

namespace LanApi.Shared;

/// <summary>
/// Input condiviso per la registrazione di un nuovo device verso il server.
/// </summary>
public record DeviceRegistrationRequest
{
    /// <summary>
    /// Nome leggibile del device da monitorare.
    /// </summary>
    [Required]
    public required string Name { get; init; }

    /// <summary>
    /// Indirizzo IP (o hostname) che identifica il device nella rete locale.
    /// </summary>
    [Required]
    [RegularExpression(@"^[a-zA-Z0-9_.:-]+$")]
    public required string IpAddress { get; init; }
}
```

// File: src/LanApi.Shared/DeviceStatusUpdate.cs
```csharp
using System.Text.Json.Serialization;

namespace LanApi.Shared;

/// <summary>
/// Contratto utilizzato per aggiornare lo stato di un device esistente.
/// </summary>
public record DeviceStatusUpdate
{
    /// <summary>
    /// Nuovo stato richiesto per il device.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public DeviceStatus Status { get; init; }
}
```

// File: src/LanApi.Shared/ApiClientOptions.cs
```csharp
namespace LanApi.Shared;

/// <summary>
/// Opzioni di configurazione condivise per i client console che consumano il server LAN.
/// </summary>
public class ApiClientOptions
{
    /// <summary>
    /// URL base del server Web API (es. http://localhost:5087).
    /// </summary>
    public string BaseUrl { get; set; } = "http://localhost:5087";

    /// <summary>
    /// Timeout espresso in secondi per le chiamate HTTP.
    /// </summary>
    public int TimeoutSeconds { get; set; } = 10;
}
```

// File: src/LanApi.Client.ConsoleA/LanApi.Client.ConsoleA.csproj
```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net8.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="Microsoft.Extensions.Configuration" Version="8.0.0" />
    <PackageReference Include="Microsoft.Extensions.Configuration.Binder" Version="8.0.0" />
    <PackageReference Include="Microsoft.Extensions.Configuration.Json" Version="8.0.0" />
  </ItemGroup>
  <ItemGroup>
    <ProjectReference Include="..\LanApi.Shared\LanApi.Shared.csproj" />
  </ItemGroup>
  <ItemGroup>
    <None Update="appsettings.json">
      <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
    </None>
  </ItemGroup>
</Project>
```

// File: src/LanApi.Client.ConsoleA/appsettings.json
```json
{
  "Api": {
    "BaseUrl": "http://localhost:5087",
    "TimeoutSeconds": 10
  }
}
```

// File: src/LanApi.Client.ConsoleA/Program.cs
```csharp
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
```

// File: src/LanApi.Client.ConsoleB/LanApi.Client.ConsoleB.csproj
```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net8.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="Microsoft.Extensions.Configuration" Version="8.0.0" />
    <PackageReference Include="Microsoft.Extensions.Configuration.Binder" Version="8.0.0" />
    <PackageReference Include="Microsoft.Extensions.Configuration.Json" Version="8.0.0" />
  </ItemGroup>
  <ItemGroup>
    <ProjectReference Include="..\LanApi.Shared\LanApi.Shared.csproj" />
  </ItemGroup>
  <ItemGroup>
    <None Update="appsettings.json">
      <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
    </None>
  </ItemGroup>
</Project>
```

// File: src/LanApi.Client.ConsoleB/appsettings.json
```json
{
  "Api": {
    "BaseUrl": "http://localhost:5087",
    "TimeoutSeconds": 10
  },
  "Monitoring": {
    "PollIntervalSeconds": 5,
    "InitialIterations": 3
  }
}
```

// File: src/LanApi.Client.ConsoleB/Program.cs
```csharp
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
```
