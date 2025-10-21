using System.Globalization;

namespace Pomodoro.App.Persistence;

public class CsvSessionRepository : ISessionRepository
{
    private readonly string _filePath;

    public CsvSessionRepository(string filePath)
    {
        _filePath = filePath;
    }

    public Task SaveAsync(PomodoroSessionLog session, CancellationToken cancellationToken = default)
    {
        // TODO: serializzare la sessione in formato CSV e salvarla su disco.
        // Suggerimento: usa CultureInfo.InvariantCulture e File.AppendAllText.
        throw new NotImplementedException("TODO: CsvSessionRepository.SaveAsync deve essere implementato dagli studenti.");
    }
}
