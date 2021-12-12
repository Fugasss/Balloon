using System;

namespace Core
{
    public interface IStartProvider
    {
        public bool IsStarted { get; }
        public float TimeFromStart { get; }

        public event Action Started;
        public event Action Ended;
        public void Begin();
        public void End();
    }
}