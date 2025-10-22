namespace BankApp.Domain;

/// <summary>
/// Rappresenta un cliente della banca.
/// </summary>
public class Cliente
{
    /// <summary>
    /// Identificativo univoco del cliente.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Nome proprio del cliente.
    /// </summary>
    public string Nome { get; }

    /// <summary>
    /// Cognome del cliente.
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
        // L'identificativo non deve essere il valore vuoto di Guid.
        if (id == Guid.Empty)
        {
            throw new ArgumentException("L'identificativo del cliente non può essere vuoto.", nameof(id));
        }

        // Nome e cognome devono essere valorizzati (non null o spazi).
        if (string.IsNullOrWhiteSpace(nome))
        {
            throw new ArgumentException("Il nome del cliente è obbligatorio.", nameof(nome));
        }

        if (string.IsNullOrWhiteSpace(cognome))
        {
            throw new ArgumentException("Il cognome del cliente è obbligatorio.", nameof(cognome));
        }

        Id = id;
        Nome = nome.Trim();
        Cognome = cognome.Trim();
    }

    public override string ToString() => $"{Nome} {Cognome}";
}
