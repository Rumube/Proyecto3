using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonCounter : MonoBehaviour
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

    int _totalGeometry;
    int _goodGeometry;
    int _badGeometry;
    public CreatePanel _createPanel;

    [Header("Animations")]
    public GameObject _bar;
    public GameObject _radar;

    /// <summary>
    /// Generate a string whith the new order
    /// </summary>
    /// <returns></returns>
    public string GetTextGame()
    {
        string message = (GeometryNumberText(_nCircle, "c�rculo") + GeometryNumberText(_nTriangle, "tri�ngulo") + GeometryNumberText(_nSquare, "cuadrado") +
        GeometryNumberText(_nDiamond, "diamante") + GeometryNumberText(_nRectangle, "rect�ngulo") + GeometryNumberText(_nPentagon, "pent�gono") + GeometryNumberText(_nHexagon, "hex�gono"));
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
        if (_squareCounter+_triangleCounter+_circleCounter+_diamondCounter+_rectangleCounter+_hexagonCounter+_pentagonCounter>0)
        {
            List<Geometry.Geometry_Type> geometryButtons = new List<Geometry.Geometry_Type>();

            foreach (GameObject currentButton in _createPanel._allList)
            {
                if (!geometryButtons.Contains(currentButton.GetComponent<Geometry>()._geometryType))
                {
                    geometryButtons.Add(currentButton.GetComponent<Geometry>()._geometryType);
                }
            }

            foreach (Geometry.Geometry_Type currentGeometry in geometryButtons)
            {
                switch (currentGeometry)
                {
                    case Geometry.Geometry_Type.circle:
                        CheckGeometry(_nCircle, _circleCounter);
                        break;
                    case Geometry.Geometry_Type.triangle:
                        CheckGeometry(_nTriangle, _triangleCounter);
                        break;
                    case Geometry.Geometry_Type.square:
                        CheckGeometry(_nSquare, _squareCounter);
                        break;
                    case Geometry.Geometry_Type.diamond:
                        CheckGeometry(_nDiamond, _diamondCounter);
                        break;
                    case Geometry.Geometry_Type.rectangle:
                        CheckGeometry(_nRectangle, _rectangleCounter);
                        break;
                    case Geometry.Geometry_Type.pentagon:
                        CheckGeometry(_nPentagon, _pentagonCounter);
                        break;
                    case Geometry.Geometry_Type.hexagon:
                        CheckGeometry(_nHexagon, _hexagonCounter);
                        break;
                    case Geometry.Geometry_Type.star:
                        CheckGeometry(_nHexagon, _hexagonCounter);
                        break;
                    default:
                        break;
                }
            }

            if (_badGeometry > 0)
                ServiceLocator.Instance.GetService<IError>().GenerateError();
            else
                ServiceLocator.Instance.GetService<IPositive>().GenerateFeedback(Vector2.zero);

            ServiceLocator.Instance.GetService<ICalculatePoints>().Puntuation(_goodGeometry, _badGeometry);

            _bar.GetComponent<Animator>().Play("Bar_Animation");
            _radar.GetComponent<Animator>().Play("Radar_Animation");

            _goodGeometry = 0;
            _badGeometry = 0;
            _totalGeometry = 0;
            _nSquare = 0;
            _nTriangle = 0;
            _nCircle = 0;
            _nDiamond = 0;
            _nRectangle = 0;
            _nPentagon = 0;
            _nHexagon = 0;
            _squareCounter = 0;
            _triangleCounter = 0;
            _circleCounter = 0;
            _diamondCounter = 0;
            _rectangleCounter = 0;
            _pentagonCounter = 0;
            _hexagonCounter = 0;

            _createPanel.Restart();
        }
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
    /// <summary>
    /// Update the <see cref="Geometry.Geometry_Type.square"/> counter
    /// </summary>
    /// <param name="button">Button clicked</param>
    public void CounterSquare(GameObject button)
    {
        _squareCounter=Counter(button, _squareCounter);
    }
    /// <summary>
    /// Update the <see cref="Geometry.Geometry_Type.triangle"/> counter
    /// </summary>
    /// <param name="button">Button clicked</param>
    public void CounterTriangle(GameObject button)
    {
        _triangleCounter=Counter(button, _triangleCounter);
    }
    /// <summary>
    /// Update the <see cref="Geometry.Geometry_Type.circle"/> counter
    /// </summary>
    /// <param name="button">Button clicked</param>
    public void CounterCircle(GameObject button)
    {
        _circleCounter= Counter(button, _circleCounter);
    }
    /// <summary>
    /// Update the <see cref="Geometry.Geometry_Type.diamond"/> counter
    /// </summary>
    /// <param name="button">Button clicked</param>
    public void CounterDiamond(GameObject button)
    {
        _diamondCounter=Counter(button, _diamondCounter);
    }
    /// <summary>
    /// Update the <see cref="Geometry.Geometry_Type.rectangle"/> counter
    /// </summary>
    /// <param name="button">Button clicked</param>
    public void CounterRectangle(GameObject button)
    {
        _rectangleCounter=Counter(button, _rectangleCounter);
    }
    /// <summary>
    /// Update the <see cref="Geometry.Geometry_Type.pentagon"/> counter
    /// </summary>
    /// <param name="button">Button clicked</param>
    public void CounterPentagon(GameObject button)
    {
        _pentagonCounter= Counter(button, _pentagonCounter);
    }
    /// <summary>
    /// Update the <see cref="Geometry.Geometry_Type.hexagon"/> counter
    /// </summary>
    /// <param name="button">Button clicked</param>
    public void CounterHexagon(GameObject button)
    {
       _hexagonCounter= Counter(button, _hexagonCounter);
    }
    /// <summary>
    /// Check if need to add or substract a value
    /// </summary>
    /// <param name="button">Button clicked</param>
    /// <param name="counter">The numeric value</param>
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
    #endregion
}
