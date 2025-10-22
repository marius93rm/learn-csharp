using System.Collections.Generic;
using BankApp.Domain;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.WriteLine("==========================================");
Console.WriteLine(" Gestione Banca — Esercizio OOP (C#)");
Console.WriteLine("==========================================\n");
Console.WriteLine("Completa le milestone M1..M6 implementando i TODO indicati nei file del dominio.");
Console.WriteLine("Esegui quindi le operazioni suggerite qui sotto e osserva l'output.");

// M1 — Creiamo un cliente di esempio su cui lavorare.
var cliente = new Cliente(Guid.NewGuid(), "Mario", "Rossi");

// M2 — Apriamo un conto corrente e registriamo alcune operazioni.
var contoCorrente = new ContoCorrente("123-CC", cliente, saldoIniziale: 500m, fido: 200m);
contoCorrente.Deposita(250m);

// M3 — Eseguiamo prelievi mostrando la gestione delle eccezioni quando si supera il fido.
try
{
    contoCorrente.Preleva(400m); // Prelievo valido entro saldo + fido.
    contoCorrente.Preleva(700m); // Questo dovrebbe superare il fido disponibile.
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"[AVVISO] Prelievo non riuscito: {ex.Message}");
}

// M4 — Creiamo un conto risparmio, eseguiamo un deposito e applichiamo gli interessi mensili.
var contoRisparmio = new ContoRisparmio("123-RS", cliente, saldoIniziale: 800m, tassoInteresseAnnuale: 0.04m);
contoRisparmio.Deposita(200m);
contoRisparmio.ApplicaInteressi();

// M5/M6 — Stampa polimorficamente gli estratti conto.
var contiCliente = new List<ContoBancario> { contoCorrente, contoRisparmio };
foreach (var conto in contiCliente)
{
    conto.StampaEstrattoConto();
}

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
