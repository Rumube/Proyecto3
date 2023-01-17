using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonCounter_1 : MonoBehaviour
{
    [Header("Number of geometry target")]
    public int _nSquare;
    public int _nTriangle;
    public int _nCircle;
    public int _nDiamond;
    public int _nRectangle;
    public int _nPentagon;
    public int _nHexagon;

    [Header("Number of geometry pressed")]
    public int _squareCounter;
    public int _triangleCounter;
    public int _circleCounter;
    public int _diamondCounter;
    public int _rectangleCounter;
    public int _pentagonCounter;
    public int _hexagonCounter;

    public CreatePanel _createPanel;

    [Header("Animations")]
    public GameObject _bar;
    public GameObject _radar;

    // Update is called once per frame
    void Update()
    {
                                
    }

    public string GetTextGame()
    {
        string message = (GeometryNumberText(_nCircle, "círculo") + GeometryNumberText(_nTriangle, "triángulo") + GeometryNumberText(_nSquare, "cuadrado") +
        GeometryNumberText(_nDiamond, "diamante") + GeometryNumberText(_nRectangle, "rectángulo") + GeometryNumberText(_nPentagon, "pentágono") + GeometryNumberText(_nHexagon, "hexágono"));
        return "Selecciona " + message;
    }

    /// <summary>Show the geometry name in plural or singular.</summary> 

    /// <param name="nGeometry">The quantity of a geometry</param> 
    /// <param name="geometryName">The name of the geometry</param>

    /// <returns>Empty or the name of the geometry in singular or plural</returns> 
    public string GeometryNumberText(int nGeometry,string geometryName)
    {
       
        if (nGeometry==0)
        {
            return "";
        }
        else if (nGeometry>1)
        {
            switch(nGeometry) {
                case 2:
                    return "dos " + geometryName + "s ";                  
                case 3:
                    return "tres " + geometryName + "s ";
                case 4:
                    return "cuatro " + geometryName + "s ";
                case 5:
                    return "cinco " + geometryName + "s ";
                case 6:
                    return "seis " + geometryName + "s ";
                case 7:
                    return "siete " + geometryName + "s ";
                case 8:
                    return "ocho " + geometryName + "s ";
                case 9:
                    return "nueve " + geometryName + "s ";
                default:
                    return "texto por defecto " + geometryName + "s ";
            }            
        }
        else
        {
            return "un " + geometryName+" ";
        }
    }

    /// <summary>Check the quantity of success.</summary> 
    public void Compare()
    {
              
    }
    /// <summary>Check how much geometry is ok.</summary> 
    /// <param name="nGeometry">The quantity of a geometry</param> 
    /// <param name="counter">The geometry of the player</param>
    public void CheckGeometry(int nGeometry, int counter)
    {
            if (nGeometry == counter)
            {
                _goodGeometry++;
            }
            else
            {
                _badGeometry++;
            }
    }
    #region Button Counters
    public void CounterSquare(GameObject button)
    {
        _squareCounter=Counter(button, _squareCounter);
    }
    public void CounterTriangle(GameObject button)
    {
        _triangleCounter=Counter(button, _triangleCounter);
    }
    public void CounterCircle(GameObject button)
    {
        _circleCounter= Counter(button, _circleCounter);
    }
    public void CounterDiamond(GameObject button)
    {
        _diamondCounter=Counter(button, _diamondCounter);
    }
    public void CounterRectangle(GameObject button)
    {
        _rectangleCounter=Counter(button, _rectangleCounter);
    }
    public void CounterPentagon(GameObject button)
    {
        _pentagonCounter= Counter(button, _pentagonCounter);
    }
    public void CounterHexagon(GameObject button)
    {
       _hexagonCounter= Counter(button, _hexagonCounter);
    }
    public int Counter(GameObject button, int counter)
    {
        if (button.GetComponent<ObjectPanel>()._pressed == false)
        {
            counter++;
            button.GetComponent<Image>().sprite = button.GetComponent<Button>().spriteState.pressedSprite;
            button.GetComponent<ObjectPanel>()._pressed = true;
        }
        else
        {
            counter--;
            button.GetComponent<Image>().sprite = button.GetComponent<Button>().spriteState.disabledSprite;
            button.GetComponent<ObjectPanel>()._pressed = false;
        }
        return counter;
    }
    #endregion  //Útil
}
