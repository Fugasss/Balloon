using UnityEngine;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    [SerializeField] private Button m_ResumeButton;
    [SerializeField] private Button m_RestartButton;

    private void OnEnable()
    {
        m_ResumeButton.onClick.AddListener(UnPause);
        m_RestartButton.onClick.AddListener(Game.Begin);
    }

    private void OnDisable()
    {
        m_ResumeButton.onClick.RemoveListener(UnPause);
        m_RestartButton.onClick.RemoveListener(Game.Begin);

    }

    private static void UnPause()
    {
        Game.Pause(false);
    }
}