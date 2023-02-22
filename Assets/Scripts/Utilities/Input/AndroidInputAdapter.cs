using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidInputAdapter : MonoBehaviour, IInput
{
    public struct Datos
    {
        public bool result;
        public Vector2 pos;
        public TouchPhase phase;
    }

    //Drag and drop system
    public void Drag()
    {
        //Debug.Log("Mobile drag input");
    }
    /// <summary>
    /// Manage the screen inputs
    /// </summary>
    /// <returns>The input data</returns>
    public Datos InputTouch()
    {
        Datos newDatos;
        newDatos.result = false;
        newDatos.pos = new Vector2();
        newDatos.phase =  TouchPhase.Canceled;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            newDatos.phase = touch.phase;
            if (touch.phase == TouchPhase.Began)
            {
                newDatos.result = true;
                newDatos.pos = touch.position;
            }else if(touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                newDatos.result = true;
                newDatos.pos = touch.position;
            }
            else
            {
                newDatos.result = false;
                newDatos.pos = touch.position;
            }
        }
        return newDatos;
    }
}
