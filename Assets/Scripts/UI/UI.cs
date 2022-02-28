using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UI : MonoBehaviour,IUI
{
    protected int _uiIndex;
    protected List<GameObject> _windowsTree;
    private void Awake()
    {
        _windowsTree = new List<GameObject>();
        _uiIndex = 0;
    }
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
    public  void OpenNextWindow()
    {
        _uiIndex++;
        _windowsTree[_uiIndex].SetActive(true);
        _windowsTree[_uiIndex - 1].SetActive(false);
    }

    /** 
      * @desc Open the previous window deppending on the position of the array and close the current one
      */
    public void OpenPreviousWindow()
    {
        _uiIndex--;
        _windowsTree[_uiIndex].SetActive(true);
        _windowsTree[_uiIndex + 1].SetActive(false);
    }
}
