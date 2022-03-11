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

    [Header("Game Timer")]
    public InputField _inputSessionMinutes;
    public InputField _inputSessionSeconds;
    public InputField _inputMinigamesMinutes;
    public InputField _inputMinigamesSeconds;
    public Text _countdown;

    [Header("Delete class popup")]
    public Text _deletingClassName;
    public Text _writingClassName;
    bool _deleteClassCorrect;
    public Button _deleteClassButton;
    void Start()
    {    
        //Adding root windows in order of appearance
        _windowsTree.Add(ServiceLocator.Instance.GetService<GameManager>()._initialScreen);
        _windowsTree.Add(ServiceLocator.Instance.GetService<GameManager>()._mainMenu);
        _windowsTree.Add(ServiceLocator.Instance.GetService<GameManager>()._class);
        _windowsTree.Add(ServiceLocator.Instance.GetService<GameManager>()._gameConnection);
        _windowsTree.Add(ServiceLocator.Instance.GetService<GameManager>()._addStudent);
        _windowsTree.Add(ServiceLocator.Instance.GetService<GameManager>()._gameTime);
        _windowsTree.Add(ServiceLocator.Instance.GetService<GameManager>()._stadistics);
        _windowsTree.Add(ServiceLocator.Instance.GetService<GameManager>()._finalScore);

        //Desactive all windows
        for (int i = 0; i < _windowsTree.Count; ++i)
        {
            _windowsTree[i].SetActive(false);
        }
        //Non root windows
        ServiceLocator.Instance.GetService<GameManager>()._credits.SetActive(false);
        ServiceLocator.Instance.GetService<GameManager>()._popupAddClass.SetActive(false);
        ServiceLocator.Instance.GetService<GameManager>()._popupDeleteClass.SetActive(false);

        _deleteClassCorrect = false;

        //Active just the first one
        _windowsTree[_uiIndex].SetActive(true);
    }

    /// <summary>Show the IP and port from the device</summary>
    public void GetIpPort()
    {
        _ip.text = "IP: "+ ServiceLocator.Instance.GetService<GameManager>()._ip;
        _port.text = "Puerto: " + ServiceLocator.Instance.GetService<GameManager>()._port;
    }

    /// <summary>Set the time for the whole session passing values to the GM</summary>
    public void SetTimeSession()
    {
        ServiceLocator.Instance.GetService<GameManager>().SetTimeSession(_inputSessionMinutes.text, _inputSessionSeconds.text);
    }

    /// <summary>Set the time for all minigames passing values to the GM</summary>
    public void SetTimeMinigames()
    {
        ServiceLocator.Instance.GetService<GameManager>().SetTimeMinigames(_inputMinigamesMinutes.text, _inputMinigamesSeconds.text);
    }

    /// <summary>Open/close the window credits</summary>
    public void PopupAddClass()
    {
        ServiceLocator.Instance.GetService<GameManager>()._popupAddClass.SetActive(!ServiceLocator.Instance.GetService<GameManager>()._popupAddClass.activeSelf);
    }

    /// <summary>Open/close the window credits</summary>
    public void PopupDeleteClass(string className = "")
    {
        ServiceLocator.Instance.GetService<GameManager>()._classNameDeleting.text = className;
        EDebug.Log(ServiceLocator.Instance.GetService<GameManager>()._classNameDeleting.text);
        ServiceLocator.Instance.GetService<GameManager>()._popupDeleteClass.SetActive(!ServiceLocator.Instance.GetService<GameManager>()._popupDeleteClass.activeSelf);
    }

    public void BeginDeleteClass()
    {
        for (int i = 0; i < ServiceLocator.Instance.GetService<GameManager>()._classPanel.transform.childCount; ++i)
        {
            ServiceLocator.Instance.GetService<GameManager>()._classPanel.transform.GetChild(i).GetComponent<Vibration>()._deleting = true;
        }
    }
    private void Update()
    {
        //Control the instructions deppending on the game state
        switch (ServiceLocator.Instance.GetService<GameManager>()._gameStateServer)
        {
            case GameManager.GAME_STATE_SERVER.connection:
                //Update the number of tablets that are connected just when something is new connected
                if (ServiceLocator.Instance.GetService<GameManager>().GetUpdateConnectedTablets())
                {
                    _connectedTablets.text = ServiceLocator.Instance.GetService<GameManager>().GetConnectedTablets().ToString();
                    ServiceLocator.Instance.GetService<GameManager>().SetUpdateConnectedTablets(false);
                }
                break;

            case GameManager.GAME_STATE_SERVER.playing:
                ShowCountDown();
                break;
        }

        if (ServiceLocator.Instance.GetService<GameManager>()._popupDeleteClass.activeSelf && !_deleteClassCorrect)
        {

            _deleteClassCorrect = AreEqual(_deletingClassName.text, _writingClassName.text);

        }
        if (_deleteClassCorrect && !_deleteClassButton.interactable)
        {
            _deleteClassButton.interactable = true;
        }
    }

    /// <summary>Show the timer</summary>
    private void ShowCountDown()
    {
        _countdown.text = ServiceLocator.Instance.GetService<GameManager>()._timeSessionMinutes + ":" + ServiceLocator.Instance.GetService<GameManager>()._timeSessionSeconds;
    }

    private bool AreEqual(string val1, string val2)
    {
        if (val1.Length != val2.Length)
            return false;

        for (int i = 0; i < val1.Length; i++)
        {
            var c1 = val1[i];
            var c2 = val2[i];
            if (c1 != c2)
                return false;
        }

        return true;
    }
}
