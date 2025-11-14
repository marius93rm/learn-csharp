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
