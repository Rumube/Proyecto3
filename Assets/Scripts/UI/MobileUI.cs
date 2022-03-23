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

    [Header("Delete student popup")]
    public Text _deletingStudentName;
    public Text _writingStudentName;
    bool _deleteStudentCorrect;
    public Button _deleteStudentButton;

    [Header("Student stadistics")]
    public GameObject _studentGamePanel;
    public Text _studentNameText;
    public Text _gameNameText;

    [Header("Add student screen")]
    public GameObject _tabletsPanel;
    public GameObject _tabletButton;
    public Dictionary<GameObject, Tablet> _tabletButtonAssociation = new Dictionary<GameObject, Tablet>();

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
        ServiceLocator.Instance.GetService<GameManager>()._popupAddStudent.SetActive(false);
        ServiceLocator.Instance.GetService<GameManager>()._popupDeleteStudent.SetActive(false);

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

    /// <summary>Get the values for the timers if it's not their first time playing. Acts like placeholders</summary>
    public void GetTimesSaved()
    {
        if (PlayerPrefs.HasKey("SessionMinutes") && PlayerPrefs.HasKey("SessionSeconds"))
        {
            _inputSessionMinutes.text = PlayerPrefs.GetString("SessionMinutes");
            _inputSessionSeconds.text = PlayerPrefs.GetString("SessionSeconds");
        }
        if (PlayerPrefs.HasKey("MinigamesMinutes") && PlayerPrefs.HasKey("MinigamesSeconds"))
        {
            _inputMinigamesMinutes.text = PlayerPrefs.GetString("MinigamesMinutes");
            _inputMinigamesSeconds.text = PlayerPrefs.GetString("MinigamesSeconds");
        }
    }
    /// <summary>Set the time for the whole session passing values to the GM</summary>
    public void SetTimeSession()
    {
        ServiceLocator.Instance.GetService<GameManager>().SetTimeSession(_inputSessionMinutes.text, _inputSessionSeconds.text);
        PlayerPrefs.SetString("SessionMinutes", _inputSessionMinutes.text);
        PlayerPrefs.SetString("SessionSeconds", _inputSessionSeconds.text);
    }

    /// <summary>Set the time for all minigames passing values to the GM</summary>
    public void SetTimeMinigames()
    {
        ServiceLocator.Instance.GetService<GameManager>().SetTimeMinigames(_inputMinigamesMinutes.text, _inputMinigamesSeconds.text);
        PlayerPrefs.SetString("MinigamesMinutes", _inputMinigamesMinutes.text);
        PlayerPrefs.SetString("MinigamesSeconds", _inputMinigamesSeconds.text);
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
        ServiceLocator.Instance.GetService<GameManager>()._popupDeleteClass.SetActive(!ServiceLocator.Instance.GetService<GameManager>()._popupDeleteClass.activeSelf);
    }

    /// <summary>Open/close the window credits</summary>
    public void PopupAddStudent()
    {
        ServiceLocator.Instance.GetService<GameManager>()._popupAddStudent.SetActive(!ServiceLocator.Instance.GetService<GameManager>()._popupAddStudent.activeSelf);
    }

    /// <summary>Open/close the window credits</summary>
    public void PopupDeleteStudent(string studentName = "")
    {
        ServiceLocator.Instance.GetService<GameManager>()._studentNameDeleting.text = studentName;
        ServiceLocator.Instance.GetService<GameManager>()._popupDeleteStudent.SetActive(!ServiceLocator.Instance.GetService<GameManager>()._popupDeleteStudent.activeSelf);
    }

    public void BeginDeleteClass()
    {
        for (int i = 0; i < ServiceLocator.Instance.GetService<GameManager>()._classPanel.transform.childCount; ++i)
        {
            ServiceLocator.Instance.GetService<GameManager>()._classPanel.transform.GetChild(i).GetComponent<Vibration>()._deleting = true;
        }
    }

    public void BeginDeleteStudent()
    {
        for (int i = 0; i < ServiceLocator.Instance.GetService<GameManager>()._studentPanel.transform.childCount; ++i)
        {
            ServiceLocator.Instance.GetService<GameManager>()._studentPanel.transform.GetChild(i).GetComponent<Vibration>()._deleting = true;
        }
    }
    /// <summary>Open the panel that shows who is playing in which game</summary>
    /// /// <param name="studentName">student's name that is currently playing</param>
    /// /// <param name="gameName">game's name that played by the student</param>
    public void OpenInfoTabletStudentGamePanel(string studentName, string gameName)
    {
        _studentNameText.text = studentName;
        _gameNameText.text = gameName;
        _studentGamePanel.SetActive(true);
    }

    /// <summary>Closes the panel that shows who is playing in which game</summary>
    public void CloseInfoTabletStudentGamePanel()
    {
        _studentGamePanel.SetActive(false);
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

        //Active the okey bytton when the input text is equal to the name of the class
        if (ServiceLocator.Instance.GetService<GameManager>()._popupDeleteClass.activeSelf && !_deleteClassCorrect)
        {

            _deleteClassCorrect = AreEqual(_deletingClassName.text, _writingClassName.text);

        }
        if (_deleteClassCorrect && !_deleteClassButton.interactable)
        {
            _deleteClassButton.interactable = true;
        }

        //Active the okey bytton when the input text is equal to the name of the student
        if (ServiceLocator.Instance.GetService<GameManager>()._popupDeleteStudent.activeSelf && !_deleteStudentCorrect)
        {

            _deleteStudentCorrect = AreEqual(_deletingStudentName.text, _writingStudentName.text);

        }
        if (_deleteStudentCorrect && !_deleteStudentButton.interactable)
        {
            _deleteStudentButton.interactable = true;
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

    public void QuitHighlightClass()
    {
        for (int i = 0; i < ServiceLocator.Instance.GetService<GameManager>()._classPanel.transform.childCount; ++i)
        {
            ServiceLocator.Instance.GetService<GameManager>()._classPanel.transform.GetChild(i).GetComponentInChildren<Vibration>()._highlighted.enabled = false;
        }
    }

    public void ClassNameForStudentsPanel()
    {
        ServiceLocator.Instance.GetService<GameManager>()._classNameStudents.text = ServiceLocator.Instance.GetService<GameManager>()._classNamedb;
    }

    public void InstantiateTabletsAddStudent()
    {
        for (int i = 0; i < ServiceLocator.Instance.GetService<GameManager>().GetConnectedTablets(); ++i)
        {
            GameObject newButton = Instantiate(_tabletButton,_tabletsPanel.transform);
            newButton.GetComponentInChildren<Text>().text = ServiceLocator.Instance.GetService<GameManager>().GetTablets(i)._id.ToString();
            _tabletButtonAssociation.Add(newButton, ServiceLocator.Instance.GetService<GameManager>().GetTablets(i));
        }
    }
    public void RemoveTabletsAddStudent()
    {
        for (int i = 0; i < _tabletsPanel.transform.childCount; ++i)
        {
            Destroy(_tabletsPanel.transform.GetChild(i));
        }
    }
    public void InstantiateStudentsAddStudent()
    {

    }
}
