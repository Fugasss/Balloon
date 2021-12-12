using System;
using Core;
using DG.Tweening;
using Score;
using UnityEngine;

namespace FallingObject
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(BoxCollider2D))]
    public abstract class FallingObjectBase : MonoBehaviour, IDamageable, IScoreProvider
    {
        protected Color Color;
        protected float FallSpeed;
        protected SpriteRenderer SpriteRenderer;

        protected int MaxHealth { get; private set; }

        protected IPauseProvider PauseProvider;
        protected IStartProvider StartProvider;
        private BalloonDestroyEffectManager m_DestroyEffectManager;

        private Action m_DestroyCallback;
        private Action m_OutOfBoundsCallback;
        
        internal void Construct(IStartProvider startProvider, IPauseProvider pauseProvider,
            BalloonDestroyEffectManager destroyEffectManager, Action destroyCallback,
            Action outOfBoundCallback)
        {
            StartProvider = startProvider;
            PauseProvider = pauseProvider;
            m_DestroyEffectManager = destroyEffectManager;
            m_DestroyCallback = destroyCallback;
            m_OutOfBoundsCallback = outOfBoundCallback;
        }

        protected virtual void Awake()
        {
            SpriteRenderer = GetComponent<SpriteRenderer>();
        }

        protected virtual void Update()
        {
            if (PauseProvider.IsPaused) return;

            Move();
            CheckOutOfBounds();
        }

        public int CurrentHealth { get; private set; }

        public virtual void TakeDamage(int damage)
        {
            CurrentHealth -= damage;

            transform.DOPunchScale(Vector3.one * 0.5f, 0.1f, 15, 0.5f);

            if (CurrentHealth <= 0)
            {
                Die();
                AfterDie();

                AudioPlayer.Play("pop");
            }
            else
            {
                AudioPlayer.Play("hit");
            }
        }

        public virtual void Die()
        {
            if (!gameObject.activeInHierarchy) return;

            m_DestroyEffectManager.Play(transform.position, Color);
            m_DestroyCallback?.Invoke();
        }

        public void Initialize(int health, float fallSpeed, Color color)
        {
            FallSpeed = fallSpeed;
            CurrentHealth = health;
            MaxHealth = health;
            Color = color;

            SpriteRenderer.color = color;

            AfterInitialize();
        }
        
        private void Move()
        {
            transform.Translate(Vector3.down * (FallSpeed * Time.deltaTime));
            FallSpeed += Time.deltaTime;
        }

        private void CheckOutOfBounds()
        {
            if (!Map.IsOutOfBounds(transform)) return;

            OnOutOfBounds();
        }

        protected virtual void AfterDie(){}
        protected virtual void AfterInitialize(){}

        private void OnOutOfBounds()
        {
            m_OutOfBoundsCallback?.Invoke();
        }
        public abstract int GetScore();
    }
}