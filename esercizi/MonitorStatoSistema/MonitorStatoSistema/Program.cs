using System;
using System.Diagnostics;
using System.Threading;
using MonitorStatoSistema.Models;

namespace MonitorStatoSistema;

internal class Program
{
    private static void Main()
    {
        PerformanceCounter? cpuCounter = null;
        string? cpuWarning = null;

        if (OperatingSystem.IsWindows())
        {
            try
            {
                cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
                _ = cpuCounter.NextValue();
            }
            catch (Exception ex)
            {
                cpuWarning = $"Impossibile inizializzare PerformanceCounter: {ex.Message}";
                cpuCounter?.Dispose();
                cpuCounter = null;
            }
        }

        while (true)
        {
            var stats = CollectStats(cpuCounter);
            RenderDashboard(stats, cpuCounter is not null, cpuWarning);

            if (Console.KeyAvailable)
            {
                /* Milestone 5: Interrompi con Console.ReadKey() */
                _ = Console.ReadKey(intercept: true);
                break;
            }

            Thread.Sleep(1000);
        }

        cpuCounter?.Dispose();
    }

    private static SystemStats CollectStats(PerformanceCounter? cpuCounter)
    {
        /* Milestone 1: Importa GetTickCount64 da kernel32.dll e calcola uptime */
        var uptimeMilliseconds = NativeMethods.GetTickCount64();
        var uptime = TimeSpan.FromMilliseconds(uptimeMilliseconds);

        /* Milestone 2: Importa GlobalMemoryStatusEx e visualizza RAM usata/totale */
        var memoryStatus = NativeMethods.GetMemoryStatus();
        var totalRam = memoryStatus.TotalPhysicalMemory;
        var availableRam = memoryStatus.AvailablePhysicalMemory;

        /* Milestone 3: Conta i processi attivi con Process.GetProcesses() */
        var processCount = Process.GetProcesses().Length;

        double? cpuUsage = null;
        if (cpuCounter is not null)
        {
            try
            {
                /* (Opzionale) Milestone 6: Visualizza utilizzo CPU */
                cpuUsage = Math.Round(cpuCounter.NextValue(), 2);
            }
            catch (InvalidOperationException)
            {
                cpuUsage = null;
            }
        }

        return new SystemStats(uptime, availableRam, totalRam, processCount, cpuUsage);
    }

    private static void RenderDashboard(SystemStats stats, bool showCpu, string? cpuWarning)
    {
        /* Milestone 4: Mostra i dati in console ogni secondo con Console.Clear() */
        Console.Clear();
        Console.WriteLine("==== Monitor Stato Sistema ====");
        Console.WriteLine($"Uptime sistema: {FormatTimeSpan(stats.Uptime)}");
        Console.WriteLine($"RAM disponibile: {FormatBytes(stats.AvailablePhysicalMemory)} / {FormatBytes(stats.TotalPhysicalMemory)}");
        Console.WriteLine($"Processi attivi: {stats.ProcessCount}");

        if (showCpu)
        {
            var cpuText = stats.CpuUsagePercentage.HasValue
                ? $"{stats.CpuUsagePercentage.Value:0.00}%"
                : "N/D";
            Console.WriteLine($"Utilizzo CPU: {cpuText}");
        }
        else
        {
            Console.WriteLine("Utilizzo CPU: funzione disponibile solo su Windows");
        }

        if (!string.IsNullOrEmpty(cpuWarning))
        {
            Console.WriteLine();
            Console.WriteLine($"Attenzione: {cpuWarning}");
        }

        Console.WriteLine();
        Console.WriteLine("Premi un tasto per uscire...");
    }

    private static string FormatBytes(ulong bytes)
    {
        const double scale = 1024;
        var units = new[] { "B", "KB", "MB", "GB", "TB" };
        var value = (double)bytes;
        var unitIndex = 0;

        while (value >= scale && unitIndex < units.Length - 1)
        {
            value /= scale;
            unitIndex++;
        }

        return $"{value:0.##} {units[unitIndex]}";
    }

    private static string FormatTimeSpan(TimeSpan timeSpan)
    {
        return $"{(int)timeSpan.TotalDays}d {timeSpan.Hours:00}h:{timeSpan.Minutes:00}m:{timeSpan.Seconds:00}s";
    }
}
