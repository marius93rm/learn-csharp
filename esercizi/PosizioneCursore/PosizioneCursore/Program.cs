using System;
using System.Threading;

namespace PosizioneCursore;

internal static class Program
{
    private static void Main()
    {
        Console.WriteLine("Premi un tasto per terminare l'applicazione...");

        while (true)
        {
            /* Milestone 3: Lettura della posizione del cursore ogni 500ms */
            if (!NativeMethods.GetCursorPos(out var point))
            {
                Console.WriteLine("Impossibile leggere la posizione del cursore.");
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Premi un tasto per terminare l'applicazione...");
                Console.WriteLine($"Posizione del cursore: X = {point.X}, Y = {point.Y}");
            }

            /* Milestone 4: Terminazione del loop con Console.ReadKey() */
            if (Console.KeyAvailable)
            {
                Console.ReadKey(intercept: true);
                break;
            }

            Thread.Sleep(500);
        }
    }
}
