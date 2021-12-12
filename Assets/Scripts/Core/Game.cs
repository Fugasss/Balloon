using System;
using UnityEngine;

namespace Core
{
    public class Game : MonoBehaviour, IPauseProvider, IStartProvider
    {
        public float TimeFromStart { get; private set; }
        public bool IsPaused { get; private set; }
        public bool IsStarted { get; private set; }

        private void Update()
        {
            if (IsPaused || !IsStarted) return;

            TimeFromStart += Time.deltaTime;
        }

        public event Action Started = delegate { };
        public event Action<bool> Paused = delegate { };
        public event Action Ended = delegate { };

        public void Begin()
        {
            TimeFromStart = 0f;
            IsStarted = true;
            Started?.Invoke();
        }

        public void Pause(bool pause)
        {
            IsPaused = pause;
            Paused?.Invoke(pause);
        }

        public void End()
        {
            IsStarted = false;
            Ended?.Invoke();
        }
    }
}