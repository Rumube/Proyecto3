using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonCounter : MonoBehaviour
{
    public int _nSquare;
    public int _nTriangle;
    public int _nCircle;
    public int _nDiamond;
    public int _nRectangle;
    public int _nPentagon;
    public int _nHexagon;

    public int _squareCounter;
    public int _triangleCounter;
    public int _circleCounter;
    public int _diamondCounter;
    public int _rectangleCounter;
    public int _pentagonCounter;
    public int _hexagonCounter;

    int _nButton;
    int _totalGeometry;
    int _goodGeometry;
    public Text _mission;
    private bool readText;
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
        _mission.text = GeometryNumberText(_nCircle, "c�rculo")+  GeometryNumberText(_nTriangle, "tri�ngulo")+ GeometryNumberText(_nSquare, "cuadrado") + 
        GeometryNumberText(_nDiamond, "diamante") + GeometryNumberText(_nRectangle, "rect�ngulo") + GeometryNumberText(_nPentagon, "pent�gono") + GeometryNumberText(_nHexagon, "hex�gono");
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
            return nGeometry+" "+geometryName + "s ";
        }
        else
        {
            return nGeometry+" "+ geometryName+" ";
        }
    }

    /// <summary>Check the quantity of success.</summary> 
    public void Compare()
    {
        ServiceLocator.Instance.GetService<GameManager>()._gameStateClient = GameManager.GAME_STATE_CLIENT.ranking;
        CheckGeometry(_nCircle, _circleCounter);
        CheckGeometry(_nSquare, _squareCounter);
        CheckGeometry(_nTriangle, _triangleCounter);
        
        int porcentaje = _goodGeometry / _totalGeometry;
        EDebug.Log(_goodGeometry+"/"+_totalGeometry);
        if (_goodGeometry==_totalGeometry)
        {
            EDebug.Log("Bien hecho");
        }
        


    }
    /// <summary>Check how much geometry is ok.</summary> 
    /// <param name="nGeometry">The quantity of a geometry</param> 
    /// <param name="counter">The geometry of the player</param>

    /// <returns>Empty or the name of the geometry in singular or plural</returns> 
    public void CheckGeometry(int nGeometry, int counter)
    {
        if (nGeometry > 0)
        {
            _totalGeometry += 1;
            if (nGeometry == counter)
            {
                _goodGeometry += 1;
            };
        }
       
    }
  
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
}
