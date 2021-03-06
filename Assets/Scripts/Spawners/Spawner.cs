using System.Collections;
using Core;
using FallingObject;
using Score;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Spawners
{
    public abstract class Spawner<T> : MonoBehaviour, IObjectPoolContainer<T> where T : FallingObjectBase
    {
        [SerializeField] private T[] m_Prefabs;

        [Space(15)] [SerializeField] protected AnimationCurve SpawnTimeCurve;
        [SerializeField] protected AnimationCurve HealthCurve;
        [SerializeField] protected AnimationCurve FallSpeedCurve;

        private Coroutine m_SpawnRoutine;

        protected int MaxHealth { get; private set; }

        private IStartProvider m_StartProvider;
        private IPauseProvider m_PauseProvider;
        private BalloonDestroyEffectManager m_DestroyEffectManager;

        private IScoreChangerProvider m_ScoreChangerProvider;

        [Inject]
        private void Construct(IStartProvider startProvider, IPauseProvider pauseProvider,
            BalloonDestroyEffectManager destroyEffectManager, IScoreChangerProvider scoreChangerProvider)
        {
            m_StartProvider = startProvider;
            m_PauseProvider = pauseProvider;
            m_DestroyEffectManager = destroyEffectManager;
            m_ScoreChangerProvider = scoreChangerProvider;
        }

        protected virtual void Awake()
        {
            ObjectPool = new ObjectPool<T>(m_Prefabs, 20);

            var healthCurveMaxTime = HealthCurve.keys[HealthCurve.length - 1].time;
            MaxHealth = Mathf.RoundToInt(HealthCurve.Evaluate(healthCurveMaxTime));

            ObjectPool.InvokeForAll(
                x => x.Construct(m_StartProvider, m_PauseProvider, m_DestroyEffectManager,
                    () => DestroyObjectCallback(x),
                    () => OutOfBoundsObjectCallback(x)));
        }

        protected virtual void OnEnable()
        {
            m_StartProvider.Started += GameOnStarted;
            m_StartProvider.Ended += GameOnEnded;
        }

        protected virtual void OnDisable()
        {
            m_StartProvider.Started -= GameOnStarted;
            m_StartProvider.Ended -= GameOnEnded;
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

        private void ReturnInPool(T obj)
        {
            ObjectPool.ReturnInPool(obj);
        }

        protected virtual void DestroyObjectCallback(T fallingObjectBase)
        {
            m_ScoreChangerProvider.ChangeScore(fallingObjectBase);
            ReturnInPool(fallingObjectBase);
        }

        protected virtual void OutOfBoundsObjectCallback(T fallingObjectBase)
        {
            ReturnInPool(fallingObjectBase);
        }
        
        private IEnumerator Spawn()
        {
            while (true)
            {
                if (m_PauseProvider.IsPaused)
                {
                    yield return null;
                    continue;
                }

                var timeFromStart = m_StartProvider.TimeFromStart;

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
}