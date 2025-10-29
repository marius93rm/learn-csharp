namespace DesignPatternsTodo.Patterns;

/// <summary>
/// Esempio didattico del pattern State.
/// Completa i TODO per introdurre nuovi stati o transizioni.
/// </summary>
public static class StatePattern
{
    public static void Run()
    {
        var player = new AudioPlayer();
        player.Play();
        player.Next();
        player.Pause();
        player.Play();

        Console.WriteLine("\nEstendi gli stati completando i TODO per scenari più complessi.\n");
    }

    private interface IPlayerState
    {
        void Play(AudioPlayer player);
        void Pause(AudioPlayer player);
        void Next(AudioPlayer player);
    }

    private sealed class AudioPlayer
    {
        private IPlayerState _state = new StoppedState();

        public void SetState(IPlayerState state)
        {
            _state = state;
            Console.WriteLine($"Stato attuale: {_state.GetType().Name}");
        }

        public void Play() => _state.Play(this);
        public void Pause() => _state.Pause(this);
        public void Next() => _state.Next(this);

        // TODO: aggiungi qui eventuali proprietà (es. traccia corrente) da gestire nei diversi stati.
    }

    private sealed class StoppedState : IPlayerState
    {
        public void Play(AudioPlayer player)
        {
            Console.WriteLine("Riproduzione avviata.");
            player.SetState(new PlayingState());
        }

        public void Pause(AudioPlayer player)
        {
            Console.WriteLine("Non puoi mettere in pausa: il player è fermo.");
        }

        public void Next(AudioPlayer player)
        {
            Console.WriteLine("Avvia la riproduzione per passare al prossimo brano.");
        }
    }

    private sealed class PlayingState : IPlayerState
    {
        public void Play(AudioPlayer player)
        {
            Console.WriteLine("Già in riproduzione.");
        }

        public void Pause(AudioPlayer player)
        {
            Console.WriteLine("Riproduzione in pausa.");
            player.SetState(new PausedState());
        }

        public void Next(AudioPlayer player)
        {
            Console.WriteLine("Salto al brano successivo.");
            // TODO: aggiorna qui la traccia corrente quando aggiungerai la proprietà corrispondente.
        }
    }

    private sealed class PausedState : IPlayerState
    {
        public void Play(AudioPlayer player)
        {
            Console.WriteLine("Riprendo la riproduzione.");
            player.SetState(new PlayingState());
        }

        public void Pause(AudioPlayer player)
        {
            Console.WriteLine("Già in pausa.");
        }

        public void Next(AudioPlayer player)
        {
            Console.WriteLine("Non puoi passare al prossimo mentre sei in pausa.");
            // TODO: valuta se permettere comunque il cambio traccia qui.
        }
    }
}
