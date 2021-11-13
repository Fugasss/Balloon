using UnityEngine;

public interface IObjectPoolContainer<T> where T : Component
{
    public ObjectPool<T> ObjectPool { get; }
}