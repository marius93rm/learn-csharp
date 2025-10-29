namespace DesignPatternsTodo.Patterns;

/// <summary>
/// Esempio didattico del pattern Bridge.
/// Completa i TODO per espandere l'interazione tra astrazione e implementazione.
/// </summary>
public static class BridgePattern
{
    public static void Run()
    {
        IDevice tv = new Television();
        var remote = new BasicRemote(tv);

        remote.TogglePower();
        remote.ChannelUp();
        remote.ChannelUp();

        Console.WriteLine("Aggiungi nuovi dispositivi o funzionalità alla remote completando i TODO.\n");
    }

    private interface IDevice
    {
        bool IsEnabled { get; }
        int Channel { get; }
        void Enable();
        void Disable();
        void SetChannel(int channel);
    }

    private sealed class Television : IDevice
    {
        public bool IsEnabled { get; private set; }
        public int Channel { get; private set; } = 1;

        public void Enable()
        {
            IsEnabled = true;
            Console.WriteLine("Televisore acceso.");
        }

        public void Disable()
        {
            IsEnabled = false;
            Console.WriteLine("Televisore spento.");
        }

        public void SetChannel(int channel)
        {
            Channel = channel;
            Console.WriteLine($"Canale impostato su {Channel}.");
        }
    }

    private abstract class RemoteControl
    {
        protected RemoteControl(IDevice device)
        {
            Device = device;
        }

        protected IDevice Device { get; }

        public void TogglePower()
        {
            if (Device.IsEnabled)
            {
                Device.Disable();
            }
            else
            {
                Device.Enable();
            }
        }

        public void ChannelUp()
        {
            Device.SetChannel(Device.Channel + 1);
        }

        public void ChannelDown()
        {
            Device.SetChannel(Math.Max(1, Device.Channel - 1));
        }

        // TODO: aggiungi un metodo astratto o virtuale per controlli avanzati (es. Volume).
    }

    private sealed class BasicRemote : RemoteControl
    {
        public BasicRemote(IDevice device) : base(device)
        {
        }

        // TODO: implementa qui il nuovo comportamento richiesto dalla TODO dell'astrazione.
    }
}
