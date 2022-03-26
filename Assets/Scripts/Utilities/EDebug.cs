using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EDebug
{
    public static void Log<T>(T msg)
    {
#if UNITY_EDITOR

        Debug.Log(msg);
#endif
    }

    public static void LogWarning<T>(T msg)
    {
#if UNITY_EDITOR

        Debug.LogWarning(msg);
#endif
    }

}
