using Core;
using Player;
using UnityEngine;

namespace FallingObject
{
    public class BonusHealth : BonusBase
    {
        private PlayerHealth m_Health;
        private int m_LastTakenDamage;
        
        internal void Construct(PlayerHealth playerHealth)
        {
            m_Health = playerHealth;
        }

        public override int GetScore()
        {
            return MaxHealth / 10;
        }

        public override void Use()
        {
            m_Health.AddHealth(Mathf.Clamp(m_LastTakenDamage,0, MaxHealth));
        }

        public override void TakeDamage(int damage)
        {
            base.TakeDamage(damage);
            m_LastTakenDamage = damage;
            Use();
        }
    }
}