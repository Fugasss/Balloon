using System;

public class BonusDamage : BonusBase
{
    private Clicker m_Clicker;

    protected override void AfterDie()
    {
        base.AfterDie();
        
        Use();
    }

    protected override void Awake()
    {
        base.Awake();

        m_Clicker ??= FindObjectOfType<Clicker>();
    }

    public override int GetScore()
    {
        return MaxHealth / 5;
    }

    public override void Use()
    {
        m_Clicker.AddDamage(1);
    }
}