using System;

namespace Score
{
    public class ScoreChanger : IScoreChangerProvider
    {
        public event Action<IScoreProvider> Changed;
        public void ChangeScore(IScoreProvider scoreProvider)
        {
            Changed?.Invoke(scoreProvider);
        }
    }
}