using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EDebug
{
#if UNITY_EDITOR
    public static void Log<T>(T msg)
    {
        Debug.Log(msg);
    }    
#endif

}
