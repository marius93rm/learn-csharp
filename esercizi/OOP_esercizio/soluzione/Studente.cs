using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace GestioneStudenti;

/// <summary>
/// Implementazione completa e commentata delle classi richieste dalle milestone.
/// </summary>
public class Studente
{
    // Proprietà in sola lettura dall'esterno per preservare l'incapsulamento.
    public string Nome { get; protected set; }
    public string Cognome { get; protected set; }
    public List<int> Voti { get; } = new();

    public Studente(string nome, string cognome)
    {
        // Validiamo gli argomenti per evitare istanze incomplete.
        Nome = string.IsNullOrWhiteSpace(nome)
            ? throw new ArgumentException("Il nome non può essere vuoto", nameof(nome))
            : nome.Trim();

        Cognome = string.IsNullOrWhiteSpace(cognome)
            ? throw new ArgumentException("Il cognome non può essere vuoto", nameof(cognome))
            : cognome.Trim();
    }

    public void AggiungiVoto(int voto)
    {
        // Accettiamo il range tipico 18-30 ma permettiamo di personalizzare facilmente.
        if (voto is < 0 or > 30)
        {
            throw new ArgumentOutOfRangeException(nameof(voto), "Il voto deve essere compreso tra 0 e 30.");
        }

        Voti.Add(voto);
    }

    public virtual double CalcolaMedia()
    {
        // Se non sono presenti voti restituiamo 0 evitando divisioni per zero.
        if (Voti.Count == 0)
        {
            return 0;
        }

        return Voti.Average();
    }
}

/// <summary>
/// Estensione della classe base con il comportamento opzionale della Milestone 4.
/// </summary>
public class StudenteUniversitario : Studente
{
    public string CorsoDiLaurea { get; protected set; }

    public StudenteUniversitario(string nome, string cognome, string corsoDiLaurea)
        : base(nome, cognome)
    {
        CorsoDiLaurea = string.IsNullOrWhiteSpace(corsoDiLaurea)
            ? throw new ArgumentException("Il corso di laurea non può essere vuoto", nameof(corsoDiLaurea))
            : corsoDiLaurea.Trim();
    }

    public override double CalcolaMedia()
    {
        var mediaBase = base.CalcolaMedia();

        // Applichiamo un bonus di +1 punto solo quando sono presenti più di 5 voti.
        if (Voti.Count > 5 && mediaBase > 0)
        {
            return Math.Min(mediaBase + 1, 30); // non superiamo il massimo di 30.
        }

        return mediaBase;
    }
}

/// <summary>
/// Gestisce la persistenza su file CSV come richiesto dalla Milestone 5 opzionale.
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
        if (studente is null)
        {
            throw new ArgumentNullException(nameof(studente));
        }

        var media = studente.CalcolaMedia();
        var line = string.Join(";", new[]
        {
            studente.Nome,
            studente.Cognome,
            media.ToString("0.00", CultureInfo.InvariantCulture)
        });

        File.AppendAllText(PercorsoFile, line + Environment.NewLine);
    }
}
