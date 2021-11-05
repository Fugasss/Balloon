using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public int Health { get; private set; }

    [SerializeField, Min(1)] private int m_Health;

    [SerializeField] private Text m_HealthText;

    private void Awake()
    {
        Health = m_Health;
        m_HealthText.text = Health.ToString();
        m_HealthText.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        Game.Started += GameOnStarted;
        Game.Ended += GameOnEnded;
        Balloon.OutOfBounds += TakeDamage;
    }

    private void OnDisable()
    {
        Game.Started -= GameOnStarted;
        Game.Ended -= GameOnEnded;
        Balloon.OutOfBounds -= TakeDamage;
    }

    private void GameOnStarted()
    {
        m_HealthText.gameObject.SetActive(true);
    }

    private void GameOnEnded()
    {
        m_HealthText.gameObject.SetActive(false);
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;

        m_HealthText.text = (Health < 0 ? 0 : Health).ToString();

        if (Health <= 0)
        {
            Game.End();
        }
    }

    private void TakeDamage(Balloon balloon) => TakeDamage(balloon.Health);
}