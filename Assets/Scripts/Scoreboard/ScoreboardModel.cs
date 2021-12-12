using FallingObject;
using Score;
using UnityEngine;

namespace Scoreboard
{
    public class ScoreboardModel
    {
        public int BestResult => PlayerPrefs.GetInt(nameof(BestResult));

        public int CurrentResult { get; private set; }

        public void Reset()
        {
            CurrentResult = 0;
        }

        public void SaveBest()
        {
            if (CurrentResult > BestResult)
                PlayerPrefs.SetInt(nameof(BestResult), CurrentResult);
        }

        public void AddScore(IScoreProvider scoreProvider)
        {
            CurrentResult += scoreProvider.GetScore();
        }
    }
}