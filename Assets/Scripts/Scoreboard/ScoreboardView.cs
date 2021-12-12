using Core;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Scoreboard
{
    public class ScoreboardView
    {
        private readonly Text m_CurrentScoreText;
        private readonly Text m_BestScoreText;
        private readonly ScoreboardModel m_Model;

        public ScoreboardView(Text bestScoreText, Text currentScoreText, ScoreboardModel model)
        {
            m_BestScoreText = bestScoreText;
            m_CurrentScoreText = currentScoreText;
            m_Model = model;
        }

        public void OnGameBegin()
        {
            m_CurrentScoreText.gameObject.SetActive(true);
        }

        public void OnGameEnd()
        {
            SetScoresTexts();
        }

        public void UpdateScore(int currentScore)
        {
            m_CurrentScoreText.text = currentScore.ToString();
            
            TryPunchText(currentScore);
        }

        private void TryPunchText(int currentScore)
        {
            var textTransform = m_CurrentScoreText.transform;

            if (DOTween.IsTweening(textTransform)) return;

            textTransform.DOPunchRotation(Vector3.forward * (12f * Random.Range(-1f, 1f)), 0.1f,
                Mathf.Clamp(currentScore / 10, 0, 10));
            textTransform.DOPunchScale(Vector3.one * (0.1f * Mathf.Clamp(currentScore * 0.02f, 0, 5)), 0.2f,
                currentScore / 10);
        }
        
        public void SetScoresTexts()
        {
            m_BestScoreText.text = m_Model.BestResult.ToString();
            m_CurrentScoreText.text = "0";
            m_CurrentScoreText.gameObject.SetActive(false);
        }
    }
}