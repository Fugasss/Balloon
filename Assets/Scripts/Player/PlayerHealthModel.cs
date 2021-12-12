using Core;
using UnityEngine;

namespace Player
{
    public class PlayerHealthModel : IDamageable
    {
        public int CurrentHealth { get; private set; }

        private readonly int m_DefaultHealth;
        private readonly IStartProvider m_StartProvider;
        private readonly PlayerHealthView m_View;

        public PlayerHealthModel(int defaultHealth, IStartProvider startProvider, PlayerHealthView view)
        {
            m_DefaultHealth = defaultHealth;
            m_StartProvider = startProvider;
            m_View = view;

            m_StartProvider.Started += ResetHealth;
        }

        public void TakeDamage(int damage)
        {
            CurrentHealth -= damage;

            m_View.SetHealthText((CurrentHealth < 0 ? 0 : CurrentHealth).ToString());

            if (CurrentHealth > 0) return;

            Die();
        }

        public void Die()
        {
            m_StartProvider.End();
        }
        
        public void AddHealth(int health)
        {
            CurrentHealth += Mathf.Abs(health);
            SetCurrentHealthText();
        }

        private void ResetHealth()
        {
            CurrentHealth = m_DefaultHealth;
            SetCurrentHealthText();
        }

        private void SetCurrentHealthText()
        {
            m_View.SetHealthText(CurrentHealth.ToString());
        }
        
    }
}