using System;

namespace LogGuidato.Step2;

/// <summary>
/// Attributo con descrizione per i metodi loggabili.
/// </summary>
[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
public class LoggableAttribute : Attribute
{
    public LoggableAttribute(string descrizione)
    {
        Descrizione = descrizione;
    }

    public string Descrizione { get; }
}
