using BankApp.Domain;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.WriteLine("==========================================");
Console.WriteLine(" Gestione Banca — Esercizio OOP (C#)");
Console.WriteLine("==========================================\n");
Console.WriteLine("Completa le milestone M1..M6 implementando i TODO indicati nei file del dominio.");
Console.WriteLine("Esegui quindi le operazioni suggerite qui sotto e osserva l'output.");

// TODO [M1]: crea un'istanza di Cliente con dati di esempio.
// var cliente = new Cliente(...);

// TODO [M2]: crea un ContoCorrente per il cliente e registra un deposito iniziale.
// ContoCorrente contoCorrente = ...;
// contoCorrente.Deposita(...);

// TODO [M3]: prova a prelevare sia entro il saldo disponibile sia sfruttando l'eventuale fido.
// Suggerimento: avvolgi il prelievo in un blocco try/catch per intercettare InvalidOperationException.

// TODO [M4]: crea un ContoRisparmio per lo stesso cliente, effettua un deposito e applica gli interessi.

// TODO [M5]: stampa gli estratti conto di entrambi i conti iterando polimorficamente su una lista di ContoBancario.

Console.WriteLine("\nQuando hai completato tutte le milestone, l'estratto conto dovrebbe mostrare le transazioni registrate.");

/*
Output atteso indicativo (i valori possono variare a seconda dei dati scelti):

=== ESTRATTO CONTO (123-CC) — Mario Rossi ===
2025-10-21  DEPOSITO    +500,00   Saldo: 500,00
2025-10-21  PRELIEVO    -200,00   Saldo: 300,00
2025-10-22  PRELIEVO    -150,00   Saldo: 150,00

=== ESTRATTO CONTO (123-RS) — Mario Rossi ===
2025-10-21  DEPOSITO    +800,00   Saldo: 800,00
2025-10-21  INTERESSI   +  5,33   Saldo: 805,33
*/
