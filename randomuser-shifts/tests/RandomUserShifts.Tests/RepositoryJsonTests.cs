using RandomUserShifts.Domain.Entities;
using RandomUserShifts.Infrastructure.Persistence;
using Xunit;

namespace RandomUserShifts.Tests;

public sealed class RepositoryJsonTests : IClassFixture<TempDirectoryFixture>
{
    private readonly TempDirectoryFixture _fixture;

    public RepositoryJsonTests(TempDirectoryFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task SaveAndLoad_PersistScheduleToDisk()
    {
        // Arrange: repository deve creare ./data/schedules/{yyyy-MM-dd}.json.
        var repository = new FileScheduleRepository(_fixture.DirectoryPath);
        var weekStart = new DateOnly(2025, 1, 13);
        var shifts = new List<Shift>
        {
            new(DayOfWeek.Monday, new TimeOnly(8, 0), new TimeOnly(16, 0), Guid.NewGuid()),
            new(DayOfWeek.Monday, new TimeOnly(16, 0), new TimeOnly(0, 0), Guid.NewGuid())
        };
        var schedule = new Schedule(weekStart, shifts, new[] { "rule" });

        // Act
        await repository.SaveAsync(schedule);
        var loaded = await repository.LoadAsync(weekStart);

        // Assert: rimane rosso finché TODO[5] non salva correttamente il JSON.
        var expectedPath = Path.Combine(_fixture.DirectoryPath, "2025-01-13.json");
        Assert.True(File.Exists(expectedPath), "Il file JSON atteso non è stato creato");
        Assert.NotNull(loaded);
        Assert.Equal(schedule.Shifts.Count, loaded!.Shifts.Count);
    }
}

public sealed class TempDirectoryFixture : IDisposable
{
    public string DirectoryPath { get; }

    public TempDirectoryFixture()
    {
        DirectoryPath = Path.Combine(Path.GetTempPath(), "RandomUserShiftsTests", Guid.NewGuid().ToString());
        Directory.CreateDirectory(DirectoryPath);
    }

    public void Dispose()
    {
        if (Directory.Exists(DirectoryPath))
        {
            Directory.Delete(DirectoryPath, recursive: true);
        }
    }
}
