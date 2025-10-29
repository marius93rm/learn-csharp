using System;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Pomodoro.App.Persistence;

namespace Pomodoro.Exercise.Solution.Persistence;

/// <summary>
/// Variante commentata di <see cref="Pomodoro.App.Persistence.CsvSessionRepository"/>.
/// Mostra come serializzare il log in formato CSV rispettando la responsabilità singola del repository.
/// </summary>
public sealed class CsvSessionRepositorySolution : ISessionRepository
{
    private readonly string _filePath;

    public CsvSessionRepositorySolution(string filePath)
    {
        _filePath = filePath;
    }

    public async Task SaveAsync(PomodoroSessionLog session, CancellationToken cancellationToken = default)
    {
        // Creiamo la cartella di destinazione se non esiste già: il repository si occupa
        // esclusivamente della persistenza su disco.
        var directory = Path.GetDirectoryName(_filePath);
        if (!string.IsNullOrWhiteSpace(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        // Serializziamo con formato ISO 8601 per la data e culture invariabile per gli interi.
        var line = string.Format(
            CultureInfo.InvariantCulture,
            "{0:o};{1};{2};{3}",
            session.StartedAt,
            session.FocusSeconds,
            session.BreakSeconds,
            session.StrategyName);

        // Appendiamo una nuova riga, lasciando all'OS la gestione dell'EOF.
        await File.AppendAllTextAsync(
            _filePath,
            line + Environment.NewLine,
            cancellationToken).ConfigureAwait(false);
    }
}
