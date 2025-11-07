using System;

namespace LogGuidato.Step1;

/// <summary>
/// Attributo personalizzato per contrassegnare i metodi da includere nel log.
/// </summary>
[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
public class LoggableAttribute : Attribute
{
    // TODO: aggiungere eventuali proprietà opzionali (es. Descrizione) per arricchire il log.
}
