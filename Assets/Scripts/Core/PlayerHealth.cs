using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField] [Min(1)] private int m_DefaultHealth;

    [SerializeField] private Text m_HealthText;

    private void Awake()
    {
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

    public int CurrentHealth { get; private set; }

    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;

        m_HealthText.text = (CurrentHealth < 0 ? 0 : CurrentHealth).ToString();

        if (CurrentHealth > 0) return;

        Die();
    }

    public void Die()
    {
        Game.End();
    }

    public void AddHealth(int health)
    {
        CurrentHealth += Mathf.Abs(health);
        UpdateHealthText();
    }

    private void GameOnStarted()
    {
        m_HealthText.gameObject.SetActive(true);
        SetDefaultHealth();
    }

    private void GameOnEnded()
    {
        m_HealthText.gameObject.SetActive(false);
    }

    private void TakeDamage(Balloon balloon)
    {
        TakeDamage(balloon.CurrentHealth);
    }

    private void SetDefaultHealth()
    {
        CurrentHealth = m_DefaultHealth;
        UpdateHealthText();
    }

    private void UpdateHealthText()
    {
        m_HealthText.text = CurrentHealth.ToString();
    }
}