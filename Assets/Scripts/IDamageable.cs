internal interface IDamageable
{
    public int Health { get; }

    public void TakeDamage(int damage);

    public void Die();
}