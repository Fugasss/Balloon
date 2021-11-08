using System;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class Balloon : MonoBehaviour, IDamageable
{
    [SerializeField] private BalloonDestroyEffect m_DestroyEffect;
    private Color m_Color;

    private float m_FallSpeed;

    private SpriteRenderer m_SpriteRenderer;
    public int MaxHealth { get; private set; }

    public static ObjectPool<BalloonDestroyEffect> ParticlesPool { get; private set; }

    private void Awake()
    {
        ParticlesPool ??= new ObjectPool<BalloonDestroyEffect>(m_DestroyEffect, 50);

        m_SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (Game.IsPaused) return;

        Move();

        CheckOutOfBounds();
    }

    public int Health { get; private set; }

    public void TakeDamage(int damage)
    {
        Health -= damage;

        transform.DOPunchScale(Vector3.one * 0.5f, 0.1f, 15, 0.5f);

        if (Health > 0) return;

        Die();

        Destroy?.Invoke(this);
    }

    public void Die()
    {
        var effect = ParticlesPool.GetAvailable();
        effect.transform.position = transform.position;
        effect.ParticleSystem.SetMainColor(m_Color);
        effect.Play();
    }

    public new static event Action<Balloon> Destroy = delegate { };
    public static event Action<Balloon> OutOfBounds = delegate { };


    public void Initialize(int health, float fallSpeed, Color color)
    {
        m_FallSpeed = fallSpeed;
        Health = health;
        MaxHealth = health;
        m_Color = color;

        m_SpriteRenderer.color = color;
    }

    private void Move()
    {
        transform.Translate(Vector3.down * (m_FallSpeed * Time.deltaTime));
        m_FallSpeed += Time.deltaTime;
    }

    private void CheckOutOfBounds()
    {
        if (!Map.IsOutOfBounds(transform)) return;

        OutOfBounds?.Invoke(this);
    }


    public int GetScore()
    {
        return Mathf.CeilToInt((Game.TimeFromStart + Health + m_FallSpeed) * 0.2f);
    }
}