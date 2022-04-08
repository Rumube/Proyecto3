using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MobileUI : UI
{
    [Header("Main Menu")]
    public Button _continueMainMenu;

    [Header("Delete class popup")]
    public Text _deletingClassName;
    public InputField _writingClassName;
    bool _deleteClassCorrect;
    public Button _confirmDeleteClassButton;
    public Button _deleteClass;
    public Sprite _cross;
    public Sprite _bin;
    bool _isDeleting;

    [Header("Add class popup")]
    public InputField _introducedNameClass;
    public Text _numCharactersClass;

    [Header("Delete student popup")]
    public Text _deletingStudentName;
    public InputField _writingStudentName;
    bool _deleteStudentCorrect;
    public Button _confirmDeleteStudentButton;
    public Button _deleteStudent;
    public Text _studentNameWritingDelete;

    [Header("Add student popup")]
    public InputField _introducedNameStudent;
    public Text _numCharactersStudent;
    public Text _studentNameWritingAdd;

    [Header("Game Connection")]
    public Text _ip;
    public Text _port;
    public Text _connectedTablets;
    public Button _continueGameConnection;

    [Header("Add student screen")]
    public GameObject _tabletsPanel;
    public GameObject _studentsPanel;
    public GameObject _tabletButton;
    public Dictionary<GameObject, Tablet> _tabletButtonAssociation = new Dictionary<GameObject, Tablet>();
    public Text _numMininautas;

    [Header("Game Timer")]
    public InputField _inputSessionMinutes;
    public InputField _inputSessionSeconds;
    public InputField _inputMinigamesMinutes;
    public InputField _inputMinigamesSeconds;
    public Text _countdown;
    public Text _adviceStudents;
    public Button _continueGameTimer;

    [Header("Student stadistics")]
    public GameObject _studentGamePanel;
    public Text _studentNameText;
    public Text _gameNameText;

    void Start()
    {
        //Adding root windows in order of appearance
        _windowsTree.Add(ServiceLocator.Instance.GetService<UIManager>()._initialScreen);
        _windowsTree.Add(ServiceLocator.Instance.GetService<UIManager>()._mainMenu);
        _windowsTree.Add(ServiceLocator.Instance.GetService<UIManager>()._class);
        _windowsTree.Add(ServiceLocator.Instance.GetService<UIManager>()._gameConnection);
        _windowsTree.Add(ServiceLocator.Instance.GetService<UIManager>()._addStudent);
        _windowsTree.Add(ServiceLocator.Instance.GetService<UIManager>()._gameTime);
        _windowsTree.Add(ServiceLocator.Instance.GetService<UIManager>()._stadistics);
        _windowsTree.Add(ServiceLocator.Instance.GetService<UIManager>()._finalScore);

        //Desactive all windows
        for (int i = 0; i < _windowsTree.Count; ++i)
        {
            _windowsTree[i].SetActive(false);
        }
        //Non root windows
        ServiceLocator.Instance.GetService<UIManager>()._credits.SetActive(false);
        ServiceLocator.Instance.GetService<UIManager>()._popupAddClass.SetActive(false);
        ServiceLocator.Instance.GetService<UIManager>()._popupDeleteClass.SetActive(false);
        ServiceLocator.Instance.GetService<UIManager>()._popupAddStudent.SetActive(false);
        ServiceLocator.Instance.GetService<UIManager>()._popupDeleteStudent.SetActive(false);

        _deleteClassCorrect = false;
        _deleteStudentCorrect = false;

        //Active just the first one
        _windowsTree[_uiIndex].SetActive(true);
    }

    private void Update()
    {
        //Control the instructions deppending on the game state
        switch (ServiceLocator.Instance.GetService<GameManager>()._gameStateServer)
        {
            case GameManager.GAME_STATE_SERVER.connection:
                //Update the number of tablets that are connected just when something is new connected
                if (ServiceLocator.Instance.GetService<NetworkManager>().GetUpdateConnectedTablets())
                {
                    _connectedTablets.text = ServiceLocator.Instance.GetService<NetworkManager>().GetConnectedTablets().ToString();
                    ServiceLocator.Instance.GetService<NetworkManager>().SetUpdateConnectedTablets(false);
                }
                if (ServiceLocator.Instance.GetService<NetworkManager>().GetConnectedTablets() > 0)
                {
                    ActivateContinueGameConnection();
                }
                else
                {
                    DesctivateContinueGameConnection();
                }
                break;
            case GameManager.GAME_STATE_SERVER.teamConfiguration:
                if (ServiceLocator.Instance.GetService<NetworkManager>()._selectedTablet == 0 && _studentsPanel.transform.childCount > 0 && _studentsPanel.transform.GetChild(0).GetComponent<Button>().interactable == true)
                {
                    StudentButtonDisableInteractuable();
                }else if (ServiceLocator.Instance.GetService<NetworkManager>()._selectedTablet != 0 && _studentsPanel.transform.childCount > 0 && _studentsPanel.transform.GetChild(0).GetComponent<Button>().interactable == false)
                {
                    StudentButtonAbleInteractuable();
                }
                if (ServiceLocator.Instance.GetService<ServerUtility>()._connectedTablets > _tabletsPanel.transform.childCount)
                {
                    UpdateTabletsAddStudent();
                }
                break;

            case GameManager.GAME_STATE_SERVER.playing:
                ShowCountDown();
                break;
        }

        //Active the okey button when the input text of deleting is equal to the name of the class
        if (ServiceLocator.Instance.GetService<UIManager>()._popupDeleteClass.activeSelf && !_deleteClassCorrect)
        {
            _deleteClassCorrect = AreEqual(_deletingClassName.text, _writingClassName.text);
        }
        if (_deleteClassCorrect && !_confirmDeleteClassButton.interactable)
        {
            _confirmDeleteClassButton.interactable = true;
        }

        //Active the okey button when the input text is equal to the name of the student
        if (ServiceLocator.Instance.GetService<UIManager>()._popupDeleteStudent.activeSelf && !_deleteStudentCorrect)
        {
            _deleteStudentCorrect = AreEqual(_deletingStudentName.text, _writingStudentName.text);
        }
        if (_deleteStudentCorrect && !_confirmDeleteStudentButton.interactable)
        {
            _confirmDeleteStudentButton.interactable = true;
        }
    }

    /// <summary>Cheks if both string are the same</summary>
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

    #region Mainmenu
    public void OkeyDeleteClass()
    {
        DesactivateOkeyDeleteClassButton();
        DeselectClass();
        PopupDeleteClass();
        BeginDeleteClass();
    }
    /// <summary>Activates the continue button of main menu</summary>
    public void ActivateContinueMainMenu()
    {
        _continueMainMenu.interactable = true;
    }

    /// <summary>Desactivates the continue button of main menu</summary>
    public void DesctivateContinueMainMenu()
    {
        _continueMainMenu.interactable = false;
    }
    public void DesactivateOkeyDeleteClassButton()
    {
        _confirmDeleteClassButton.interactable = false;
    }
    public void DeselectClass()
    {
        ServiceLocator.Instance.GetService<UIManager>()._classNameStudents.text = "";
        ServiceLocator.Instance.GetService<UIManager>()._classNamedb = "";
        DesctivateContinueMainMenu();
    }
    /// <summary>Open/close the popup add class</summary>
    public void PopupAddClass()
    {      
        _introducedNameClass.text = "";
        ServiceLocator.Instance.GetService<UIManager>()._popupAddClass.SetActive(!ServiceLocator.Instance.GetService<UIManager>()._popupAddClass.activeSelf);
    }
    public void UpdateCharactersShownAddClass()
    {
        if (_introducedNameClass.text.Length > 6)
        {
            _numCharactersClass.color = Color.red;
        }
        else
        {
            _numCharactersClass.color = Color.black;
        }
        _numCharactersClass.text = _introducedNameClass.text.Length + "/9";
    }
    /// <summary>Open/close the popup delete class and assing the name of the class we want to delete</summary>
    /// <param name="className">The name of the class we want to delete</param>
    public void PopupDeleteClass(string className = "")
    {
        DesactivateOkeyDeleteClassButton();
        _deleteClassCorrect = false;
        _writingClassName.text = "";
        ServiceLocator.Instance.GetService<UIManager>()._classNameDeleting.text = className;
        ServiceLocator.Instance.GetService<UIManager>()._popupDeleteClass.SetActive(!ServiceLocator.Instance.GetService<UIManager>()._popupDeleteClass.activeSelf);
    }

    /// <summary>Change the sprite of the bin and makes the order of starts or stops the buttons animation</summary>
    public void BeginDeleteClass()
    {
        if (!_isDeleting)
        {
            _isDeleting = true;
            for (int i = 0; i < ServiceLocator.Instance.GetService<UIManager>()._classPanel.transform.childCount; ++i)
            {
                ServiceLocator.Instance.GetService<UIManager>()._classPanel.transform.GetChild(i).GetComponent<ClassButton>()._deleting = true;
            }
            _deleteClass.GetComponent<Image>().sprite = _cross;
        }
        else
        {
            _isDeleting = false;
            for (int i = 0; i < ServiceLocator.Instance.GetService<UIManager>()._classPanel.transform.childCount; ++i)
            {
                ServiceLocator.Instance.GetService<UIManager>()._classPanel.transform.GetChild(i).GetComponent<ClassButton>()._deleting = false;
            }
            _deleteClass.GetComponent<Image>().sprite = _bin;
        }
    }
    /// <summary>Quit the highlight for every class</summary>
    public void QuitHighlightClass()
    {
        for (int i = 0; i < ServiceLocator.Instance.GetService<UIManager>()._classPanel.transform.childCount; ++i)
        {
            ServiceLocator.Instance.GetService<UIManager>()._classPanel.transform.GetChild(i).GetComponentInChildren<ClassButton>()._highlighted.gameObject.SetActive(false);
        }
    }
    #endregion
    #region Class
    /// <summary>Shows the name of the classroom inside class window</summary>
    public void ClassNameForStudentsPanel()
    {
        ServiceLocator.Instance.GetService<UIManager>()._classNameStudents.text = ServiceLocator.Instance.GetService<UIManager>()._classNamedb;
    }

    /// <summary>Open/close the popup add student</summary>
    public void PopupAddStudent()
    {
        _introducedNameStudent.text = "";
        ServiceLocator.Instance.GetService<UIManager>()._popupAddStudent.SetActive(!ServiceLocator.Instance.GetService<UIManager>()._popupAddStudent.activeSelf);
    }
    public void UpdateCharactersShownAddStudent()
    {
        if (_introducedNameStudent.text.Length > 30)
        {
            _numCharactersStudent.color = Color.red;
        }
        else
        {
            _numCharactersStudent.color = Color.black;
        }
        _numCharactersStudent.text = _introducedNameStudent.text.Length + "/36";
        _studentNameWritingAdd.text = _introducedNameStudent.text;
    }
    public void UpdateNameShownDeleteStudent()
    {
        _studentNameWritingDelete.text = _writingStudentName.text;
    }
    public void DesactivateOkeyDeleteStudentutton()
    {
        _confirmDeleteStudentButton.interactable = false;
    }
    public void OkeyDeleteStudent()
    {
        DesactivateOkeyDeleteStudentutton();
        PopupDeleteStudent();
        BeginDeleteStudent();
    }
    /// <summary>Open/close the popup delete student and assing the name we want to delete</summary>
    /// /// <param name="studentName">The name we want to delete, it's for having a doble check if she is sure of deleting that</param>
    public void PopupDeleteStudent(string studentName = "")
    {
        DesactivateOkeyDeleteStudentutton();
        _writingStudentName.text = "";
        _deleteStudentCorrect = false;
        ServiceLocator.Instance.GetService<UIManager>()._studentNameDeleting.text = studentName;
        ServiceLocator.Instance.GetService<UIManager>()._popupDeleteStudent.SetActive(!ServiceLocator.Instance.GetService<UIManager>()._popupDeleteStudent.activeSelf);
    }

    /// <summary>Change the sprite of the bin and makes the order of starts or stops the buttons animation</summary>
    public void BeginDeleteStudent()
    {
        if (!_isDeleting)
        {
            _isDeleting = true;
            for (int i = 0; i < ServiceLocator.Instance.GetService<UIManager>()._studentPanel.transform.childCount; ++i)
            {
                ServiceLocator.Instance.GetService<UIManager>()._studentPanel.transform.GetChild(i).GetComponent<StudentButton>()._deleting = true;
            }
            _deleteStudent.GetComponent<Image>().sprite = _cross;
        }
        else
        {
            _isDeleting = false;
            for (int i = 0; i < ServiceLocator.Instance.GetService<UIManager>()._studentPanel.transform.childCount; ++i)
            {
                ServiceLocator.Instance.GetService<UIManager>()._studentPanel.transform.GetChild(i).GetComponent<StudentButton>()._deleting = false;
            }
            _deleteStudent.GetComponent<Image>().sprite = _bin;
        }
    }
    #endregion
    #region GameConnection
    /// <summary>Show the IP and port from the device</summary>
    public void GetIpPort()
    {
        _ip.text = "IP: " + ServiceLocator.Instance.GetService<NetworkManager>()._ip;
        _port.text = "Puerto: " + ServiceLocator.Instance.GetService<NetworkManager>()._port;
    }
    /// <summary>Activates the continue button of Game connection</summary>
    public void ActivateContinueGameConnection()
    {
        if (!_continueGameConnection.interactable)
        {
            _continueGameConnection.interactable = true;
        }
    }

    /// <summary>Desactivates the continue button of Game connection</summary>
    public void DesctivateContinueGameConnection()
    {
        if (_continueGameConnection.interactable)
        {
            _continueGameConnection.interactable = false;
        }        
    }
    #endregion
    #region AddStudent
    /// <summary>
    /// Change the gamestate server to TeamConfiguration and create as many tablet buttons as connected. Also shows their id and 
    /// assing that in the _studentsToTablets list
    /// </summary>
    public void InstantiateTabletsAddStudent()
    {
        ServiceLocator.Instance.GetService<GameManager>()._gameStateServer = GameManager.GAME_STATE_SERVER.teamConfiguration;

        for (int i = 0; i < ServiceLocator.Instance.GetService<NetworkManager>().GetConnectedTablets(); ++i)
        {
            GameObject newButton = Instantiate(_tabletButton,_tabletsPanel.transform);
            newButton.GetComponentInChildren<Text>().text = ServiceLocator.Instance.GetService<NetworkManager>().GetTablets(i)._id.ToString();
            _tabletButtonAssociation.Add(newButton, ServiceLocator.Instance.GetService<NetworkManager>().GetTablets(i));// a lo mejor no lo necesito
            ServiceLocator.Instance.GetService<NetworkManager>()._studentsToTablets.Add(ServiceLocator.Instance.GetService<NetworkManager>().GetTablets(i));
        }
    }
    public void UpdateTabletsAddStudent()
    {
        GameObject newButton = Instantiate(_tabletButton, _tabletsPanel.transform);
        newButton.GetComponentInChildren<Text>().text = ServiceLocator.Instance.GetService<NetworkManager>().GetTablets(ServiceLocator.Instance.GetService<NetworkManager>().GetConnectedTablets() - 1)._id.ToString();
        _tabletButtonAssociation.Add(newButton, ServiceLocator.Instance.GetService<NetworkManager>().GetTablets(ServiceLocator.Instance.GetService<NetworkManager>().GetConnectedTablets()));// a lo mejor no lo necesito
        ServiceLocator.Instance.GetService<NetworkManager>()._studentsToTablets.Add(ServiceLocator.Instance.GetService<NetworkManager>().GetTablets(ServiceLocator.Instance.GetService<NetworkManager>().GetConnectedTablets() - 1));
    }
    /// <summary>If we go back destroy all the tablets and clear the lists of the association between tablets and students
    /// </summary>
    public void RemoveTabletsAddStudent()
    {
        int numTablets = _tabletsPanel.transform.childCount;
        for (int i = 0; i < numTablets; ++i)
        {
            Destroy(_tabletsPanel.transform.GetChild(i).gameObject);
            ServiceLocator.Instance.GetService<NetworkManager>()._studentsToTablets[i]._students.Clear();
        }

        int numStudents = _studentsPanel.transform.childCount;
        for (int i = 0; i < numStudents; ++i)
        {
            Destroy(_studentsPanel.transform.GetChild(i).gameObject);
        }

        _tabletButtonAssociation.Clear();
        ServiceLocator.Instance.GetService<NetworkManager>()._studentsToTablets.Clear();
    }

    /// <summary>Quit the highlight for every tablet</summary>
    public void QuitHighlightTablets()
    {
        for (int i = 0; i < _tabletsPanel.transform.childCount; ++i)
        {
            _tabletsPanel.transform.GetChild(i).GetComponentInChildren<TabletButton>()._highlighted.gameObject.SetActive(false);
        }
    }

    public void UpdateNumberMininautas()
    {
        if (ServiceLocator.Instance.GetService<NetworkManager>()._selectedTablet > 0)
        {
            _numMininautas.text = ServiceLocator.Instance.GetService<NetworkManager>()._studentsToTablets[ServiceLocator.Instance.GetService<NetworkManager>()._selectedTablet - 1]._students.Count.ToString();

        }
        else
        {
            _numMininautas.text = "0";
        }
    }

    public void StudentButtonDisableInteractuable()
    {
        for (int i = 0; i < _studentsPanel.transform.childCount; ++i)
        {
            _studentsPanel.transform.GetChild(i).GetComponentInChildren<Button>().interactable = false;
        }
    }
    public void StudentButtonAbleInteractuable()
    {
        for (int i = 0; i < _studentsPanel.transform.childCount; ++i)
        {
            _studentsPanel.transform.GetChild(i).GetComponentInChildren<Button>().interactable = true;
        }
    }
    #endregion
    #region GameTime
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

        ServiceLocator.Instance.GetService<UIManager>().SetTimeSession(_inputSessionMinutes.text, _inputSessionSeconds.text);
        PlayerPrefs.SetString("SessionMinutes", _inputSessionMinutes.text);
        PlayerPrefs.SetString("SessionSeconds", _inputSessionSeconds.text);
    }

    /// <summary>Set the time for all minigames passing values to the GM</summary>
    public void SetTimeMinigames()
    {
        ServiceLocator.Instance.GetService<UIManager>().SetTimeMinigames(_inputMinigamesMinutes.text, _inputMinigamesSeconds.text);
        PlayerPrefs.SetString("MinigamesMinutes", _inputMinigamesMinutes.text);
        PlayerPrefs.SetString("MinigamesSeconds", _inputMinigamesSeconds.text);
    }
    #endregion
    #region Stadistics
    /// <summary>Show the timer</summary>
    private void ShowCountDown()
    {
        _countdown.text = ServiceLocator.Instance.GetService<UIManager>()._timeSessionMinutes + ":" + ServiceLocator.Instance.GetService<UIManager>()._timeSessionSeconds;
    }
    /// <summary>Open the panel that shows who is playing in which game</summary>
    /// <param name="studentName">student's name that is currently playing</param>
    /// <param name="gameName">game's name that played by the student</param>
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
    #endregion
    public void InstantiateStudentsAddStudent()
    {

    }
}
