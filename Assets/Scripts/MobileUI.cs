using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileUI : UI
{
    
    // Start is called before the first frame update
    void Start()
    {
        _windowsTree = new List<GameObject>();
        _windowsTree.Add(ServiceLocator.Instance.GetService<GameManager>()._initialScreen);
        _windowsTree.Add(ServiceLocator.Instance.GetService<GameManager>()._mainMenu);
        _windowsTree.Add(ServiceLocator.Instance.GetService<GameManager>()._class);
        _windowsTree.Add(ServiceLocator.Instance.GetService<GameManager>()._gameConnection);
        _windowsTree.Add(ServiceLocator.Instance.GetService<GameManager>()._addStudent);
        _windowsTree.Add(ServiceLocator.Instance.GetService<GameManager>()._gameTime);
        _windowsTree.Add(ServiceLocator.Instance.GetService<GameManager>()._stadistics);
        _windowsTree.Add(ServiceLocator.Instance.GetService<GameManager>()._finalScore);

        for (int i = 0; i < _windowsTree.Count; ++i)
        {
            if(_windowsTree[i] != null) //Esto sacarlo cuando esten las pantallas asignadas en el GM
            _windowsTree[i].SetActive(false);
        }
        _uiIndex = 0;
        _windowsTree[_uiIndex].SetActive(true);
    }
    /** 
     * @desc Open the next window deppending on the position of the array and close the previous one
     */
    public override void OpenNextWindow()
    {
        _uiIndex++;
        _windowsTree[_uiIndex].SetActive(true);
        _windowsTree[_uiIndex - 1].SetActive(false);
    }
    /** 
      * @desc Open the previous window deppending on the position of the array and close the current one
      */
    public override void OpenPreviousWindow()
    {
        _uiIndex--;
        _windowsTree[_uiIndex].SetActive(true);
        _windowsTree[_uiIndex + 1].SetActive(false);
    }
}
