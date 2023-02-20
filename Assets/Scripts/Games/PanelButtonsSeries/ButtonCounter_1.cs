using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonCounter_1 : MonoBehaviour
{
    [Header("Animations")]
    public GameObject _bar;
    public GameObject _radar;

    [Header("Geometry List")]
    public List<GameObject> _geometrylist = new List<GameObject>();
    private Geometry.Geometry_Type _trueGeometry;
    private int _geo = 0;
    private void Start()
    {
     UpdateButtonGO();
    }
    /// <summary>
    /// Changes the activated gameObject depending on the value of the <see cref="Geometry.Geometry_Type"/>
    /// </summary>
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
    /// <summary>
    /// Turn off all the buttons 
    /// </summary>
    private void TurnsButtonsOff()
    {
        for (int i = 0; i < _geometrylist.Count; i++)
        {
            _geometrylist[i].SetActive(false);
        }

    }
    /// <summary>
    /// Generate new order
    /// </summary>
    /// <returns></returns>
    public string _GetTextGame()
    {
        return "¡Pulsa los botones hasta completar las series!";
    }
    /// <summary>
    /// Change the geometry to the next geometry in the list using <see cref="_geo"/>
    /// </summary>
    public void ChangeGeometry()
    {
        _geo++;
        if (_geo >= 6)
        {
            _geo = 0;
        }

        switch (_geo)
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
    /// <summary>
    /// Turns buttons on or off depending on the value received
    /// </summary>
    /// <param name="value">The value</param>
    public void EnableButtons(bool value)
    {
        foreach (GameObject currentButton in _geometrylist)
        {
            currentButton.GetComponent<Button>().enabled = value;
        }
    }
    /// <summary>
    /// Sets the true geometry needs to be correct
    /// </summary>
    /// <param name="newGeometry"></param>
    public void SetTrueGeometry(Geometry.Geometry_Type newGeometry)
    {
        _trueGeometry = newGeometry;
    }
    /// <summary>
    /// Returns the true geometry
    /// </summary>
    /// <returns><see cref="Geometry.Geometry_Type"/></returns>
    public Geometry.Geometry_Type GetTrueGeometry()
    {
        return _trueGeometry;
    }
}
