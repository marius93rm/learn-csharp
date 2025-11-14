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
