using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public abstract class FallingObjectBase : MonoBehaviour, IDamageable
{
    protected Color Color;
    protected float FallSpeed;
    protected SpriteRenderer SpriteRenderer;

    protected int MaxHealth { get; private set; }

    protected virtual void Awake()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected virtual void Update()
    {
        if (Game.IsPaused) return;

        Move();
        CheckOutOfBounds();
    }

    public int CurrentHealth { get; private set; }

    public virtual void TakeDamage(int damage)
    {
        CurrentHealth -= damage;

        transform.DOPunchScale(Vector3.one * 0.5f, 0.1f, 15, 0.5f);

        if (CurrentHealth <= 0)
        {
            Die();
            AfterDie();
            
            AudioPlayer.Play("pop");

        }
        else
        {
            AudioPlayer.Play("hit");
        }
    }

    public virtual void Die()
    {
        var effect = Balloon.ParticlesPool.GetAvailable();
        effect.transform.position = transform.position;
        effect.ParticleSystem.SetMainColor(Color);
        effect.Play();
    }

    public void Initialize(int health, float fallSpeed, Color color)
    {
        FallSpeed = fallSpeed;
        CurrentHealth = health;
        MaxHealth = health;
        Color = color;

        SpriteRenderer.color = color;

        AfterInitialize();
    }

    public virtual void AfterInitialize()
    {
    }

    private void Move()
    {
        transform.Translate(Vector3.down * (FallSpeed * Time.deltaTime));
        FallSpeed += Time.deltaTime;
    }

    private void CheckOutOfBounds()
    {
        if (!Map.IsOutOfBounds(transform)) return;

        OnOutOfBounds();
    }

    protected virtual void AfterDie()
    {
    }

    protected abstract void OnOutOfBounds();
    public abstract int GetScore();
}