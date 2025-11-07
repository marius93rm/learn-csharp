using System;

namespace LogGuidato.Step2;

/// <summary>
/// Classe con metodi decorati da individuare via reflection.
/// Rispetto allo step precedente non richiamiamo più i metodi manualmente:
/// sarà <see cref="ReflectionRunner"/> a farlo in modo dinamico.
/// </summary>
public class ServizioOrdini
{
    [Loggable("Crea un report sintetico")]
    public void GeneraReport()
    {
        Console.WriteLine("[Step2] Report generato.");
    }

    /// <summary>
    /// Secondo metodo loggabile: simuliamo l'invio di notifiche push di aggiornamento.
    /// </summary>
    [Loggable("Invia le notifiche di aggiornamento stato")] 
    public void InviaNotifiche()
    {
        Console.WriteLine("[Step2] Notifiche inviate agli utenti attivi.");
    }

    /// <summary>
    /// Metodo volutamente non decorato: la reflection non dovrà mai richiamarlo.
    /// </summary>
    public void PuliziaCache()
    {
        Console.WriteLine("[Step2] Pulizia cache interna (non loggato).");
    }
}
