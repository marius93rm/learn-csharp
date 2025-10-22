using System;
using System.Collections.Generic;

namespace GestioneStudenti
{
    /// <summary>
    /// Rappresenta uno studente della scuola. Completa la classe seguendo le milestone.
    /// </summary>
    public class Studente
    {
        // TODO Milestone 1: valuta se rendere le proprietà a sola lettura dall'esterno.
        public string Nome { get; protected set; } = string.Empty;
        public string Cognome { get; protected set; } = string.Empty;
        public List<int> Voti { get; protected set; } = new();

        /// <summary>
        /// Crea un nuovo studente impostando nome e cognome.
        /// </summary>
        /// <param name="nome">Il nome dello studente.</param>
        /// <param name="cognome">Il cognome dello studente.</param>
        public Studente(string nome, string cognome)
        {
            // TODO Milestone 1: assegna nome e cognome e inizializza la lista dei voti.
            throw new NotImplementedException("Milestone 1: implementa il costruttore di Studente.");
        }

        /// <summary>
        /// Aggiunge un voto alla lista dei voti dello studente.
        /// </summary>
        /// <param name="voto">Il voto da aggiungere (0-30, ma puoi decidere tu le regole).</param>
        public void AggiungiVoto(int voto)
        {
            // TODO Milestone 1: aggiungi il voto alla lista. Valuta se validare l'input.
            throw new NotImplementedException("Milestone 1: implementa AggiungiVoto.");
        }

        /// <summary>
        /// Calcola la media dei voti dello studente.
        /// </summary>
        /// <returns>La media aritmetica dei voti oppure 0 se non sono presenti voti.</returns>
        public virtual double CalcolaMedia()
        {
            // TODO Milestone 2: calcola la media e gestisci il caso senza voti.
            throw new NotImplementedException("Milestone 2: implementa CalcolaMedia.");
        }
    }

    /// <summary>
    /// Milestone 4 (opzionale): estendi Studente per rappresentare uno studente universitario.
    /// </summary>
    public class StudenteUniversitario : Studente
    {
        // TODO Milestone 4: aggiungi la proprietà CorsoDiLaurea e il costruttore dedicato.
        public string CorsoDiLaurea { get; protected set; } = string.Empty;

        public StudenteUniversitario(string nome, string cognome, string corsoDiLaurea)
            : base(nome, cognome)
        {
            // TODO Milestone 4: salva il corso di laurea.
            throw new NotImplementedException("Milestone 4: completa il costruttore di StudenteUniversitario.");
        }

        /// <summary>
        /// Calcola la media dei voti applicando un bonus di +1 se ci sono più di 5 voti.
        /// </summary>
        /// <returns>La media con il bonus previsto.</returns>
        public override double CalcolaMedia()
        {
            // TODO Milestone 4: richiedi alla classe base il calcolo della media e applica il bonus.
            throw new NotImplementedException("Milestone 4: sovrascrivi CalcolaMedia.");
        }
    }

    /// <summary>
    /// Milestone 5 (opzionale): gestisce il salvataggio degli studenti su file CSV.
    /// </summary>
    public class CsvRepository
    {
        public string PercorsoFile { get; }

        public CsvRepository(string percorsoFile = "studenti.csv")
        {
            PercorsoFile = percorsoFile;
        }

        public void SalvaStudente(Studente studente)
        {
            // TODO Milestone 5: scrivi sul file una nuova riga con Nome;Cognome;Media.
            // Puoi usare File.AppendAllText o File.AppendAllLines da System.IO.
            throw new NotImplementedException("Milestone 5: implementa la persistenza su CSV.");
        }
    }
}
