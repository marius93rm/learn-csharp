using System.Reflection;
using ValidazioneModelliGuidato.Solution.Validation;

namespace ValidazioneModelliGuidato.Solution.Validation.Attributes;

/// <summary>
/// Indica che il valore della proprietà è obbligatorio.
/// Implementa <see cref="IRegolaValidazione"/> in modo da poter essere eseguito dal validatore generico.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public sealed class ObbligatorioAttribute : Attribute, IRegolaValidazione
{
    /// <summary>
    /// Messaggio personalizzabile per fornire indicazioni specifiche al chiamante.
    /// </summary>
    public ObbligatorioAttribute(string? messaggioPersonalizzato = null)
    {
        MessaggioPersonalizzato = messaggioPersonalizzato;
    }

    /// <summary>
    /// Testo alternativo da mostrare quando il campo non è valorizzato.
    /// </summary>
    public string? MessaggioPersonalizzato { get; }

    /// <inheritdoc />
    public MessaggioValidazione? Verifica(object? valore, PropertyInfo proprieta)
    {
        // Valore completamente mancante (null) -> errore diretto.
        if (valore is null)
        {
            return MessaggioValidazione.Error(CreaMessaggio(proprieta.Name, "è obbligatoria."));
        }

        // Se la proprietà è una stringa controlliamo anche spazi vuoti.
        if (valore is string testo && string.IsNullOrWhiteSpace(testo))
        {
            return MessaggioValidazione.Error(CreaMessaggio(proprieta.Name, "non può essere vuota."));
        }

        // Nessun messaggio significa regola soddisfatta.
        return null;
    }

    private string CreaMessaggio(string nomeProprieta, string messaggioDiDefault)
        => MessaggioPersonalizzato ?? $"La proprietà {nomeProprieta} {messaggioDiDefault}";
}
