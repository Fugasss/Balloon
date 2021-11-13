using System;
using UnityEngine;

public class Balloon : FallingObjectBase
{
    [SerializeField] private BalloonDestroyEffect m_DestroyEffect;

    public static ObjectPool<BalloonDestroyEffect> ParticlesPool { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        ParticlesPool ??= new ObjectPool<BalloonDestroyEffect>(m_DestroyEffect, 20);
    }

    public new static event Action<Balloon> Destroy = delegate { };
    public static event Action<Balloon> OutOfBounds = delegate { };

    public override void Die()
    {
        var effect = ParticlesPool.GetAvailable();
        effect.transform.position = transform.position;
        effect.ParticleSystem.SetMainColor(Color);
        effect.Play();
    }

    protected override void AfterDie()
    {
        base.AfterDie();
        
        Destroy?.Invoke(this);
    }

    protected override void OnOutOfBounds()
    {
        OutOfBounds?.Invoke(this);
    }

    public override int GetScore()
    {
        return Mathf.CeilToInt((Game.TimeFromStart + CurrentHealth + FallSpeed) * 0.2f);
    }
}