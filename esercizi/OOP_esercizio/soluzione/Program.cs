using System;
using System.Collections.Generic;

namespace GestioneStudenti;

/// <summary>
/// Esempio di utilizzo delle classi completate: crea studenti, calcola medie e salva i dati.
/// </summary>
public static class Program
{
    public static void Main()
    {
        // Inizializziamo una lista con studenti di tipologie differenti.
        var studenti = new List<Studente>
        {
            new Studente("Luca", "Bianchi")
            {
                // Aggiungiamo alcuni voti sfruttando il costruttore completato.
            },
            new StudenteUniversitario("Anna", "Rossi", "Informatica")
        };

        studenti[0].AggiungiVoto(30);
        studenti[0].AggiungiVoto(27);
        studenti[0].AggiungiVoto(29);

        var universitaria = (StudenteUniversitario)studenti[1];
        universitaria.AggiungiVoto(28);
        universitaria.AggiungiVoto(30);
        universitaria.AggiungiVoto(26);
        universitaria.AggiungiVoto(29);
        universitaria.AggiungiVoto(30);
        universitaria.AggiungiVoto(27);

        var repository = new CsvRepository("studenti-demo.csv");

        foreach (var studente in studenti)
        {
            var media = studente.CalcolaMedia();
            Console.WriteLine($"Studente: {studente.Nome} {studente.Cognome}");
            Console.WriteLine($"Media voti: {media:0.00}");

            repository.SalvaStudente(studente);
            Console.WriteLine("Dati salvati su file.\n");
        }
    }
}
