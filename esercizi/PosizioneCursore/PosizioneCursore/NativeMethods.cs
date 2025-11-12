using System.Runtime.InteropServices;

namespace PosizioneCursore;

internal static class NativeMethods
{
    /* Milestone 1: Creazione della struct POINT con layout compatibile */
    [StructLayout(LayoutKind.Sequential)]
    internal struct POINT
    {
        public int X;
        public int Y;
    }

    /* Milestone 2: Dichiarazione della funzione GetCursorPos con DllImport */
    [DllImport("user32.dll")]
    internal static extern bool GetCursorPos(out POINT lpPoint);
}
