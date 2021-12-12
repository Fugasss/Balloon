using System;

namespace Core
{
    public interface IPauseProvider
    {
        public bool IsPaused { get; }

        public event Action<bool> Paused;

        public void Pause(bool pause);
    }
}