using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesktopInputAdapter : IInput
{
    public void Drag()
    {
       Debug.Log("Desktop input");
    }
}
