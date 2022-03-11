using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidInputAdapter : MonoBehaviour, IInput
{
    public struct Datos
    {
        public bool result;
        public Vector2 pos;
    }

    //Drag and drop system
    public void Drag()
    {
        //Debug.Log("Mobile drag input");
    }

    public Datos InputTouch()
    {
        EDebug.Log("Tocado");
        Datos newDatos;
        newDatos.result = false;
        newDatos.pos = new Vector2();

        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            newDatos.result = true;
            newDatos.pos = Camera.main.ScreenToWorldPoint(touch.position);
        }
        return newDatos;
    }
}
