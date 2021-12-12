using Core;
using FallingObject;
using Score;
using Spawners;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Scoreboard
{
    public class Scoreboard : MonoBehaviour
    {
        [SerializeField] private Text m_CurrentScoreText;
        [SerializeField] private Text m_BestScoreText;

        private IStartProvider m_StartProvider;

        private ScoreboardModel m_Model;
        private ScoreboardView m_View;

        private IScoreChangerProvider m_ScoreChangerProvider;

        [Inject]
        private void Construct(IStartProvider startProvider, IScoreChangerProvider scoreChangerProvider)
        {
            m_StartProvider = startProvider;
            m_ScoreChangerProvider = scoreChangerProvider;
        }

        private void Awake()
        {
            m_Model = new ScoreboardModel();
            m_View = new ScoreboardView(m_BestScoreText, m_CurrentScoreText, m_Model);

            m_View.SetScoresTexts();
        }

        private void OnEnable()
        {
            m_StartProvider.Started += GameOnStarted;
            m_StartProvider.Ended += GameOnEnded;

            m_ScoreChangerProvider.Changed += AddScore;
        }

        private void OnDisable()
        {
            m_StartProvider.Started -= GameOnStarted;
            m_StartProvider.Ended -= GameOnEnded;

            m_ScoreChangerProvider.Changed -= AddScore;
        }

        private void GameOnStarted()
        {
            m_Model.Reset();
            m_View.OnGameBegin();
        }

        private void GameOnEnded()
        {
            m_Model.SaveBest();
            m_View.OnGameEnd();
        }

        private void AddScore(IScoreProvider scoreProvider)
        {
            if (!m_StartProvider.IsStarted) return;

            m_Model.AddScore(scoreProvider);
            m_View.UpdateScore(m_Model.CurrentResult);
        }
    }
}