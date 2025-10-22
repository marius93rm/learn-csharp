# Soluzione commentata — Pomodoro Focus Timer

Questa cartella contiene una possibile implementazione completa dell'esercizio
**Pomodoro Focus Timer**, arricchita da commenti per facilitare lo studio del
codice. Il progetto è una copia del codice fornito agli studenti con i `TODO`
risolti e spiegati.

## Struttura

```
SoluzioneCommentata/
├─ README.md
└─ src/
   └─ Pomodoro.App/
      ├─ Core/
      ├─ Notify/
      ├─ Persistence/
      ├─ Sessions/
      ├─ Timer/
      ├─ Program.cs
      └─ Pomodoro.App.csproj
```

I file nelle cartelle `Core`, `Timer` e `Persistence` contengono le parti più
rilevanti della soluzione, con commenti che spiegano le scelte effettuate
rispetto ai principi SOLID richiesti dall'esercizio.

## Come eseguire la soluzione

Per compilare ed eseguire direttamente questa soluzione puoi lanciare:

```bash
dotnet run --project SoluzioneCommentata/src/Pomodoro.App
```

I test automatici continuano a fare riferimento al progetto originale con i
TODO, così da poterli utilizzare durante le lezioni senza modificare il
materiale assegnato agli studenti.
