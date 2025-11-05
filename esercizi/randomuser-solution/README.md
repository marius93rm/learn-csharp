# RandomUser Solution (soluzione commentata)

Questa cartella contiene una possibile soluzione **interamente commentata** per l'esercizio dedicato a
[RandomUser](https://randomuser.me). L'obiettivo è mostrare, con codice quanto più chiaro possibile, come
interrogare l'API pubblica per ottenere una lista di utenti fittizi e visualizzarla in console.

## Struttura del progetto

```
randomuser-solution/
├── RandomUserSolution.sln           # Solution che include la sola app console
└── src/
    └── RandomUserSolution.App/
        ├── Program.cs               # Punto di ingresso dell'applicazione
        ├── Models/                  # Record e classi di dominio (AppOptions, RandomUser, ...)
        ├── Services/                # RandomUserService con HttpClient e deserializzazione JSON
        └── Utilities/               # Parser degli argomenti e formatter della tabella
```

Tutti i file sono ricchi di commenti in italiano per facilitare lo studio.

## Requisiti

* [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0)
* Accesso a Internet (l'app effettua richieste HTTP verso `https://randomuser.me`)

## Come eseguire

1. Ripristina i pacchetti NuGet e compila:
   ```bash
   dotnet restore RandomUserSolution.sln
   dotnet build RandomUserSolution.sln
   ```
2. Avvia la console specificando eventualmente i filtri disponibili:
   ```bash
   dotnet run --project src/RandomUserSolution.App -- --count 5 --nat IT --gender female --seed demo
   ```

L'output consiste in una tabella con nome, email, nazionalità, genere e numero di telefono dei profili
fittizi restituiti dall'API. Usando il parametro `--seed` è possibile ottenere risultati ripetibili
(comodo durante le verifiche).

## Scelte progettuali principali

* **Separazione delle responsabilità** – `Program.cs` delega a `ArgumentParser`, `RandomUserService` e
  `ConsoleTableFormatter` le varie fasi dell'elaborazione.
* **Service per l'accesso ai dati** – `RandomUserService` si occupa di costruire la query string,
  chiamare l'API, gestire eventuali errori e trasformare il JSON in oggetti di dominio.
* **Commenti didattici** – Ogni file inizia con un riquadro descrittivo e contiene note inline per
  spiegare il perché delle scelte implementative.

## Milestone della soluzione

Per facilitare la comprensione, la soluzione è stata pensata in **milestone** progressive che puoi
seguire (o ripercorrere) passo dopo passo:

1. **Impostazione della struttura** – Creazione della solution con un progetto console separato, più
   cartelle per modelli, servizi e utility. Questo aiuta a mantenere il codice organizzato fin da
   subito e ad abituarsi a una suddivisione pulita delle responsabilità.
2. **Definizione dei modelli** – Introduzione dei record `AppOptions`, `RandomUser` e
   `RandomUserApiResponse` per rappresentare, rispettivamente, i parametri da linea di comando, i
   dati di un singolo utente e il payload restituito dall'API. In questa fase i commenti spiegano come
   ogni proprietà mappi il JSON originale.
3. **Parsing degli argomenti** – Implementazione di `ArgumentParser`, che valida i parametri e fornisce
   messaggi d'errore chiari in caso di input non valido. La milestone include la descrizione delle
   regole applicate (limiti, valori ammessi, ecc.).
4. **Chiamata HTTP e deserializzazione** – Costruzione di `RandomUserService`, responsabile di comporre
   la query string, eseguire la richiesta HTTP con `HttpClient`, gestire le eccezioni più comuni e
   deserializzare la risposta JSON in oggetti .NET. I commenti evidenziano come vengono gestiti status
   code non riusciti e tempi di timeout.
5. **Formattazione dell'output** – Sviluppo di `ConsoleTableFormatter`, che impagina i dati in una
   tabella leggibile rispettando la larghezza delle colonne. La milestone illustra la logica di
   calcolo delle colonne e gli accorgimenti per un output allineato.
6. **Orchestrazione finale** – Aggiornamento di `Program.cs` per orchestrare le componenti: parsing,
   chiamata al servizio e stampa a video. I commenti mostrano il flusso completo, incluso il blocco
   `try/catch` finale per intercettare eventuali eccezioni e comunicare errori significativi.

Seguendo queste milestone puoi esplorare la soluzione in modo incrementale, verificando a ogni passo
il comportamento dell'applicazione e comprendendo il motivo di ciascuna scelta.

Questa soluzione vuole essere un punto di partenza: sentiti libero di estenderla con test automatizzati,
cache locale, esportazione in CSV o altre funzionalità utili al tuo percorso di studio! 😊
