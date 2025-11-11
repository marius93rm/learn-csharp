using System;

namespace RegistroPresenzeDipendenti.Models
{
    /* Milestone 1: Creazione del modello dati "Dipendente" con nome, oraIngresso, oraUscita */
    public class Dipendente
    {
        public string Nome { get; set; } = string.Empty;

        public TimeSpan OraIngresso { get; set; }

        public TimeSpan OraUscita { get; set; }

        // TODO Studente: aggiungi qui eventuali campi extra (es. SmartWorking, Reparto, Note)
        // per rendere il modello più aderente ai processi della tua azienda.

        public TimeSpan OreTotali =>
            OraUscita >= OraIngresso
                ? OraUscita - OraIngresso
                : TimeSpan.Zero;

        public string OreTotaliFormattate =>
            string.Format("{0:%h}h {0:%m}m", OreTotali);
    }
}
