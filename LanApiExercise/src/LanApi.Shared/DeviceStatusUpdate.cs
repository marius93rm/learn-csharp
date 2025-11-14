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
