namespace BankApp.Domain;

/// <summary>
/// Rappresenta un movimento registrato su un conto bancario.
/// </summary>
public record Transazione(DateTime Data, decimal Importo, string Tipo, string? Descrizione);
