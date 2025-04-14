using System;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public abstract class Singleton<T> : MonoBehaviour where T : Component
{
    protected static T instance;
    public static T Instance
    {
        get
        {
            try
            {
                instance = FindAnyObjectByType<T>();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                GameObject obj = new GameObject($"{typeof(T).Name}");
                instance = obj.AddComponent<T>();
                DontDestroyOnLoad(obj);
            }
            finally
            {
                //this code will always run
            }

            return instance;
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Awake()
    {
        if (!instance)
        {
            instance = this as T;
            DontDestroyOnLoad(instance);
            return;
        }

        Destroy(gameObject);
    }
}
