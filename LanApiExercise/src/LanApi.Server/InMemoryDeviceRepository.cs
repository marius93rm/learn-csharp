using System.Collections.Concurrent;
using LanApi.Shared;

namespace LanApi.Server;

/// <summary>
/// Implementazione in memoria pensata per la didattica: non usa un database reale ma tiene tutto in RAM.
/// </summary>
public class InMemoryDeviceRepository : IDeviceRepository
{
    private readonly ConcurrentDictionary<int, DeviceDto> _storage = new();
    private int _currentId;

    public InMemoryDeviceRepository()
    {
        // Inseriamo un paio di device di esempio per rendere l'esercizio immediatamente esplorabile.
        Add(new DeviceRegistrationRequest { Name = "Sensore Temperatura", IpAddress = "192.168.1.21" });
        Add(new DeviceRegistrationRequest { Name = "Access Point Ufficio", IpAddress = "192.168.1.3" });
    }

    public DeviceDto Add(DeviceRegistrationRequest request)
    {
        var id = Interlocked.Increment(ref _currentId);
        var device = new DeviceDto
        {
            Id = id,
            Name = request.Name.Trim(),
            IpAddress = request.IpAddress.Trim(),
            Status = DeviceStatus.Unknown,
            LastSeenUtc = null
        };

        _storage[id] = device;
        return device;
    }

    public IEnumerable<DeviceDto> GetAll()
    {
        return _storage.Values
            .OrderBy(d => d.Id)
            .ToList();
    }

    public DeviceDto? GetById(int id)
    {
        return _storage.TryGetValue(id, out var device) ? device : null;
    }

    public DeviceDto? UpdateStatus(int id, DeviceStatus status)
    {
        if (!_storage.TryGetValue(id, out var existing))
        {
            return null;
        }

        var updated = existing with
        {
            Status = status,
            LastSeenUtc = DateTime.UtcNow
        };

        _storage[id] = updated;
        return updated;
    }
}
