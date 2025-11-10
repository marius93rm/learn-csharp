# Soluzione completa – Percorso guidato "ValidazioneModelliGuidato"

Questa cartella contiene una possibile implementazione completa dell'esercizio proposto nel percorso guidato. La soluzione è stata strutturata per mostrare in modo chiaro come estendere il sistema di validazione tramite nuovi attributi senza modificare il validatore generico, rispettando i principi **SRP** e **OCP**.

## Struttura del progetto

```
ValidazioneModelliGuidato.Solution/
├── Models/
│   └── Utente.cs
├── Validation/
│   ├── Attributes/
│   │   ├── MinLunghezzaAttribute.cs
│   │   └── ObbligatorioAttribute.cs
│   ├── IRegolaValidazione.cs
│   ├── IValidabile.cs
│   ├── MessaggioValidazione.cs
│   └── Validatore.cs
├── Program.cs
└── ValidazioneModelliGuidato.Solution.csproj
```

* **Models** contiene il dominio (nel nostro caso il modello `Utente`).
* **Validation** racchiude il framework di validazione riutilizzabile.
* **Program.cs** mostra un esempio d'uso che stampa l'esito per più utenti.

## Scelte progettuali

1. **Separazione delle responsabilità** – Il modello `Utente` conosce i propri attributi e implementa `IValidabile`, mentre `Validatore<T>` si limita a orchestrare la chiamata al metodo `Valida` delle istanze.
2. **Estendibilità tramite interfacce** – Ogni attributo di validazione implementa `IRegolaValidazione`. In questo modo possiamo aggiungere nuove regole semplicemente creando nuovi attributi senza toccare il validatore.
3. **Messaggi tipizzati** – La struct `MessaggioValidazione` distingue tra errori, avvisi e informazioni. Questo permette di comunicare più contesto al chiamante e gestire scenari in cui un attributo viene usato in modo improprio (es. `[MinLunghezza]` su un tipo non supportato).
4. **Reflection controllata** – La reflection è confinata nel metodo `Utente.Valida`, così da mantenere il codice facile da testare e da estendere.

## Esecuzione

Per provare la soluzione:

```bash
dotnet run --project esercizi/ValidazioneModelliGuidato/ValidazioneModelliGuidato.Solution/ValidazioneModelliGuidato.Solution.csproj
```

L'output mostrerà per ogni utente se la validazione è andata a buon fine e l'elenco dei messaggi generati (errori, avvisi, informazioni).

## Estratto di codice commentato

Il seguente frammento, tratto da `Program.cs`, evidenzia l'uso del validatore e include commenti esplicativi.

```csharp
var validatore = new Validatore<Utente>();
var risultati = validatore.ValidaTutti(utenti); // Iteratore lazy: valida ogni utente al momento dell'iterazione.

foreach (var esito in risultati)
{
    StampaEsito(esito); // Logica di presentazione separata dal dominio.
}
```

Mentre la classe `Utente` mostra come le regole siano dichiarate tramite attributi e come vengano lette con la reflection:

```csharp
[Obbligatorio]
[MinLunghezza(8)]
public string? Password { get; set; } // La password deve esistere e rispettare la lunghezza minima.

public bool Valida(out List<MessaggioValidazione> messaggi)
{
    messaggi = new List<MessaggioValidazione>();
    foreach (var proprieta in GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
    {
        var valore = proprieta.GetValue(this);
        var regole = proprieta.GetCustomAttributes().OfType<IRegolaValidazione>();
        foreach (var regola in regole)
        {
            var esito = regola.Verifica(valore, proprieta); // Ogni attributo decide se emettere un messaggio.
            if (esito is not null)
            {
                messaggi.Add(esito.Value);
            }
        }
    }
    return messaggi.All(m => m.Livello != LivelloMessaggio.Errore); // Il modello è valido solo se non ci sono errori.
}
```

I commenti inline servono a collegare il codice alle scelte progettuali descritte sopra, facilitando la consultazione e l'adattamento del progetto a scenari reali.
