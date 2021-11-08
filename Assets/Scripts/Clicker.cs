using UnityEngine;

public class Clicker : MonoBehaviour
{
    [SerializeField] [Min(1)] private int m_Damage;

    private Camera m_Camera;

    private void Awake()
    {
        m_Camera = Camera.main;
    }

    private void Update()
    {
        if (Game.IsPaused) return;
        if (!Game.IsPlaying) return;
        if (!Input.GetMouseButtonDown(0)) return;

        if (TryHit(out var damageable)) damageable.TakeDamage(m_Damage);
    }

    private bool TryHit(out IDamageable damageable)
    {
        var mousePos = m_Camera.ScreenToWorldPoint(Input.mousePosition);
        var hit = Physics2D.Raycast(mousePos, Vector2.zero);

        if (hit.collider != null && hit.collider.TryGetComponent<IDamageable>(out var obj))
        {
            damageable = obj;
            return true;
        }

        damageable = null;
        return false;
    }
}