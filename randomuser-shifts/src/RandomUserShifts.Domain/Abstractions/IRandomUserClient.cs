using RandomUserShifts.Domain.Abstractions;

namespace RandomUserShifts.Domain.Abstractions;

public interface IRandomUserClient
{
    Task<IReadOnlyList<PersonDto>> GetRandomUsersAsync(int count, CancellationToken cancellationToken = default);
}
