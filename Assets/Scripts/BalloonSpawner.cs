using System.Collections;
using UnityEngine;

public class BalloonSpawner : MonoBehaviour
{
    [SerializeField] private Balloon m_BalloonPrefab;

    [Space(15)] [SerializeField] private AnimationCurve m_SpawnTimeCurve;

    [SerializeField] private AnimationCurve m_HealthCurve;
    [SerializeField] private AnimationCurve m_FallSpeedCurve;
    private ObjectPool<Balloon> m_BalloonsPool;
    private Camera m_Camera;

    private int m_MaxHealth;

    private Coroutine m_SpawnRoutine;

    private void Awake()
    {
        m_Camera = Camera.main;
        m_BalloonsPool = new ObjectPool<Balloon>(m_BalloonPrefab, 50);

        var healthCurveMaxTime = m_HealthCurve.keys[m_HealthCurve.length - 1].time;
        m_MaxHealth = Mathf.RoundToInt(m_HealthCurve.Evaluate(healthCurveMaxTime));
    }

    private void OnEnable()
    {
        Game.Started += GameOnStarted;
        Game.Ended += GameOnEnded;

        Balloon.OutOfBounds += BalloonOnOutOfBounds;
        Balloon.Destroy += ReturnInPool;
    }

    private void OnDisable()
    {
        Game.Started -= GameOnStarted;
        Game.Ended -= GameOnEnded;

        Balloon.OutOfBounds -= BalloonOnOutOfBounds;
        Balloon.Destroy -= ReturnInPool;
    }

    private void BalloonOnOutOfBounds(Balloon obj)
    {
        m_BalloonsPool.ReturnInPool(obj);
    }

    private void GameOnStarted()
    {
        m_SpawnRoutine = StartCoroutine(SpawnBalloon());
    }

    private void GameOnEnded()
    {
        StopCoroutine(m_SpawnRoutine);
        m_BalloonsPool.ReturnAll(balloon => balloon.Die());
    }

    private float CalculateSpawnTime(float time)
    {
        return m_SpawnTimeCurve.Evaluate(time);

        // var x = time * time / 20;
        //
        // return Mathf.Clamp(m_DefaultSpawnTime - x, m_MinSpawnTime, float.MaxValue);
    }

    private int CalculateHealth(float time)
    {
        return Mathf.CeilToInt(m_HealthCurve.Evaluate(time) * Random.value);

        // var fx = -(10 / (time + 2.5f)) + m_MaxHealth;
        //
        // return Mathf.RoundToInt(fx);
    }

    private float CalculateSpeed(float time, float random)
    {
        return m_FallSpeedCurve.Evaluate(time) * random;

        //return (time * time / 20 + 1) * random;
    }

    private Color CalculateColor(int health)
    {
        var h = Random.Range(0, 360);

        Color.RGBToHSV(m_Camera.backgroundColor, out var backH, out _, out _);

        if (h > backH + 45 || h < backH - 45)
            h += 25 * Mathf.RoundToInt(Mathf.Sign(Random.value));

        var s = Mathf.Clamp(health, 0.1f, m_MaxHealth * 0.8f) / m_MaxHealth;

        return Color.HSVToRGB(h / 360f, s, 1);
    }

    private IEnumerator SpawnBalloon()
    {
        while (true)
        {
            if (Game.IsPaused)
            {
                yield return null;
                continue;
            }

            var timeFromStart = Game.TimeFromStart;

            yield return new WaitForSeconds(CalculateSpawnTime(timeFromStart));

            var balloonInstance = m_BalloonsPool.GetAvailable();

            var health = CalculateHealth(timeFromStart);
            var fallSpeed = CalculateSpeed(timeFromStart, Random.value);
            var color = CalculateColor(health);

            balloonInstance.Initialize(health, fallSpeed, color);
            balloonInstance.transform.position = Map.GetRandomUpperPosition();
        }
    }

    private void ReturnInPool(Balloon balloon)
    {
        m_BalloonsPool.ReturnInPool(balloon);
    }
}