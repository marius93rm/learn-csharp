using RandomUserShifts.Domain.Entities;

namespace RandomUserShifts.Domain.Abstractions;

public interface IScheduleRepository
{
    Task SaveAsync(Schedule schedule, CancellationToken cancellationToken = default);

    Task<Schedule?> LoadAsync(DateOnly weekStart, CancellationToken cancellationToken = default);
}
