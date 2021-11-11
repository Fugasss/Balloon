public class BonusHealth : BonusBase
{
    private PlayerHealth m_Health;
    
    protected override void Awake()
    {
        base.Awake();
        
        m_Health ??= FindObjectOfType<PlayerHealth>();
    }

    public override int GetScore()
    {
        return MaxHealth / 10;
    }

    public override void Use()
    {
        m_Health.AddHealth(1);
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        Use();
    }
}