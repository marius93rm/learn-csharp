# OsservatoreFileSystem

## Introduzione
`FileSystemWatcher` è una classe del namespace `System.IO` di .NET pensata per monitorare in tempo reale le modifiche che avvengono all'interno del file system. È particolarmente utile quando serve reagire automaticamente a eventi su cartelle condivise, directory di log o repository di integrazione continua: ogni variazione può essere intercettata per avviare notifiche, task di automazione o procedure di auditing.

## Obiettivo dell'applicazione
L'applicazione console **OsservatoreFileSystem** permette di sorvegliare una cartella specifica e di segnalare immediatamente la creazione, la modifica, l'eliminazione o la rinomina dei file contenuti. L'utente inserisce il percorso da monitorare e riceve in console l'elenco degli eventi con un timestamp leggibile.

## Milestone guidate
1. **Milestone 1: Chiedi all’utente il percorso da monitorare**  
   - Mostra un messaggio di benvenuto.  
   - Richiedi all'utente di digitare il percorso completo della cartella.  
   - Valida il percorso: se è vuoto o la cartella non esiste, termina informando l'utente.
2. **Milestone 2: Crea un oggetto FileSystemWatcher e configuralo**  
   - Istanzia `FileSystemWatcher` passando il percorso scelto.  
   - Imposta eventuali proprietà aggiuntive: `IncludeSubdirectories`, `NotifyFilter` e `EnableRaisingEvents` per avviare il monitoraggio.
3. **Milestone 3: Gestisci eventi Created, Changed, Deleted, Renamed**  
   - Registra i delegati per ciascun evento e stampa un messaggio informativo con il nome del file coinvolto.  
   - Per il rinomino mostra il vecchio e il nuovo nome.
4. **Milestone 4: Mantieni il programma in attesa con Console.ReadKey**  
   - Scrivi un messaggio che inviti l'utente a premere un tasto per terminare.  
   - Utilizza `Console.ReadKey()` per bloccare l'applicazione finché non arriva l'input.

## Codice d’esempio minimo
```csharp
using System.IO;

var watcher = new FileSystemWatcher(@"C:\\Percorso\\Da\\Monitorare")
{
    EnableRaisingEvents = true
};

watcher.Created += (_, e) =>
    Console.WriteLine($"Nuovo file: {e.Name}");

Console.ReadKey();
```

## Come testare l'applicazione
1. Avvia il progetto `OsservatoreFileSystem`.  
2. Inserisci il percorso della cartella che desideri monitorare.  
3. In un altro terminale o tramite Esplora File crea, modifica, rinomina o elimina alcuni file all'interno della cartella.  
4. Osserva la console: per ogni azione riceverai una notifica immediata con l'orario dell'evento.

## Output atteso
```
[CREATO] test.txt alle 12:03:14
[MODIFICATO] report.docx alle 12:04:22
[RINOMINATO] da log.txt a log_old.txt alle 12:05:00
```

## Tecnologie usate
- .NET 6+
- C#
- Console Application
- System.IO.FileSystemWatcher

Questo esercizio è pensato per professionisti IT, sviluppatori o sysadmin che desiderano automatizzare il controllo di cartelle condivise o log di sistema senza dover ricorrere a strumenti complessi.
