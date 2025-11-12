# MonitorStatoSistema

## Introduzione
"Uptime" indica il tempo trascorso dall'ultimo avvio del sistema operativo, un parametro spesso utilizzato per capire stabilità e affidabilità di una macchina. Il "memory status" rappresenta invece la quantità di memoria fisica totale e disponibile, utile per individuare rapidamente situazioni di stress. In ambiente .NET queste informazioni non sono esposte direttamente tramite API managed: occorre ricorrere al **Platform Invocation Services (P/Invoke)**, un meccanismo che permette al codice C# di chiamare funzioni native di librerie Windows come `kernel32.dll`.

## Obiettivo didattico
L'esercizio guida passo passo nella creazione di una mini dashboard da console in grado di mostrare lo stato del sistema, consolidando i seguenti concetti:

- Definizione di interfacce P/Invoke in C#.
- Utilizzo di tipi `record struct` per incapsulare metriche.
- Recupero di informazioni sul sistema tramite API native e classi del framework (`Process`, `PerformanceCounter`).
- Gestione di un loop temporizzato con aggiornamento dell'interfaccia testuale.
- Gestione dell'input da tastiera non bloccante tramite `Console.KeyAvailable`.

## Come compilare ed eseguire il progetto
1. Posizionati nella cartella del progetto:
   ```bash
   cd esercizi/MonitorStatoSistema
   ```
2. Ripristina i pacchetti (facoltativo, non sono presenti dipendenze esterne):
   ```bash
   dotnet restore
   ```
3. Compila ed esegui l'applicazione console:
   ```bash
   dotnet run --project MonitorStatoSistema/MonitorStatoSistema.csproj
   ```

> **Nota:** le funzioni P/Invoke utilizzate (`GetTickCount64`, `GlobalMemoryStatusEx`) sono disponibili solo su Windows. L'applicazione verifica il sistema operativo prima di utilizzare `PerformanceCounter` per evitare eccezioni su piattaforme non supportate.

## Milestone guidate
1. **Milestone 1: Importa GetTickCount64 da kernel32.dll e calcola uptime**
   - Definisci la firma P/Invoke per `GetTickCount64` in `NativeMethods.cs`.
   - Chiama la funzione all'interno del loop principale e convertila in `TimeSpan` per formattare l'uptime.
2. **Milestone 2: Importa GlobalMemoryStatusEx e visualizza RAM usata/totale**
   - Dichiara la struct `MEMORYSTATUSEX` con `StructLayout` sequenziale.
   - Richiama `GlobalMemoryStatusEx` per ottenere memoria fisica totale e disponibile.
3. **Milestone 3: Conta i processi attivi con Process.GetProcesses()**
   - Usa `Process.GetProcesses().Length` per ottenere il numero di processi correnti.
4. **Milestone 4: Mostra i dati in console ogni secondo con Console.Clear()**
   - Pulisci la console e stampa i valori formattati, quindi sospendi il thread per 1 secondo.
5. **Milestone 5: Interrompi con Console.ReadKey()**
   - Controlla `Console.KeyAvailable` e interrompi il loop dopo aver letto il tasto.
6. **(Opzionale) Milestone 6: Visualizza utilizzo CPU**
   - Solo su Windows, crea un `PerformanceCounter` per la categoria `Processor`, istanza `_Total`, contatore `% Processor Time`.
   - Ricorda che la prima chiamata a `NextValue()` restituisce 0: effettua una chiamata di riscaldamento e usa il valore successivo.

## P/Invoke: esempi minimi
```csharp
[DllImport("kernel32.dll")]
internal static extern ulong GetTickCount64();

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
private struct MEMORYSTATUSEX
{
    public uint dwLength;
    public ulong ullTotalPhys;
    public ulong ullAvailPhys;
    // Campi rimanenti omessi per brevità
}

[DllImport("kernel32.dll", SetLastError = true)]
private static extern bool GlobalMemoryStatusEx(ref MEMORYSTATUSEX buffer);
```

## Suggerimenti per il testing
- Avvia l'applicazione e confronta i valori mostrati con quelli del Task Manager (schede "Prestazioni" e "Processi").
- Riduci volontariamente la memoria disponibile aprendo applicazioni pesanti per osservare l'aggiornamento in tempo reale.
- Verifica la gestione dell'input premendo un tasto qualsiasi per terminare il programma.

## Output atteso
```
==== Monitor Stato Sistema ====
Uptime sistema: 0d 05h:12m:47s
RAM disponibile: 5.32 GB / 15.92 GB
Processi attivi: 215
Utilizzo CPU: 12.45%

Premi un tasto per uscire...
```

## Estensioni suggerite (opzionale)
- Aggiungi la possibilità di salvare periodicamente le metriche in un file CSV o JSON per analisi successive.
- Integra un parametro da linea di comando per scegliere l'intervallo di aggiornamento.
- Mostra metriche aggiuntive come utilizzo di memoria virtuale o spazio disco.
