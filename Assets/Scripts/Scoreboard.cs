using System;
using UnityEngine;
using UnityEngine.UI;

public class Scoreboard : MonoBehaviour
{
    public static int BestResult => PlayerPrefs.GetInt(nameof(BestResult));
    public static int CurrentResult { get; private set; }

    [SerializeField] private Text m_CurrentScoreText;
    [SerializeField] private Text m_BestScoreText;
    

    private void Awake()
    {
        m_BestScoreText.text = BestResult.ToString();
        m_CurrentScoreText.text = "0";
        m_CurrentScoreText.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        Game.Started += GameOnStarted;
        Game.Ended += GameOnEnded;
        
        Balloon.Destroy += AddScore;
    }

    private void OnDisable()
    {
        Game.Started -= GameOnStarted;
        Game.Ended -= GameOnEnded;
        
        Balloon.Destroy -= AddScore;
    }

    private void GameOnStarted()
    {
        m_CurrentScoreText.gameObject.SetActive(true);
    }
    private void GameOnEnded()
    {
        m_CurrentScoreText.gameObject.SetActive(false);
        
        if (CurrentResult > BestResult)
            PlayerPrefs.SetInt(nameof(BestResult), CurrentResult);
    }

    private void AddScore(Balloon balloon)
    {
        if (!Game.IsPlaying) return;

        CurrentResult += balloon.MaxHealth;
        m_CurrentScoreText.text = CurrentResult.ToString();
    }
}