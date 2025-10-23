using System.Threading.Tasks;

namespace AsincronaExamples.Examples;

/// <summary>
/// Interfaccia comune per tutti gli esempi asincroni.
/// Ogni esempio fornisce un nome, una descrizione ed un metodo asincrono da eseguire.
/// </summary>
public interface IAsyncExample
{
    /// <summary>
    /// Nome breve dell'esempio da mostrare nel menù.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Descrizione sintetica di ciò che l'esempio dimostra.
    /// </summary>
    string Description { get; }

    /// <summary>
    /// Metodo principale dell'esempio. Deve sfruttare le primitive asincrone di .NET.
    /// </summary>
    Task RunAsync();
}
