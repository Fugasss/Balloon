using FallingObject;
using Player;
using UnityEngine;
using Zenject;

namespace Spawners
{
    public class BalloonSpawner : Spawner<Balloon>
    {
        private Camera m_Camera;
        private PlayerHealth m_PlayerHealth;
        
        [Inject]
        private void Construct(PlayerHealth playerHealth)
        {
            m_PlayerHealth = playerHealth;
        }
        
        protected override void Awake()
        {
            base.Awake();

            m_Camera = Camera.main;
        }
        
        protected override Color CalculateColor(int health)
        {
            var h = Random.Range(0, 360);

            Color.RGBToHSV(m_Camera.backgroundColor, out var backH, out _, out _);

            if (h > backH + 45 || h < backH - 45)
                h += 25 * Mathf.RoundToInt(Mathf.Sign(Random.value));

            var s = Mathf.Clamp(health, 0.1f, MaxHealth * 0.8f) / MaxHealth;

            return Color.HSVToRGB(h / 360f, s, 1);
        }

        protected override void OutOfBoundsObjectCallback(Balloon fallingObjectBase)
        {
            base.OutOfBoundsObjectCallback(fallingObjectBase);
            m_PlayerHealth.TakeDamage(fallingObjectBase.CurrentHealth);
        }
    }
}