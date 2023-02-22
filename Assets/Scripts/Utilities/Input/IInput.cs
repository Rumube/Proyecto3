using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInput
{
    public void Drag();
    /// <summary>
    /// Manage the screen inputs
    /// </summary>
    /// <returns>The input data</returns>
    public AndroidInputAdapter.Datos InputTouch();
}
