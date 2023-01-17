using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ButtonCounter_Prueba_1 : MonoBehaviour
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


    int _goodGeometry;
    int _badGeometry;

    private CreatePanel_Prueba _createPanel;

    [Header("Animations")]
    public GameObject _leveler;
    public GameObject _wheel;
    private bool _newGame = true;
    List<Geometry.Geometry_Type> geometryButtonsSelected = new List<Geometry.Geometry_Type>();
    List<Geometry.Geometry_Type> geometryButtons = new List<Geometry.Geometry_Type>();

    // Update is called once per frame
    void Update()
    {

        //_gameText= GeometryNumberText(_nCircle, "círculo")+  GeometryNumberText(_nTriangle, "triángulo")+ GeometryNumberText(_nSquare, "cuadrado") + 
        //GeometryNumberText(_nDiamond, "diamante") + GeometryNumberText(_nRectangle, "rectángulo") + GeometryNumberText(_nPentagon, "pentágono") + GeometryNumberText(_nHexagon, "hexágono");
    }

    private void Start()
    {
        _createPanel = GetComponent<CreatePanel_Prueba>();
        
    }
   
    public string GetTextGame()
    {
        string message = "";
        if (_newGame)
        {
            _newGame = false;
            message = "Pulsa los botones con forma de ";
        }
        else
        {
            message = "Ahora con forma de " ;
        }
        Geometry geometry_aux = new Geometry();
        geometryButtons.Clear();
        foreach (GameObject currentGeometry in _createPanel._targetList)
        {
            geometryButtons.Add(currentGeometry.GetComponent<Geometry>()._geometryType);
        }
        geometryButtons = geometryButtons.Distinct().ToList();
        for (int i = 0; i < geometryButtons.Count; i++)
        {
            if(i != 0)
            {
                message += "y de " + (geometry_aux.getGeometryString(geometryButtons[i]));
            }
            else
            {
                message += (geometry_aux.getGeometryString(geometryButtons[i])) + " ";
            }
        }      
        return message;
    }

    /// <summary>Check the quantity of success.</summary> 
    public void Compare()
    {
        if (_squareCounter + _triangleCounter + _circleCounter + _diamondCounter + _rectangleCounter + _hexagonCounter + _pentagonCounter > 0)
        {
           

            foreach (GameObject currentButton in _createPanel._allList)
            {
                if (!geometryButtonsSelected.Contains(currentButton.GetComponent<Geometry>()._geometryType))
                {
                    geometryButtonsSelected.Add(currentButton.GetComponent<Geometry>()._geometryType);
                }
            }

            foreach (Geometry.Geometry_Type currentGeometry in geometryButtonsSelected)
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

            _leveler.GetComponent<Animator>().Play("Leveler_Animation");
            _wheel.GetComponent<Animator>().Play("Wheel_Animation");

            _goodGeometry = 0;
            _badGeometry = 0;
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
                _goodGeometry += 1;
            }
            else
            {
                _badGeometry++;
                Debug.Log("bad"+ _badGeometry);
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
