using System;

namespace LogGuidato.Step1;

/// <summary>
/// Attributo personalizzato per contrassegnare i metodi da includere nel log.
/// L'esercizio originale proponeva di aggiungere una proprietà opzionale:
/// qui scegliamo di esporre <see cref="Descrizione"/> così da spiegare il
/// contesto dell'operazione quando l'attributo verrà letto via reflection.
/// </summary>
[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
public sealed class LoggableAttribute : Attribute
{
    /// <summary>
    /// In fase di decorazione possiamo specificare una breve descrizione.
    /// Il parametro è opzionale per non appesantire i metodi più semplici.
    /// </summary>
    /// <param name="descrizione">Testo libero riportato nel log.</param>
    public LoggableAttribute(string descrizione = "")
    {
        Descrizione = descrizione;
    }

    /// <summary>
    /// Testo da stampare prima dell'esecuzione del metodo loggato.
    /// Viene lasciato vuoto se il chiamante non fornisce esplicitamente nulla.
    /// </summary>
    public string Descrizione { get; }
}
