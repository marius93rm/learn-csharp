using RandomUserShifts.Domain.Abstractions;
using RandomUserShifts.Domain.Entities;

namespace RandomUserShifts.Infrastructure.Persistence;

public sealed class FileScheduleRepository : IScheduleRepository
{
    private readonly string _basePath;

    public FileScheduleRepository(string basePath)
    {
        _basePath = basePath;
    }

    public Task SaveAsync(Schedule schedule, CancellationToken cancellationToken = default)
    {
        // TODO[5]: salva/carica JSON nel path ./data/schedules/{yyyy-MM-dd}.json (crea cartella se manca).
        return Task.CompletedTask;
    }

    public Task<Schedule?> LoadAsync(DateOnly weekStart, CancellationToken cancellationToken = default)
    {
        // TODO[5]: salva/carica JSON nel path ./data/schedules/{yyyy-MM-dd}.json (crea cartella se manca).
        return Task.FromResult<Schedule?>(null);
    }
}
