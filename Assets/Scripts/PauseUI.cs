using Core;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PauseUI : MonoBehaviour
{
    [SerializeField] private Button m_PauseButton;
    [SerializeField] private Button m_RestartButton;

    [SerializeField] private GameObject m_ButtonsContainer;

    private IStartProvider m_StartProvider;
    private IPauseProvider m_PauseProvider;
    
    [Inject]
    private void Construct(IStartProvider startProvider, IPauseProvider pauseProvider)
    {
        m_StartProvider = startProvider;
        m_PauseProvider = pauseProvider;
    }
    
    private void OnEnable()
    {
        m_PauseButton.onClick.AddListener(Pause);
        m_RestartButton.onClick.AddListener(Restart);
        m_StartProvider.Started += ShowPauseButton;
    }

    private void OnDisable()
    {
        m_PauseButton.onClick.RemoveListener(Pause);
        m_RestartButton.onClick.RemoveListener(Restart);
        m_StartProvider.Started -= ShowPauseButton;
    }
    
    private void ShowPauseButton()
    {
        m_PauseButton.gameObject.SetActive(true);
    }

    private void Pause()
    {
        var gameIsPaused = m_PauseProvider.IsPaused;

        m_PauseProvider.Pause(!gameIsPaused);
        m_PauseButton.transform.DOLocalRotate(Vector3.forward * 90 * (gameIsPaused ? 0 : 1), 0.2f);
        m_ButtonsContainer.SetActive(!gameIsPaused);
    }

    private void Restart()
    {
        Pause();
        m_StartProvider.End();
        m_StartProvider.Begin();
    }
}