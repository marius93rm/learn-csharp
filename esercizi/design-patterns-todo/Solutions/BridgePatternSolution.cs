namespace DesignPatternsTodo.Solutions;

/// <summary>
/// Variante risolta del pattern Bridge con un controllo volume aggiuntivo.
/// L'astrazione espone un metodo virtuale che le sottoclassi possono
/// personalizzare mentre i dispositivi forniscono le funzionalità concrete.
/// </summary>
public static class BridgePatternSolution
{
    public static void Run()
    {
        IDevice tv = new Television();
        var remote = new BasicRemote(tv);

        remote.TogglePower();
        remote.ChannelUp();
        remote.ChangeVolume(5);

        Console.WriteLine();

        IDevice speaker = new SmartSpeaker();
        var advancedRemote = new BasicRemote(speaker);
        advancedRemote.TogglePower();
        advancedRemote.ChangeVolume(15);
        advancedRemote.ChangeVolume(-5);
    }

    private interface IDevice
    {
        bool IsEnabled { get; }
        int Channel { get; }
        int Volume { get; }
        void Enable();
        void Disable();
        void SetChannel(int channel);
        void SetVolume(int volume);
    }

    private sealed class Television : IDevice
    {
        public bool IsEnabled { get; private set; }
        public int Channel { get; private set; } = 1;
        public int Volume { get; private set; } = 10;

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

        public void SetVolume(int volume)
        {
            Volume = Math.Clamp(volume, 0, 100);
            Console.WriteLine($"Volume TV: {Volume}");
        }
    }

    private sealed class SmartSpeaker : IDevice
    {
        public bool IsEnabled { get; private set; }
        public int Channel => 0;
        public int Volume { get; private set; } = 20;

        public void Enable()
        {
            IsEnabled = true;
            Console.WriteLine("Smart speaker pronto a riprodurre musica.");
        }

        public void Disable()
        {
            IsEnabled = false;
            Console.WriteLine("Smart speaker in standby.");
        }

        public void SetChannel(int channel)
        {
            Console.WriteLine("Il diffusore non supporta i canali, richiesta ignorata.");
        }

        public void SetVolume(int volume)
        {
            Volume = Math.Clamp(volume, 0, 100);
            Console.WriteLine($"Volume speaker: {Volume}");
        }
    }

    private abstract class RemoteControl
    {
        protected RemoteControl(IDevice device) => Device = device;

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

        public void ChannelUp() => Device.SetChannel(Device.Channel + 1);

        public void ChannelDown() => Device.SetChannel(Math.Max(1, Device.Channel - 1));

        /// <summary>
        /// Metodo virtuale che incapsula un comportamento avanzato comune alle remote.
        /// Le sottoclassi possono sovrascriverlo per specializzare la logica.
        /// </summary>
        public virtual void ChangeVolume(int delta)
        {
            var desiredVolume = Device.Volume + delta;
            Device.SetVolume(desiredVolume);
        }
    }

    private sealed class BasicRemote : RemoteControl
    {
        public BasicRemote(IDevice device) : base(device)
        {
        }

        public override void ChangeVolume(int delta)
        {
            // Eseguiamo la logica base ma aggiungiamo un messaggio descrittivo
            // per tracciare l'operazione extra introdotta dalla soluzione.
            base.ChangeVolume(delta);
            Console.WriteLine($"[BasicRemote] Volume regolato di {delta} unità.");
        }
    }
}
