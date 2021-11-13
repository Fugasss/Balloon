using System.Linq;
using UnityEngine;

public class BombBonus : BonusBase
{
    private float m_ExplodeRadius;

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(transform.position, m_ExplodeRadius);
    }
#endif

    protected override void AfterDie()
    {
        base.AfterDie();

        Use();
    }

    public override void AfterInitialize()
    {
        m_ExplodeRadius = MaxHealth + 1;
    }

    public override int GetScore()
    {
        return CurrentHealth;
    }

    public override void Use()
    {
        var results = Physics2D.OverlapCircleAll(transform.position, m_ExplodeRadius);

        if (results.Length <= 0) return;

        foreach (var balloon in results.Select(x => x.GetComponent<Balloon>()))
            balloon.TakeDamage(balloon.CurrentHealth);
    }
}