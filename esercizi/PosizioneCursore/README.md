# PosizioneCursore

Questo esercizio mostra come usare P/Invoke per interrogare le API Win32 da una semplice applicazione console .NET 6. L'obiettivo è leggere la posizione del cursore del mouse ogni mezzo secondo sfruttando la funzione `GetCursorPos` esposta da **user32.dll**.

## Introduzione a P/Invoke e GetCursorPos
Platform Invocation Services (P/Invoke) è il meccanismo che consente al codice managed di .NET di chiamare funzioni native esposte da librerie dinamiche (DLL). Nel nostro caso sfruttiamo `GetCursorPos`, funzione di **user32.dll** che riempie una struttura `POINT` con le coordinate assolute del cursore sullo schermo.

La firma nativa è:

```c
BOOL GetCursorPos(LPPOINT lpPoint);
```

In C# viene tradotta come `bool GetCursorPos(out POINT lpPoint)`: il valore restituito indica il successo dell'operazione.

## Il layout della struct `POINT`
La struttura nativa `POINT` è composta da due interi (`LONG x` e `LONG y`). Perché la memoria venga interpretata correttamente dal runtime .NET, dobbiamo riprodurre la struttura con lo **stesso ordine dei campi** usando l'attributo `[StructLayout(LayoutKind.Sequential)]`. Questo obbliga il compilatore a mantenere i campi nell'esatto ordine dichiarato.

## Collegare user32.dll
Per collegare una DLL nativa si usa l'attributo `[DllImport]`. Specificando il nome della libreria (`"user32.dll"`) indichiamo al runtime dove trovare la funzione. Il binding avviene al momento della prima chiamata a `GetCursorPos`.

## Struttura del progetto
```
esercizi/PosizioneCursore/
├── PosizioneCursore.sln
├── PosizioneCursore/
│   ├── PosizioneCursore.csproj
│   ├── Program.cs
│   └── NativeMethods.cs
└── README.md
```

Apri la soluzione `PosizioneCursore.sln` oppure lavora direttamente nel progetto console `PosizioneCursore/`.

## Guida alle milestone
1. **Milestone 1 – Creazione della struct POINT con layout compatibile**  
   File: `PosizioneCursore/NativeMethods.cs`  
   Dichiarare la struct `POINT` marcata con `[StructLayout(LayoutKind.Sequential)]` e i campi `int X` e `int Y`.

2. **Milestone 2 – Dichiarazione della funzione GetCursorPos con DllImport**  
   File: `PosizioneCursore/NativeMethods.cs`  
   Aggiungere un metodo `GetCursorPos` statico con `[DllImport("user32.dll")]` che restituisce `bool` e prende `out POINT`.

3. **Milestone 3 – Lettura della posizione del cursore ogni 500ms**  
   File: `PosizioneCursore/Program.cs`  
   Nel ciclo principale chiamare `NativeMethods.GetCursorPos(out var point)`, verificare il valore booleano e stampare le coordinate. Facoltativamente usare `Console.Clear()` per aggiornare l'output.

4. **Milestone 4 – Terminazione del loop con Console.ReadKey()**  
   File: `PosizioneCursore/Program.cs`  
   Controllare `Console.KeyAvailable` e, quando un tasto viene premuto, leggere con `Console.ReadKey(true)` per chiudere l'applicazione.

Ogni milestone è contrassegnata da un commento specifico nel codice per facilitare l'individuazione.

## Come testare
1. Apri un terminale in `esercizi/PosizioneCursore/`.
2. Compila ed esegui il progetto (ad esempio con `dotnet run` su Windows con .NET 6 o successivo installato).
3. Osserva la console: ogni 500 ms viene stampata la posizione corrente del cursore.
4. Premi qualsiasi tasto per terminare il programma.

## Output atteso
Durante l'esecuzione viene mostrato un messaggio simile:

```
Premi un tasto per terminare l'applicazione...
Posizione del cursore: X = 842, Y = 520
```

Le coordinate cambieranno continuamente seguendo gli spostamenti del mouse. Alla pressione di un tasto il programma termina immediatamente.

## Tecnologie
- .NET 6
- C#
- Applicazione console
- P/Invoke
- user32.dll
- Strutture (`struct`)
- `Thread.Sleep`
