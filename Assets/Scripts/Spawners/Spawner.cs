using System.Collections;
using UnityEngine;

public abstract class Spawner<T> : MonoBehaviour, IObjectPoolContainer<T> where T : FallingObjectBase
{
    [SerializeField] private T[] m_Prefabs;

    [Space(15)] [SerializeField] protected AnimationCurve SpawnTimeCurve;
    [SerializeField] protected AnimationCurve HealthCurve;
    [SerializeField] protected AnimationCurve FallSpeedCurve;

    private Coroutine m_SpawnRoutine;

    protected int MaxHealth { get; private set; }

    protected virtual void Awake()
    {
        ObjectPool = new ObjectPool<T>(m_Prefabs, 20);

        var healthCurveMaxTime = HealthCurve.keys[HealthCurve.length - 1].time;
        MaxHealth = Mathf.RoundToInt(HealthCurve.Evaluate(healthCurveMaxTime));
    }

    protected virtual void OnEnable()
    {
        Game.Started += GameOnStarted;
        Game.Ended += GameOnEnded;
    }

    protected virtual void OnDisable()
    {
        Game.Started -= GameOnStarted;
        Game.Ended -= GameOnEnded;
    }

    public ObjectPool<T> ObjectPool { get; private set; }

    private void GameOnStarted()
    {
        m_SpawnRoutine = StartCoroutine(Spawn());
    }

    private void GameOnEnded()
    {
        StopCoroutine(m_SpawnRoutine);
        ObjectPool.ReturnAll(obj => obj.Die());
    }


    protected virtual float CalculateSpawnTime(float time)
    {
        return SpawnTimeCurve.Evaluate(time);
    }

    protected virtual int CalculateHealth(float time)
    {
        return Mathf.CeilToInt(HealthCurve.Evaluate(time) * Random.value);
    }

    protected virtual float CalculateSpeed(float time, float random)
    {
        return FallSpeedCurve.Evaluate(time) * random;
    }

    protected abstract Color CalculateColor(int health);

    protected void ReturnInPool(T obj)
    {
        ObjectPool.ReturnInPool(obj);
    }

    private IEnumerator Spawn()
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

            var instance = ObjectPool.GetAvailable();

            var health = CalculateHealth(timeFromStart);
            var fallSpeed = CalculateSpeed(timeFromStart, Random.value);
            var color = CalculateColor(health);

            instance.Initialize(health, fallSpeed, color);
            instance.transform.position = Map.GetRandomUpperPosition();
        }
    }
}