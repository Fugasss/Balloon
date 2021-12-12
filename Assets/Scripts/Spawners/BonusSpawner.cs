using Core;
using FallingObject;
using Player;
using UnityEngine;
using Zenject;

namespace Spawners
{
    public class BonusSpawner : Spawner<BonusBase>
    {
        private Clicker m_Clicker;
        private PlayerHealth m_PlayerHealth;
        
        [Inject]
        private void Construct(Clicker clicker, PlayerHealth playerHealth)
        {
            m_Clicker = clicker;
            m_PlayerHealth = playerHealth;
        }
        protected override void Awake()
        {
            base.Awake();
            
            ObjectPool.InvokeForAll(x =>
            {
                switch (x)
                {
                    case BonusDamage bonusDamage:
                        bonusDamage.Construct(m_Clicker);
                        break;
                    case BonusHealth bonusHealth:
                        bonusHealth.Construct(m_PlayerHealth);
                        break;
                }
            });
        }

        protected override Color CalculateColor(int health)
        {
            return Color.white;
        }
    }
}