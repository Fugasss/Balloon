using System;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class Balloon : MonoBehaviour, IDamageable
{
    public new static event Action<Balloon> Destroy = delegate { };
    public static event Action<Balloon> OutOfBounds = delegate { };
    public int Health { get; private set; }
    public int MaxHealth { get; private set; }

    private float m_FallSpeed;
    private Color m_Color;

    private SpriteRenderer m_SpriteRenderer;
    [SerializeField] private BalloonDestroyEffect m_DestroyEffect;

    public static ObjectPool<BalloonDestroyEffect> ParticlesPool { get; private set; }

    private void Awake()
    {
        ParticlesPool ??= new ObjectPool<BalloonDestroyEffect>(m_DestroyEffect, 50);

        m_SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Initialize(int health, float fallSpeed, Color color)
    {
        m_FallSpeed = fallSpeed;
        Health = health;
        MaxHealth = health;
        m_Color = color;

        m_SpriteRenderer.color = color;
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;

        transform.DOPunchScale(Vector3.one * 0.5f, 0.1f, 15, 0.5f);

        if (Health > 0) return;

        var effect = ParticlesPool.GetAvailable();
        effect.transform.position = transform.position;
        effect.ParticleSystem.SetMainColor(m_Color);
        effect.Play();

        Destroy?.Invoke(this);
    }

    private void Update()
    {
        Move();

        CheckOutOfBounds();
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
}