using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T: MonoBehaviour
{
    public static T instance => _instance;
    private static T _instance;

    protected void Awake()
    {
        if (instance != null && instance != this as T)
        {
            Destroy(this);
            return;
        }
        _instance = this as T;
    }
}
