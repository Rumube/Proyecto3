using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonCounter_Prueba : MonoBehaviour
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

    private CreatePanel_Prueba _createPanel;

    // Update is called once per frame
    void Update()
    {
        //_gameText= GeometryNumberText(_nCircle, "c�rculo")+  GeometryNumberText(_nTriangle, "tri�ngulo")+ GeometryNumberText(_nSquare, "cuadrado") + 
        //GeometryNumberText(_nDiamond, "diamante") + GeometryNumberText(_nRectangle, "rect�ngulo") + GeometryNumberText(_nPentagon, "pent�gono") + GeometryNumberText(_nHexagon, "hex�gono");
    }

    private void Start()
    {
        _createPanel = GetComponent<CreatePanel_Prueba>();
    }

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
    public string GeometryNumberText(int nGeometry, string geometryName)
    {
        if (nGeometry == 0)
        {
            return "";
        }
        else
        { 
            return geometryName + "s ";
        }
    
    }

    /// <summary>Check the quantity of success.</summary> 
    public void Compare()
    {
        
        CheckGeometry(_nCircle, _circleCounter);
        CheckGeometry(_nSquare, _squareCounter);
        CheckGeometry(_nDiamond, _diamondCounter);
        CheckGeometry(_nRectangle, _rectangleCounter);
        CheckGeometry(_nPentagon, _pentagonCounter);
        CheckGeometry(_nHexagon, _hexagonCounter);

        _badGeometry = _totalGeometry - _goodGeometry;
        if (_badGeometry > 0)
            ServiceLocator.Instance.GetService<IError>().GenerateError();
        else
            ServiceLocator.Instance.GetService<IPositive>().GenerateFeedback(Vector2.zero);


        ServiceLocator.Instance.GetService<ICalculatePoints>().Puntuation(_goodGeometry, _badGeometry);
        _goodGeometry = 0;
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
    /// <summary>Check how much geometry is ok.</summary> 
    /// <param name="nGeometry">The quantity of a geometry</param> 
    /// <param name="counter">The geometry of the player</param>
    public void CheckGeometry(int nGeometry, int counter)
    {
        if (nGeometry > 0)
        {
            _totalGeometry += 1;
            if (nGeometry == counter)
            {
                _goodGeometry += 1;
            }
        }

    }
    #region Button Counters
    public void CounterSquare(GameObject button)
    {
        _squareCounter = Counter(button, _squareCounter);
    }
    public void CounterTriangle(GameObject button)
    {
        _triangleCounter = Counter(button, _triangleCounter);
    }
    public void CounterCircle(GameObject button)
    {
        _circleCounter = Counter(button, _circleCounter);
    }
    public void CounterDiamond(GameObject button)
    {
        _diamondCounter = Counter(button, _diamondCounter);
    }
    public void CounterRectangle(GameObject button)
    {
        _rectangleCounter = Counter(button, _rectangleCounter);
    }
    public void CounterPentagon(GameObject button)
    {
        _pentagonCounter = Counter(button, _pentagonCounter);
    }
    public void CounterHexagon(GameObject button)
    {
        _hexagonCounter = Counter(button, _hexagonCounter);
    }
    public int Counter(GameObject button, int counter)
    {
        if (button.GetComponent<ObjectPanel_Prueba>()._pressed == false)
        {
            counter++;
            button.GetComponent<Image>().sprite = button.GetComponent<Button>().spriteState.pressedSprite;
            button.GetComponent<ObjectPanel_Prueba>()._pressed = true;
        }
        else
        {
            counter--;
            button.GetComponent<Image>().sprite = button.GetComponent<Button>().spriteState.disabledSprite;
            button.GetComponent<ObjectPanel_Prueba>()._pressed = false;
        }
        return counter;
    }
    #endregion
}
