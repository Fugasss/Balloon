using UnityEngine;
using UnityEngine.UI;

public class StartUI : MonoBehaviour
{
    [SerializeField] private Button m_StartButton;

    private GameObject m_Canvas;

    private void Awake()
    {
        m_Canvas = GetComponentInChildren<Canvas>().gameObject;
    }

    private void OnEnable()
    {
        m_StartButton.onClick.AddListener(Game.Begin);

        Game.Started += DisableCanvas;
        Game.Ended += EnableCanvas;
    }

    private void OnDisable()
    {
        m_StartButton.onClick.RemoveListener(Game.Begin);

        Game.Started -= DisableCanvas;
        Game.Ended -= EnableCanvas;
    }

    private void EnableCanvas()
    {
        m_Canvas.SetActive(true);
    }

    private void DisableCanvas()
    {
        m_Canvas.SetActive(false);
    }
}