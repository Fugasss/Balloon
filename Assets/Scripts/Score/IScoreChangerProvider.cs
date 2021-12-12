using System;
using FallingObject;
using UnityEngine;

namespace Score
{
    public interface IScoreChangerProvider
    {
        public event Action<IScoreProvider> Changed;
        public void ChangeScore(IScoreProvider scoreProvider);
    }
}