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

    

    public CreatePanel _createPanel;

    [Header("Animations")]
    public GameObject _bar;
    public GameObject _radar;

    // Update is called once per frame
    void Update()
    {
                                
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
