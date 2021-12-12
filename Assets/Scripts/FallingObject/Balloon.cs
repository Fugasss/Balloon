using UnityEngine;

namespace FallingObject
{
    public class Balloon : FallingObjectBase
    {
        public override int GetScore()
        {
            return Mathf.CeilToInt((StartProvider.TimeFromStart + CurrentHealth + FallSpeed) * 0.2f);
        }
    }
}