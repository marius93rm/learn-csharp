using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace MonitorStatoSistema;

internal static class NativeMethods
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    private struct MEMORYSTATUSEX
    {
        public uint dwLength;
        public uint dwMemoryLoad;
        public ulong ullTotalPhys;
        public ulong ullAvailPhys;
        public ulong ullTotalPageFile;
        public ulong ullAvailPageFile;
        public ulong ullTotalVirtual;
        public ulong ullAvailVirtual;
        public ulong ullAvailExtendedVirtual;
    }

    internal readonly record struct MemoryStatus(ulong AvailablePhysicalMemory, ulong TotalPhysicalMemory);

    [DllImport("kernel32.dll")]
    internal static extern ulong GetTickCount64();

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool GlobalMemoryStatusEx(ref MEMORYSTATUSEX lpBuffer);

    internal static MemoryStatus GetMemoryStatus()
    {
        var status = new MEMORYSTATUSEX
        {
            dwLength = (uint)Marshal.SizeOf<MEMORYSTATUSEX>()
        };

        if (!GlobalMemoryStatusEx(ref status))
        {
            throw new Win32Exception(Marshal.GetLastWin32Error(), "Chiamata a GlobalMemoryStatusEx fallita");
        }

        return new MemoryStatus(status.ullAvailPhys, status.ullTotalPhys);
    }
}
