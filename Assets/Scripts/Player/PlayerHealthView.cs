using Core;
using UnityEngine.UI;
using Zenject;

namespace Player
{
    public class PlayerHealthView : ILateDisposable
    {
        private readonly Text m_HealthText;
        private readonly IStartProvider m_StartProvider;

        public PlayerHealthView(Text healthText, IStartProvider startProvider)
        {
            m_HealthText = healthText;
            m_StartProvider = startProvider;
            
            m_StartProvider.Started += GameOnStarted;
            m_StartProvider.Ended += GameOnEnded;
        }

        private void GameOnStarted()
        {
            m_HealthText.gameObject.SetActive(true);
        }
        private void GameOnEnded()
        {
            m_HealthText.gameObject.SetActive(false);
        }
        
        public void SetHealthText(string text)
        {
            m_HealthText.text = text;
        }

        public void LateDispose()
        {
            m_StartProvider.Started -= GameOnStarted;
            m_StartProvider.Ended -= GameOnEnded;
        }
    }
}