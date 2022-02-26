using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EDebug
{
#if UNITY_EDITOR
    public static void Log(string msg)
    {

        Debug.Log(msg);
    }    
    
    public static void Log(int msg)
    {

        Debug.Log(msg);
    }
    
    public static void Log(float msg)
    {

        Debug.Log(msg);
    }
    
    public static void Log(bool msg)
    {

        Debug.Log(msg);
    }
#endif

}
