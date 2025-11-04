using System.Collections.Generic;

namespace DesignPatternsTodo.Solutions;

/// <summary>
/// Soluzione del pattern State con gestione della traccia corrente e stato bloccato.
/// </summary>
public static class StatePatternSolution
{
    public static void Run()
    {
        var playlist = new List<string> { "Song A", "Song B", "Song C" };
        var player = new AudioPlayer(playlist);

        player.Play();
        player.Next();
        player.Pause();
        player.ToggleLock();
        player.Play();
        player.ToggleLock();
        player.Play();
        player.Next();
    }

    private interface IPlayerState
    {
        void Play(AudioPlayer player);
        void Pause(AudioPlayer player);
        void Next(AudioPlayer player);
        void ToggleLock(AudioPlayer player);
    }

    private sealed class AudioPlayer
    {
        private IPlayerState _state = new StoppedState();

        public AudioPlayer(IReadOnlyList<string> playlist)
        {
            Playlist = playlist;
        }

        public IReadOnlyList<string> Playlist { get; }
        public int CurrentTrackIndex { get; private set; }
        public string CurrentTrack => Playlist.Count == 0 ? "Nessun brano" : Playlist[CurrentTrackIndex];

        public void SetState(IPlayerState state)
        {
            _state = state;
            Console.WriteLine($"Stato attuale: {_state.GetType().Name}");
        }

        public void Play() => _state.Play(this);
        public void Pause() => _state.Pause(this);
        public void Next() => _state.Next(this);
        public void ToggleLock() => _state.ToggleLock(this);

        public void AdvanceTrack()
        {
            if (Playlist.Count == 0)
            {
                Console.WriteLine("Playlist vuota.");
                return;
            }

            CurrentTrackIndex = (CurrentTrackIndex + 1) % Playlist.Count;
            Console.WriteLine($"Riproduco la traccia: {CurrentTrack}");
        }
    }

    private sealed class StoppedState : IPlayerState
    {
        public void Play(AudioPlayer player)
        {
            Console.WriteLine($"Riproduzione avviata: {player.CurrentTrack}.");
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

        public void ToggleLock(AudioPlayer player)
        {
            Console.WriteLine("Player bloccato.");
            player.SetState(new LockedState(this));
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
            player.AdvanceTrack();
        }

        public void ToggleLock(AudioPlayer player)
        {
            Console.WriteLine("Player bloccato durante la riproduzione.");
            player.SetState(new LockedState(this));
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
            Console.WriteLine("Cambio traccia anche da pausa e rimango in pausa.");
            player.AdvanceTrack();
        }

        public void ToggleLock(AudioPlayer player)
        {
            Console.WriteLine("Player bloccato da pausa.");
            player.SetState(new LockedState(this));
        }
    }

    private sealed class LockedState : IPlayerState
    {
        private readonly IPlayerState _previousState;

        public LockedState(IPlayerState previousState)
        {
            _previousState = previousState;
        }

        public void Play(AudioPlayer player)
        {
            Console.WriteLine("Player bloccato: operazione ignorata.");
        }

        public void Pause(AudioPlayer player)
        {
            Console.WriteLine("Player bloccato: operazione ignorata.");
        }

        public void Next(AudioPlayer player)
        {
            Console.WriteLine("Player bloccato: non posso cambiare traccia.");
        }

        public void ToggleLock(AudioPlayer player)
        {
            Console.WriteLine("Sblocco del player.");
            player.SetState(_previousState);
        }
    }
}
