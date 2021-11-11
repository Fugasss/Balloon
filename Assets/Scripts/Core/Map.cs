using UnityEngine;

public static class Map
{
    private static Camera m_Camera;
    public static Vector2 UpperBound { get; private set; }
    public static Vector2 RightBound { get; private set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Awake()
    {
        m_Camera = Camera.main;

        UpperBound = m_Camera.ScreenToWorldPoint(new Vector2(m_Camera.pixelWidth / 2, m_Camera.pixelHeight + 50));
        RightBound = m_Camera.ScreenToWorldPoint(new Vector2(m_Camera.pixelWidth - 20, m_Camera.pixelHeight / 2));
    }

    public static bool IsOutOfBounds(Transform obj)
    {
        return obj.position.y <= -UpperBound.y;
    }

    public static Vector2 GetRandomUpperPosition()
    {
        return UpperBound + RightBound * Random.Range(-1f, 1f);
    }
}