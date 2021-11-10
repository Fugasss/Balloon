using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField, Min(1)]  private int m_Health;

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

    public void TakeHeal(int health)
    {
        m_Health += Mathf.Abs(health);
    }

    public void Die()
    {
        Game.End();
    }

    private void GameOnStarted()
    {
        m_HealthText.gameObject.SetActive(true);
        SetHealth();
    }

    private void GameOnEnded()
    {
        m_HealthText.gameObject.SetActive(false);
    }

    private void TakeDamage(Balloon balloon)
    {
        TakeDamage(balloon.CurrentHealth);
    }
    private void SetHealth()
    {
        CurrentHealth = m_Health;
        m_HealthText.text = CurrentHealth.ToString();
    }
}