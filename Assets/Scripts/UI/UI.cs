using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UI : MonoBehaviour,IUI
{
    protected int _uiIndex;
    protected List<GameObject> _windowsTree;
    /** 
  * @desc Quit the game
  */
    public void QuitGame()
    {
        Application.Quit();
    }
    /** 
    * @desc Open/close the window credits
    */
    public void Credits()
    {
        ServiceLocator.Instance.GetService<GameManager>()._credits.SetActive(!ServiceLocator.Instance.GetService<GameManager>()._credits.activeSelf);
    }
    /** 
    * @desc Open the next window deppending on the position of the array and close the previous one
    */
    public abstract void OpenNextWindow();
    /** 
   * @desc Open the previous window deppending on the position of the array and close the current one
   */
    public abstract void OpenPreviousWindow();
}
