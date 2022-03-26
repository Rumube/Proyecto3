using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesktopInputAdapter : MonoBehaviour, IInput
{

    public void Drag()
    {
       //Debug.Log("Desktop input");
    }

    public AndroidInputAdapter.Datos InputTouch()
    {
        EDebug.Log("Tocado");
        AndroidInputAdapter.Datos newDatos;
        newDatos.result = false;
        newDatos.pos = new Vector2();

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            newDatos.result = true;
            newDatos.pos = touch.position;
        }
        return newDatos;
    }

}
