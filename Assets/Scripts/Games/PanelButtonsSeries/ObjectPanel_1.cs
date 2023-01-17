using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPanel_1 : MonoBehaviour
{
    public bool _placed=true;
    public bool _pressed = false;
    public Sprite _pressedSprite;
    public Sprite _restSprite;
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
                buttonCounter._nDiamond = 1 + buttonCounter._nDiamond;
            }
            else if (GetComponent<Geometry>().getGeometryString() == "rectángulo")
            {
                buttonCounter._nRectangle = 1 + buttonCounter._nRectangle;
            }
            else if (GetComponent<Geometry>().getGeometryString() == "pentágono")
            {
                buttonCounter._nPentagon = 1 + buttonCounter._nPentagon;
            }
            else if (GetComponent<Geometry>().getGeometryString() == "hexágono")
            {
                buttonCounter._nHexagon = 1 + buttonCounter._nHexagon;
            }

        }
        //if (_pressed==true)
        //{
        //    GetComponent<GeometryButton>()._light.SetActive(false);
        //}
        //else
        //{
        //    GetComponent<GeometryButton>()._light.SetActive(true);
        //}
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
