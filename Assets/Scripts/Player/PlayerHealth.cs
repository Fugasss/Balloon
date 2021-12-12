using Core;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Player
{
    public class PlayerHealth : MonoBehaviour
    {
        [SerializeField] [Min(1)] private int m_DefaultHealth;

        [SerializeField] private Text m_HealthText;

        private IStartProvider m_StartProvider;
        
        private PlayerHealthModel m_Model;
        private PlayerHealthView m_View;

        [Inject]
        private void Construct(IStartProvider startProvider)
        {
            m_StartProvider = startProvider;

            m_View = new PlayerHealthView(m_HealthText, startProvider);
            m_Model = new PlayerHealthModel(m_DefaultHealth, m_StartProvider, m_View);
        }

        private void Awake()
        {
            m_HealthText.gameObject.SetActive(false);
        }

        public void TakeDamage(int damage)
        {
            m_Model.TakeDamage(damage);
        }

        public void AddHealth(int health)
        {
            m_Model.AddHealth(health);
        }
    }
}