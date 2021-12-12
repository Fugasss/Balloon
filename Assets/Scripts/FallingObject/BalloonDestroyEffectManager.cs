using Core;
using Extensions;
using UnityEngine;

namespace FallingObject
{
    public class BalloonDestroyEffectManager : MonoBehaviour, IObjectPoolContainer<BalloonDestroyEffect>
    {
        [SerializeField] private BalloonDestroyEffect m_BalloonDestroyEffectPrefab;

        public ObjectPool<BalloonDestroyEffect> ObjectPool { get; private set; }

        private void Awake()
        {
            ObjectPool = new ObjectPool<BalloonDestroyEffect>(m_BalloonDestroyEffectPrefab, 20);
        }

        public void Play(Vector2 position, Color color)
        {
            var effect = ObjectPool.GetAvailable();
            effect.transform.position = position;
            effect.ParticleSystem.SetMainColor(color);

            effect.Play(destroyEffect => ObjectPool.ReturnInPool(destroyEffect));
        }
    }
}