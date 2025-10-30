/*
 * Pattern: Adapter
 * Obiettivi didattici:
 *   - Adattare un'interfaccia esistente a quella attesa dal client.
 *   - Riutilizzare componenti legacy senza modificarne il codice interno.
 *   - Comprendere la differenza tra adapter oggetto e classe.
 * Istruzioni:
 *   - Completa i TODO per integrare nuove fonti di dati nel formato richiesto.
 */

namespace DesignPatternsTodo2.Patterns;

public static class AdapterPattern
{
    public static void Run()
    {
        IWeatherProvider provider = new ThirdPartyWeatherAdapter(new ThirdPartyWeatherService());
        Console.WriteLine(provider.GetCurrentTemperature("Roma"));

        // TODO: collega un nuovo servizio legacy implementando un ulteriore adapter.
    }
}

public interface IWeatherProvider
{
    string GetCurrentTemperature(string city);
}

public sealed class ThirdPartyWeatherAdapter : IWeatherProvider
{
    private readonly ThirdPartyWeatherService _service;

    public ThirdPartyWeatherAdapter(ThirdPartyWeatherService service) => _service = service;

    public string GetCurrentTemperature(string city)
    {
        var legacyResult = _service.FetchTemperature(city);
        // TODO: gestisci eventuali errori o dati mancanti nel risultato legacy.
        return $"Temperatura attuale a {legacyResult.City}: {legacyResult.TemperatureCelsius}°C";
    }
}

public sealed class ThirdPartyWeatherService
{
    public LegacyWeatherResult FetchTemperature(string city)
    {
        Console.WriteLine($"Chiamata API legacy per {city}");
        return new LegacyWeatherResult
        {
            City = city,
            TemperatureCelsius = 21.5,
            ConditionCode = "SUNNY"
        };
    }
}

public sealed class LegacyWeatherResult
{
    public string City { get; set; } = string.Empty;
    public double TemperatureCelsius { get; set; }
    public string ConditionCode { get; set; } = string.Empty;
}

// TODO: crea un adapter bidirezionale o un adapter di collezione per aggregare più risultati legacy.
