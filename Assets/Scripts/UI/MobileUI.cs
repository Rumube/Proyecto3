using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MobileUI : UI
{
    public InputField _inputMinutes;
    public InputField _inputSeconds;
    public Text _countdown;
    // Start is called before the first frame update
    void Start()
    {    
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
      
        _windowsTree[_uiIndex].SetActive(true);
    }

    /** 
  * @desc Set the time for the whole session passing values to the GM
  */
    public void SetTimeSession()
    {
        ServiceLocator.Instance.GetService<GameManager>().SetTimeSession(_inputMinutes.text, _inputSeconds.text);
    }
    private void Update()
    {
        ShowCountDown();
    }
    /** 
    * @desc Show the timer
    */
    private void ShowCountDown()
    {
        _countdown.text = ServiceLocator.Instance.GetService<GameManager>()._timeSessionMinutes + ":" + ServiceLocator.Instance.GetService<GameManager>()._timeSessionSeconds;
    }
}
