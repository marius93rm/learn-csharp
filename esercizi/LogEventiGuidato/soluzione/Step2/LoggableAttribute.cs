using System;

namespace LogGuidato.Step2;

/// <summary>
/// Attributo con descrizione per i metodi loggabili.
/// Negli step successivi useremo la descrizione per spiegare l'operazione al logger.
/// </summary>
[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
public sealed class LoggableAttribute : Attribute
{
    public LoggableAttribute(string descrizione)
    {
        Descrizione = descrizione;
    }

    /// <summary>
    /// Descrizione testuale dell'operazione loggata.
    /// </summary>
    public string Descrizione { get; }
}
