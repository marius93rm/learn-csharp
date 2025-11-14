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
