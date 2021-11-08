using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class ObjectPool<T> where T : Component
{
    private readonly List<T> m_Objects;
    private readonly Transform m_ParentObject;
    private readonly T m_Prefab;

    private int m_LastIndex = -1;

    public ObjectPool(T prefab, int count)
    {
        m_Prefab = prefab;
        m_Objects = new List<T>(count);
        m_ParentObject = new GameObject(typeof(T).ToString()).transform;

        Fill(count);
    }

    private void Fill(int count)
    {
        for (var i = 0; i < count; i++) _ = CreateNew();
    }

    private T CreateNew()
    {
        var instance = Object.Instantiate(m_Prefab, m_ParentObject);
        instance.gameObject.SetActive(false);

        m_Objects.Add(instance);

        return instance;
    }

    public T GetAvailable()
    {
        T instance = null;
        var count = m_Objects.Count;

        if (m_LastIndex == count - 1)
            m_LastIndex = -1;

        for (var i = ++m_LastIndex; i < count; ++i)
        {
            if (m_Objects[i].gameObject.activeInHierarchy) continue;

            m_LastIndex = i;
            instance = m_Objects[i];
            break;
        }

        instance ??= CreateNew();

        instance.gameObject.SetActive(true);

        return instance;
    }

    public void ReturnInPool(T obj)
    {
        if (m_Objects.Contains(obj)) obj.gameObject.SetActive(false);
    }

    public void ReturnAll(Action<T> actionWithObject = null)
    {
        foreach (var obj in m_Objects)
        {
            actionWithObject?.Invoke(obj);
            ReturnInPool(obj);
        }
    }
}