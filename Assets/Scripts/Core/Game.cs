using System;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static float TimeFromStart { get; private set; }
    public static bool IsPaused { get; private set; }
    public static bool IsPlaying { get; private set; }

    private void Update()
    {
        if (IsPaused || !IsPlaying) return;

        TimeFromStart += Time.deltaTime;
    }

    public static event Action Started = delegate { };
    public static event Action<bool> Paused = delegate { };
    public static event Action Ended = delegate { };

    public static void Begin()
    {
        TimeFromStart = 0f;
        IsPlaying = true;
        Started?.Invoke();
    }

    public static void Pause(bool pause)
    {
        IsPaused = pause;
        Paused?.Invoke(pause);
    }

    public static void End()
    {
        IsPlaying = false;
        Ended?.Invoke();
    }
}