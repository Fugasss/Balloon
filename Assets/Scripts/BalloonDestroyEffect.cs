using System;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class BalloonDestroyEffect : MonoBehaviour
{
    public ParticleSystem ParticleSystem { get; private set; }

    private Action<BalloonDestroyEffect> m_EndCallback;
    private void Awake()
    {
        ParticleSystem = GetComponent<ParticleSystem>();
    }

    private void OnParticleSystemStopped()
    {
        m_EndCallback?.Invoke(this);
    }

    public void Play(Action<BalloonDestroyEffect> endCallback = null)
    {
        m_EndCallback = endCallback;
        ParticleSystem.Play();
    }
}