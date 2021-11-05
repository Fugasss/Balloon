using System;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static float TimeFromStart { get; private set; } = 0f;
    public static bool IsPaused { get; private set; } = false;
    public static bool IsPlaying { get; private set; } = false;

    public static event Action Started = delegate { };
    public static event Action<bool> Paused = delegate { };
    public static event Action Ended = delegate { };
    
    private void Awake()
    {
        
    }

    public static void Begin()
    {
        TimeFromStart = 0f;
        IsPlaying = true;
        Started?.Invoke();
    }

    public static void Pause(bool pause)
    {
        IsPaused = pause;
        IsPlaying = !pause;
        Paused?.Invoke(pause);
    }

    public static void End()
    {
        IsPlaying = false;
        Ended?.Invoke();
    }

    private void Update()
    {
        if(IsPaused) return;

        TimeFromStart += Time.deltaTime;
    }
}
