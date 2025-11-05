// ---------------------------------------------------------------------------------------------------------------------
// Program.cs
// ---------------------------------------------------------------------------------------------------------------------
// Questo file contiene il punto di ingresso della nostra applicazione console. L'obiettivo dell'esercizio
// è mostrare come interrogare l'API pubblica "https://randomuser.me" per ottenere utenti fittizi e
// presentare il risultato in maniera leggibile. L'applicazione è volutamente semplice e ricca di commenti
// per aiutare lo studio del codice.
// ---------------------------------------------------------------------------------------------------------------------

using RandomUserSolution.App.Models;
using RandomUserSolution.App.Services;
using RandomUserSolution.App.Utilities;

// Creiamo un'istanza del parser degli argomenti. Questa classe incapsula tutta la logica necessaria
// per interpretare le opzioni passate a riga di comando (es. "--count 5").
var parser = new ArgumentParser();

AppOptions options;

try
{
    // Se il parsing fallisce (per esempio perché l'utente ha scritto un argomento non riconosciuto)
    // vogliamo interrompere l'applicazione mostrando un messaggio chiaro.
    options = parser.Parse(args);
}
catch (ArgumentException parseError)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(parseError.Message);
    Console.ResetColor();
    return; // Uscita anticipata: non possiamo proseguire senza opzioni valide.
}

// HttpClient è l'oggetto suggerito da Microsoft per effettuare chiamate HTTP. Normalmente si userebbe
// una singola istanza riutilizzata tramite dependency injection. In questo esempio utilizziamo il
// factory method messo a disposizione dal nostro servizio per ottenere un client preconfigurato.
using var httpClient = RandomUserService.CreateHttpClient();

// Il servizio incapsula tutta la logica di chiamata HTTP, deserializzazione e proiezione in oggetti di dominio.
var service = new RandomUserService(httpClient);

try
{
    // Richiediamo l'elenco degli utenti secondo le opzioni fornite dall'utente.
    var people = await service.GetRandomUsersAsync(
        options.RequestedPeople,
        options.NationalityFilter,
        options.GenderFilter,
        options.Seed);

    // Stampiamo una tabella leggibile. Il formatter pensa a spaziature e intestazioni.
    ConsoleTableFormatter.PrintPeople(people, options);
}
catch (HttpRequestException networkError)
{
    // L'eccezione HttpRequestException viene sollevata da HttpClient per errori di rete.
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("Errore di rete durante la chiamata a randomuser.me: " + networkError.Message);
    Console.ResetColor();
}
catch (RandomUserServiceException serviceError)
{
    // Eccezione personalizzata: il servizio la usa per segnalare problemi specifici (es. JSON non valido).
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("Impossibile completare la richiesta: " + serviceError.Message);
    Console.ResetColor();
}
