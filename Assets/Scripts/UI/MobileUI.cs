using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MobileUI : UI
{
    [Header("Game Connection")]
    public Text _ip;
    public Text _port;
    public Text _connectedTablets;

    public InputField _inputSessionMinutes;
    public InputField _inputSessionSeconds;
    public InputField _inputMinigamesMinutes;
    public InputField _inputMinigamesSeconds;
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
  * @desc Get ip/port from GM
  */
    public void GetIpPort()
    {
        _ip.text = "IP: "+ ServiceLocator.Instance.GetService<GameManager>()._ip;
        _port.text = "Puerto: " + ServiceLocator.Instance.GetService<GameManager>()._port;
    }

    /** 
  * @desc Set the time for the whole session passing values to the GM
  */
    public void SetTimeSession()
    {
        ServiceLocator.Instance.GetService<GameManager>().SetTimeSession(_inputSessionMinutes.text, _inputSessionSeconds.text);
    }
    /** 
  * @desc Set the time for all minigames passing values to the GM
  */
    public void SetTimeMinigames()
    {
        ServiceLocator.Instance.GetService<GameManager>().SetTimeMinigames(_inputMinigamesMinutes.text, _inputMinigamesSeconds.text);
    }
    private void Update()
    {
        switch (ServiceLocator.Instance.GetService<GameManager>()._gameStateServer)
        {
            case GameManager.GAME_STATE_SERVER.connection:
                if (ServiceLocator.Instance.GetService<GameManager>().GetUpdateConnectedTablets())
                {
                    _connectedTablets.text = ServiceLocator.Instance.GetService<GameManager>().GetConnectedTablets().ToString();
                    ServiceLocator.Instance.GetService<GameManager>().SetUpdateConnectedTablets(false);
                }
                else
                {
                    print("No");
                }
                break;
        }
       
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
