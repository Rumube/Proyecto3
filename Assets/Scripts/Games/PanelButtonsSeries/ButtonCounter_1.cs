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

    // Update is called once per frame
    void Update()
    {
        switch (GetComponent<CreatePanel_1>()._GeometryButtons.GetComponent<Geometry>()._geometryType)
        {
            case Geometry.Geometry_Type.circle:

                break;
            case Geometry.Geometry_Type.triangle:
                break;
            case Geometry.Geometry_Type.square:
                break;
            case Geometry.Geometry_Type.diamond:
                break;
            case Geometry.Geometry_Type.rectangle:
                break;
            case Geometry.Geometry_Type.pentagon:
                break;
            case Geometry.Geometry_Type.hexagon:
                break;
            case Geometry.Geometry_Type.star:
                break;
            default:
                break;
        }

       
    }

    
    public void Compare()
    {
              
    }
    /// <summary>Check how much geometry is ok.</summary> 
    /// <param name="nGeometry">The quantity of a geometry</param> 
    /// <param name="counter">The geometry of the player</param>
    public void CheckGeometry(int nGeometry, int counter) //Comprobar las listas
    {
           
    }
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
