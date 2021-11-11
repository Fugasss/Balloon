using UnityEngine;

public static class ParticleSystemExtension
{
    public static void SetMainColor(this ParticleSystem ps, Color color)
    {
        var main = ps.main;
        main.startColor = color;
    }
}