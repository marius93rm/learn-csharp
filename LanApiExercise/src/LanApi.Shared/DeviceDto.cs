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
