using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPanel : MonoBehaviour
{
    public bool _placed=true;
    public ButtonCounter buttonCounter;
    void Start()
    {
        if (_placed == false)
        {
            if (GetComponent<Geometry>().getGeometryString() == "círculo")
            {
                buttonCounter._nCircle = 1 + buttonCounter._nCircle;
            }
            else if (GetComponent<Geometry>().getGeometryString() == "triángulo")
            {
                buttonCounter._nTriangle = 1 + buttonCounter._nTriangle;
            }
            else if (GetComponent<Geometry>().getGeometryString() == "cuadrado")
            {
                buttonCounter._nSquare = 1 + buttonCounter._nSquare;
            }
            else if (GetComponent<Geometry>().getGeometryString() == "diamante")
            {
                buttonCounter._circleCounter = 1 + buttonCounter._circleCounter;
            }
            else if (GetComponent<Geometry>().getGeometryString() == "rectángulo")
            {
                buttonCounter._circleCounter = 1 + buttonCounter._circleCounter;
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
