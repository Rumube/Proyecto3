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
  
    private void Start()
    {
        TurnsButtonsOff();
        _geometrylist[(int)GetComponent<Geometry>()._geometryType].SetActive(true);
    }

    private void Update()
    {
        
    }
    private void TurnsButtonsOff()
    {
        for (int i = 0; i < _geometrylist.Count; i++)
        {
            _geometrylist[i].SetActive(false);
        }

    }

    public void ChangeGeometry()
    {
        int geo = (int)GetComponent<Geometry>()._geometryType;
        geo++;

        if (geo >= 7)
        {
            geo = 0;
        }
        GetComponent<Geometry>()._geometryType = (Geometry.Geometry_Type)geo;
        TurnsButtonsOff();
        _geometrylist[(int)GetComponent<Geometry>()._geometryType].SetActive(true);
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
}
