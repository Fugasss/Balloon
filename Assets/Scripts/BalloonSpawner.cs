using UnityEngine;

public class BalloonSpawner : Spawner<Balloon>
{
    private Camera m_Camera;
    

    protected override void Awake()
    {
        base.Awake();
        
        m_Camera = Camera.main;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
  
        Balloon.OutOfBounds += ReturnInPool;
        Balloon.Destroy += ReturnInPool;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        Balloon.OutOfBounds -= ReturnInPool;
        Balloon.Destroy -= ReturnInPool;
    }
    
    
    protected override Color CalculateColor(int health)
    {
        var h = Random.Range(0, 360);

        Color.RGBToHSV(m_Camera.backgroundColor, out var backH, out _, out _);

        if (h > backH + 45 || h < backH - 45)
            h += 25 * Mathf.RoundToInt(Mathf.Sign(Random.value));

        var s = Mathf.Clamp(health, 0.1f, MaxHealth * 0.8f) / MaxHealth;

        return Color.HSVToRGB(h / 360f, s, 1);
    }
}