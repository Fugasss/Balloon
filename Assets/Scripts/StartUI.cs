using Core;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class StartUI : MonoBehaviour
{
    [SerializeField] private Button m_StartButton;

    private GameObject m_Canvas;

    private IStartProvider m_StartProvider;
    
    [Inject]
    private void Construct(IStartProvider startProvider)
    {
        m_StartProvider = startProvider;
    }
    
    private void Awake()
    {
        m_Canvas = GetComponentInChildren<Canvas>().gameObject;
    }

    private void OnEnable()
    {
        m_StartButton.onClick.AddListener(m_StartProvider.Begin);

        m_StartProvider.Started += DisableCanvas;
        m_StartProvider.Ended += EnableCanvas;
    }

    private void OnDisable()
    {
        m_StartButton.onClick.RemoveListener(m_StartProvider.Begin);

        m_StartProvider.Started -= DisableCanvas;
        m_StartProvider.Ended -= EnableCanvas;
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