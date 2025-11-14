using LanApi.Shared;

namespace LanApi.Server;

/// <summary>
/// Contratto semplice che incapsula le operazioni del dominio di monitoraggio device.
/// </summary>
public interface IDeviceRepository
{
    IEnumerable<DeviceDto> GetAll();

    DeviceDto? GetById(int id);

    DeviceDto Add(DeviceRegistrationRequest request);

    DeviceDto? UpdateStatus(int id, DeviceStatus status);
}
