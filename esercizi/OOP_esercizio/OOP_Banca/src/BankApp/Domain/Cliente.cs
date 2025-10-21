namespace BankApp.Domain;

/// <summary>
/// Rappresenta un cliente della banca.
/// </summary>
public class Cliente
{
    /// <summary>
    /// TODO [M1]: rendi la proprietà in sola lettura e valida che l'identificativo non sia vuoto.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// TODO [M1]: aggiungi le validazioni opportune per impedire nomi nulli o vuoti.
    /// </summary>
    public string Nome { get; }

    /// <summary>
    /// TODO [M1]: aggiungi le validazioni opportune per impedire cognomi nulli o vuoti.
    /// </summary>
    public string Cognome { get; }

    /// <summary>
    /// Crea un nuovo cliente.
    /// </summary>
    /// <param name="id">Identificativo univoco del cliente.</param>
    /// <param name="nome">Nome del cliente.</param>
    /// <param name="cognome">Cognome del cliente.</param>
    public Cliente(Guid id, string nome, string cognome)
    {
        // TODO [M1]: aggiungi le validazioni richieste prima di assegnare le proprietà.
        Id = id;
        Nome = nome;
        Cognome = cognome;
    }

    public override string ToString() => $"{Nome} {Cognome}";
}
