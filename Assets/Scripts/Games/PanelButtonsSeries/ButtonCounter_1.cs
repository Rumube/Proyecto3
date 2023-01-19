using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonCounter_1 : MonoBehaviour
{
    public CreatePanel _createPanel;

    [Header("Animations")]
    public GameObject _bar;
    public GameObject _radar;

    [Header("Geometry List")]
    public List<GameObject> _geometrylist = new List<GameObject>();
    private Geometry.Geometry_Type _trueGeometry;
    private int geo = 0;
    private void Start()
    {
        UpdateButtonGO();
    }

    private void Update()
    {

    }

    private void UpdateButtonGO()
    {
        TurnsButtonsOff();
        switch (GetComponent<Geometry>()._geometryType)
        {
            case Geometry.Geometry_Type.circle:
                _geometrylist[0].SetActive(true);
                break;
            case Geometry.Geometry_Type.triangle:
                _geometrylist[1].SetActive(true);
                break;
            case Geometry.Geometry_Type.square:
                _geometrylist[2].SetActive(true);
                break;
            case Geometry.Geometry_Type.diamond:
                _geometrylist[3].SetActive(true);
                break;
            case Geometry.Geometry_Type.pentagon:
                _geometrylist[4].SetActive(true);
                break;
            case Geometry.Geometry_Type.star:
                _geometrylist[5].SetActive(true);
                break;
            default:
                break;
        }

    }
    private void TurnsButtonsOff()
    {
        for (int i = 0; i < _geometrylist.Count; i++)
        {
            _geometrylist[i].SetActive(false);
        }

    }

    public string _GetTextGame()
    {
        return "¡Pulsa los botones hasta completar las series!";
    }

    public void ChangeGeometry()
    {
        geo++;
        if (geo >= 6)
        {
            geo = 0;
        }

        switch (geo)
        {
            case 0:
                GetComponent<Geometry>()._geometryType = Geometry.Geometry_Type.circle;
                break;
            case 1:
                GetComponent<Geometry>()._geometryType = Geometry.Geometry_Type.triangle;
                break;
            case 2:
                GetComponent<Geometry>()._geometryType = Geometry.Geometry_Type.square;
                break;
            case 3:
                GetComponent<Geometry>()._geometryType = Geometry.Geometry_Type.diamond;
                break;
            case 4:
                GetComponent<Geometry>()._geometryType = Geometry.Geometry_Type.pentagon;
                break;
            case 5:
                GetComponent<Geometry>()._geometryType = Geometry.Geometry_Type.star;
                break;
            default:
                break;
        }
        UpdateButtonGO();
    }


    /// <summary>Check how much geometry is ok.</summary> 
    /// <param name="nGeometry">The quantity of a geometry</param> 
    /// <param name="counter">The geometry of the player</param>

    #region Button Counters //Se utiliza tal cual

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

    public void EnableButtons(bool value)
    {
        foreach (GameObject currentButton in _geometrylist)
        {
            currentButton.GetComponent<Button>().enabled = value;
        }
    }

    public void SetTrueGeometry(Geometry.Geometry_Type newGeometry)
    {
        _trueGeometry = newGeometry;
    }
    public Geometry.Geometry_Type GetTrueGeometry()
    {
        return _trueGeometry;
    }
}
