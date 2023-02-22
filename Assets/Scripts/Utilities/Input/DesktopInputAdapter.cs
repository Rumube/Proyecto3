using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesktopInputAdapter : MonoBehaviour, IInput
{

    public void Drag()
    {
    }
    /// <summary>
    /// Manage the screen inputs
    /// </summary>
    /// <returns>The input data</returns>
    public AndroidInputAdapter.Datos InputTouch()
    {
        AndroidInputAdapter.Datos newDatos;
        newDatos.result = false;
        newDatos.pos = new Vector2();
        newDatos.phase = TouchPhase.Canceled;
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            newDatos.result = true;
            newDatos.pos = touch.position;
            newDatos.phase = touch.phase;
        }
        return newDatos;
    }

}
