using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    [SerializeField] private Button m_PauseButton;
    [SerializeField] private Button m_RestartButton;

    [SerializeField] private GameObject m_ButtonsContainer;

    private void OnEnable()
    {
        m_PauseButton.onClick.AddListener(Pause);
        m_RestartButton.onClick.AddListener(Game.Begin);
        Game.Started += ShowPauseButton;
    }

    private void OnDisable()
    {
        m_PauseButton.onClick.RemoveListener(Pause);
        m_RestartButton.onClick.RemoveListener(Game.Begin);
        Game.Started -= ShowPauseButton;
    }


    private void ShowPauseButton()
    {
        m_PauseButton.gameObject.SetActive(true);
    }

    private void Pause()
    {
        var gameIsPaused = Game.IsPaused;

        Game.Pause(!gameIsPaused);
        m_PauseButton.transform.DOLocalRotate(Vector3.forward * 90 * (gameIsPaused ? 0 : 1), 0.2f);
        m_ButtonsContainer.SetActive(!gameIsPaused);
    }
}