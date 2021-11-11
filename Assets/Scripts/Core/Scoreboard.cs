using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Scoreboard : MonoBehaviour
{
    [SerializeField] private Text m_CurrentScoreText;
    [SerializeField] private Text m_BestScoreText;
    public static int BestResult => PlayerPrefs.GetInt(nameof(BestResult));
    public static int CurrentResult { get; private set; }


    private void Awake()
    {
        SetScoresTexts();
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
        CurrentResult = 0;
    }

    private void GameOnEnded()
    {
        if (CurrentResult > BestResult)
            PlayerPrefs.SetInt(nameof(BestResult), CurrentResult);

        SetScoresTexts();
    }

    private void AddScore(Balloon balloon)
    {
        if (!Game.IsPlaying) return;

        CurrentResult += balloon.GetScore();
        m_CurrentScoreText.text = CurrentResult.ToString();

        var textTransform = m_CurrentScoreText.transform;

        if (DOTween.IsTweening(textTransform)) return;

        textTransform.DOPunchRotation(Vector3.forward * (12f * Random.Range(-1f, 1f)), 0.1f,
            Mathf.Clamp(CurrentResult / 10, 0, 10));
        textTransform.DOPunchScale(Vector3.one * (0.1f * Mathf.Clamp(CurrentResult * 0.02f, 0, 5)), 0.2f,
            CurrentResult / 10);
    }

    private void SetScoresTexts()
    {
        m_BestScoreText.text = BestResult.ToString();
        m_CurrentScoreText.text = "0";
        m_CurrentScoreText.gameObject.SetActive(false);
    }
}