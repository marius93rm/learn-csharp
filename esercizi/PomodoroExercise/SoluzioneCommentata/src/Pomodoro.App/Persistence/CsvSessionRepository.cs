using System.Globalization;

namespace Pomodoro.App.Persistence;

public class CsvSessionRepository : ISessionRepository
{
    private readonly string _filePath;

    public CsvSessionRepository(string filePath)
    {
        _filePath = filePath;
    }

    public async Task SaveAsync(PomodoroSessionLog session, CancellationToken cancellationToken = default)
    {
        // Garantiamo che la directory esista prima di tentare l'append. `DirectoryName`
        // può essere nullo se il chiamante passa solo il nome del file.
        var directory = Path.GetDirectoryName(_filePath);
        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }

        // Normalizziamo i valori numerici usando la cultura invariante per evitare
        // virgole/point differenti a seconda del sistema operativo.
        var focus = session.FocusSeconds.ToString(CultureInfo.InvariantCulture);
        var @break = session.BreakSeconds.ToString(CultureInfo.InvariantCulture);
        var startedAt = session.StartedAt.ToString("O", CultureInfo.InvariantCulture);

        // Evitiamo di rompere il formato CSV sostituendo eventuali punti e virgola
        // nel nome strategia con una virgola semplice.
        var strategy = session.StrategyName.Replace(';', ',');

        var line = string.Join(';', new[] { startedAt, focus, @break, strategy }) + Environment.NewLine;

        await File.AppendAllTextAsync(_filePath, line, cancellationToken);
    }
}
