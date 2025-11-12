using System;

namespace MonitorStatoSistema.Models;

internal readonly record struct SystemStats(
    TimeSpan Uptime,
    ulong AvailablePhysicalMemory,
    ulong TotalPhysicalMemory,
    int ProcessCount,
    double? CpuUsagePercentage);
