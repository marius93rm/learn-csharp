# Esercizio: Architettura a Microservizi in C#

Benvenuto in questo esercizio guidato: lavorerai su un progetto educativo che ti
permette di esplorare i concetti principali delle architetture a microservizi
attraverso una simulazione leggera. Il progetto è composto da servizi
indipendenti che comunicano tra loro tramite interfacce e DTO condivisi.

## 1. Cos'è un microservizio e come differisce da un monolite

Un **monolite** concentra tutta la logica applicativa in un'unica codebase,
spesso con un singolo processo di deploy. Questo significa che:
- le responsabilità si mescolano;
- un bug o un errore di performance può impattare l'intero sistema;
- è difficile scalare solo alcune parti dell'applicazione.

Un **microservizio** è invece un'unità autonoma:
- possiede responsabilità ristrette e ben definite (Single Responsibility
  Principle);
- espone un'API (nel nostro caso un'API simulata) per interagire con gli altri
  servizi;
- può essere rilasciato e scalato indipendentemente.

Nel progetto troverai tre microservizi: `UserService`, `OrderService` e un
`Gateway` che funge da facciata. Ogni servizio implementa la propria logica, ma
si coordina tramite interfacce e modelli condivisi nella cartella `Shared`.

## 2. Comunicazione tra servizi: REST simulato con metodi e DTO condivisi

Non useremo un vero server HTTP, ma simuleremo chiamate REST. Ogni servizio
espone interfacce che rappresentano endpoint (ad esempio `IUserReadService` per
recuperare utenti). Il **Gateway** agisce come API gateway ed esegue chiamate ai
servizi tramite tali interfacce. L'uso di DTO condivisi (`Shared/Models.cs`)
permette di scambiare dati senza condividere le implementazioni.

Ricorda: la comunicazione avviene *solo* tramite interfacce. I servizi non
conoscono i dettagli interni degli altri: rispettano l'**Open/Closed Principle**
perché possono essere estesi sostituendo l'implementazione dietro un'interfaccia
senza modificare il contratto.

## 3. Uso di interfacce e DIP per separare logica e infrastruttura

Applicherai il **Dependency Inversion Principle (DIP)** introducendo astrazioni
per repository e client HTTP simulati. Per esempio, il `UserService`
conoscerà solo un'interfaccia `IUserRepository`. L'implementazione concreta
potrebbe essere in memoria, su file o su database, ma il servizio non deve
saperlo. Gli oggetti verranno risolti tramite una semplice **Dependency
Injection** manuale (creazione delle dipendenze in un punto centrale e passaggio
via costruttore).

## 4. Principi di resilienza e isolamento dei servizi

Ogni microservizio dovrebbe poter fallire o rallentare senza bloccare gli altri.
Nel progetto simulerai:
- time-out o eccezioni nella comunicazione (TODO nel `Gateway` e negli adapter);
- fallback o messaggi di errore chiari verso il client;
- logica idempotente nella creazione ordini.

Questi aspetti aiutano ad applicare concetti come **Bulkhead** e
**Circuit Breaker** (anche solo come discussione o estensioni opzionali).

## 5. Guida alle milestone

1. **Creare UserService**: implementa repository in memoria, logica di validazione
   e interfacce per lettura utenti.
2. **Integrare OrderService**: aggiungi un repository in memoria per gli ordini e
   logica di business per la creazione con controllo duplicati.
3. **Creare Gateway**: comporre i servizi tramite dependency injection e
   orchestrare il flusso "crea ordine".
4. **Gestire errori e simulare timeout**: completa i TODO dedicati a resilienza e
   gestione degli errori in modo da restituire risposte coerenti.

## Obiettivo finale

Il Gateway deve essere in grado di:
1. Ricevere una richiesta di creazione ordine (simulata tramite metodo).
2. Verificare tramite `UserService` se l'utente esiste.
3. Creare un ordine tramite `OrderService`.
4. Restituire un risultato combinato (ad esempio un DTO `OrderConfirmationDto`).

Ricorda di mantenere i servizi indipendenti: ogni comunicazione avviene tramite
interfacce e modelli condivisi. I file contengono TODO numerati che ti guideranno
passo dopo passo.

Buon lavoro e buon divertimento!
