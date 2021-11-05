using System;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class BalloonDestroyEffect : MonoBehaviour
{
    public ParticleSystem ParticleSystem { get; private set; }
    
    private void Awake()
    {
        ParticleSystem = GetComponent<ParticleSystem>();
    }

    private void OnParticleSystemStopped()
    {
        Balloon.ParticlesPool.ReturnInPool(this);
    }

    public void Play()
    {
        ParticleSystem.Play();
    }
    
    

}