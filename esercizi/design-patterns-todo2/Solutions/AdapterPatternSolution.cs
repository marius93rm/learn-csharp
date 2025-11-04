using System.Collections.Generic;
using System.Linq;

namespace DesignPatternsTodo2.Solutions;

/// <summary>
/// Soluzione del pattern Adapter con gestione errori e adattatori multipli.
/// </summary>
public static class AdapterPatternSolution
{
    public static void Run()
    {
        IWeatherProvider provider = new ThirdPartyWeatherAdapter(new ThirdPartyWeatherService());
        Console.WriteLine(provider.GetCurrentTemperature("Roma"));

        IWeatherProvider xmlProvider = new XmlWeatherAdapter(new LegacyXmlWeatherService());
        Console.WriteLine(xmlProvider.GetCurrentTemperature("Milano"));

        var compositeProvider = new CompositeWeatherAdapter(new [] { provider, xmlProvider });
        Console.WriteLine(compositeProvider.GetCurrentTemperature("Torino"));
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
        if (legacyResult is null || string.IsNullOrWhiteSpace(legacyResult.City))
        {
            throw new InvalidOperationException("Risposta legacy non valida.");
        }

        return $"Temperatura attuale a {legacyResult.City}: {legacyResult.TemperatureCelsius:F1}°C ({legacyResult.ConditionCode})";
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

public sealed class LegacyXmlWeatherService
{
    public string GetWeatherReport(string city)
    {
        Console.WriteLine($"Recupero XML meteo per {city}");
        return $"<weather><city>{city}</city><temp>18.3</temp><condition>CLOUDY</condition></weather>";
    }
}

public sealed class XmlWeatherAdapter : IWeatherProvider
{
    private readonly LegacyXmlWeatherService _service;

    public XmlWeatherAdapter(LegacyXmlWeatherService service) => _service = service;

    public string GetCurrentTemperature(string city)
    {
        var xml = _service.GetWeatherReport(city);
        var parsed = ParseXml(xml);
        return $"Temperatura attuale a {parsed.City}: {parsed.TemperatureCelsius:F1}°C ({parsed.ConditionCode})";
    }

    private static LegacyWeatherResult ParseXml(string xml)
    {
        // Parser minimale per mantenere il focus sull'adattamento.
        var result = new LegacyWeatherResult();
        result.City = Extract(xml, "city") ?? "N/D";
        result.TemperatureCelsius = double.TryParse(Extract(xml, "temp"), out var value) ? value : 0;
        result.ConditionCode = Extract(xml, "condition") ?? "UNKNOWN";
        return result;
    }

    private static string? Extract(string xml, string tag)
    {
        var open = $"<{tag}>";
        var close = $"</{tag}>";
        var startIndex = xml.IndexOf(open, StringComparison.OrdinalIgnoreCase);
        var endIndex = xml.IndexOf(close, StringComparison.OrdinalIgnoreCase);
        if (startIndex < 0 || endIndex < 0 || endIndex <= startIndex)
        {
            return null;
        }

        startIndex += open.Length;
        return xml[startIndex..endIndex];
    }
}

public sealed class CompositeWeatherAdapter : IWeatherProvider
{
    private readonly IReadOnlyCollection<IWeatherProvider> _providers;

    public CompositeWeatherAdapter(IReadOnlyCollection<IWeatherProvider> providers)
    {
        _providers = providers;
    }

    public string GetCurrentTemperature(string city)
    {
        var results = _providers.Select(p => p.GetCurrentTemperature(city));
        return string.Join(" | ", results);
    }
}
