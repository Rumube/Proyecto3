using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class MobileUI : UI
{
    [Header("Initial screen")]
    public VideoPlayer _video;
    public Animator[] _initialButtonAnims = new Animator[3];
    public Animator _fadeOutInitialScreen;

    [Header("Main Menu")]
    public Button _continueMainMenu;
    public TextMeshProUGUI _adviceTextMainMenu;
    public Texture _textureVideoMainMenu;
    public Animator _imageMainMenu;
    public VideoPlayer _videoMainMenu;

    [Header("Delete class popup")]
    public TextMeshProUGUI _deletingClassName;
    public TMP_InputField _writingClassName;
    bool _deleteClassCorrect;
    public Button _confirmDeleteClassButton;
    public Button _deleteClass;
    public Sprite _cross;
    public Sprite _bin;
    bool _isDeleting;

    [Header("Add class popup")]
    public TMP_InputField _introducedNameClass;
    public TextMeshProUGUI _numCharactersClass;
    public TextMeshProUGUI _errorAdviceAddClass;

    [Header("Class")]
    public GameObject _loading;

    [Header("Delete student popup")]
    public TextMeshProUGUI _deletingStudentName;
    public TMP_InputField _writingStudentName;
    bool _deleteStudentCorrect;
    public Button _confirmDeleteStudentButton;
    public Button _deleteStudent;
    public TextMeshProUGUI _studentNameWritingDelete;

    [Header("Add student popup")]
    public TMP_InputField _introducedNameStudent;
    public TextMeshProUGUI _numCharactersStudent;
    public TextMeshProUGUI _studentNameWritingAdd;
    public TextMeshProUGUI _errorAdviceAddStudent;

    [Header("Game Connection")]
    public TextMeshProUGUI _ip;
    public TextMeshProUGUI _port;
    public TextMeshProUGUI _connectedTablets;
    public Button _continueGameConnection;

    [Header("Add student screen")]
    public GameObject _tabletsPanel;
    public GameObject _studentsPanel;
    public GameObject _tabletButton;
    public Dictionary<GameObject, Tablet> _tabletButtonAssociation = new Dictionary<GameObject, Tablet>();
    public TextMeshProUGUI _numMininautas;
    public Button _continueButtonAddStudent;

    [Header("Game Timer")]
    public TextMeshProUGUI _sessionMinutes;
    public Button _sessionMinutesPlus;
    public Button _sessionMinutesMinus;
    public TextMeshProUGUI _minigameMinutes;
    public Button _minigamesMinutesPlus;
    public Button _minigamesMinutesMinus;
    public TextMeshProUGUI _minigameSeconds;
    public Button _minigamesSecondsPlus;
    public Button _minigamesSecondsMinus;
    public TextMeshProUGUI _adviceStudents;
    public Button _continueGameTimer;
    public GameObject _rocketsReady;
    public GameObject _rocketReady;
    public Sprite[] _rocketReadySprites;

    [Header("Student stadistics")]
    public TextMeshProUGUI _countdown;
    public GameObject _tabletsPanelStadistics;
    public Button _tabletButtonPrefabStadistics;
    public Button _allTabletsButtonStadistics;
    public GameObject _studentGamePanel;
    public TextMeshProUGUI _studentNameText;
    public TextMeshProUGUI _gameNameText;
    public Button _pauseButton;
    public Sprite _playSprite;
    public Sprite _playPressedSprite;
    public Sprite _pauseSprite;
    public Sprite _pausePressedSprite;

    [Header("Final Score")]
    public TextMeshProUGUI _numTabletsViewingFinalScore;
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
        _continueNextScreen = true;
        //Initial animations
        _fadeOutInitialScreen.Play("InitialScreenFadeOut");
        _video.Play();
        _video.loopPointReached += InitialAnimEndReached;
    }
    void InitialAnimEndReached(VideoPlayer vp)
    {
        for (int i = 0; i < _initialButtonAnims.Length; ++i)
        {
            _initialButtonAnims[i].Play("FadeIn");
        }       
    }
    public void Alpha100InitialButtons()
    {
        for (int i = 0; i < _initialButtonAnims.Length; ++i)
        {
            _initialButtonAnims[i].enabled = false;
           _initialButtonAnims[i].gameObject.GetComponent<Image>().color = Color.white;
        }
    }
    private void Update()
    {
        //Control the instructions deppending on the game state
        switch (ServiceLocator.Instance.GetService<IGameManager>().GetServerState())
        {
            case IGameManager.GAME_STATE_SERVER.connection:
                //Update the number of tablets that are connected just when something is new connected
                if (ServiceLocator.Instance.GetService<NetworkManager>().GetUpdateConnectedTablets())
                {
                    _connectedTablets.text = ServiceLocator.Instance.GetService<NetworkManager>().GetConnectedTablets() + "/6";
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
            case IGameManager.GAME_STATE_SERVER.teamConfiguration:
                if (ServiceLocator.Instance.GetService<NetworkManager>()._selectedTablet == 0 && _studentsPanel.transform.childCount > 0 && _studentsPanel.transform.GetChild(0).GetComponent<Button>().interactable == true)
                {
                    StudentButtonDisableInteractuable();
                } else if (ServiceLocator.Instance.GetService<NetworkManager>()._selectedTablet != 0 && _studentsPanel.transform.childCount > 0 && _studentsPanel.transform.GetChild(0).GetComponent<Button>().interactable == false)
                {
                    StudentButtonAbleInteractuable();
                }
                if (ServiceLocator.Instance.GetService<ServerUtility>()._connectedTablets > _tabletsPanel.transform.childCount)
                {
                    UpdateTabletsAddStudent();
                }
                break;

            case IGameManager.GAME_STATE_SERVER.playing:
                ShowCountDown();
                break;
        }

        //Active the okey button when the input text of deleting is equal to the name of the class
        if (ServiceLocator.Instance.GetService<UIManager>()._popupDeleteClass.activeSelf && !_deleteClassCorrect)
        {
            _deleteClassCorrect = AreEqual(_deletingClassName.text.ToUpper(), _writingClassName.text.ToUpper());
        }
        if (_deleteClassCorrect && !_confirmDeleteClassButton.interactable)
        {
            _confirmDeleteClassButton.interactable = true;
        }

        //Active the okey button when the input text is equal to the name of the student
        if (ServiceLocator.Instance.GetService<UIManager>()._popupDeleteStudent.activeSelf && !_deleteStudentCorrect)
        {
            _deleteStudentCorrect = AreEqual(_deletingStudentName.text.ToUpper(), _writingStudentName.text.ToUpper());
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

    /// <summary>Put together the different methods I use in this button</summary>
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
        _adviceTextMainMenu.gameObject.SetActive(false);
        _continueMainMenu.interactable = true;
    }

    /// <summary>Desactivates the continue button of main menu</summary>
    public void DesctivateContinueMainMenu()
    {
        _adviceTextMainMenu.gameObject.SetActive(true);
        _continueMainMenu.interactable = false;
    }

    /// <summary>Desactivates the okey delete class button</summary>
    public void DesactivateOkeyDeleteClassButton()
    {
        _confirmDeleteClassButton.interactable = false;
    }

    /// <summary>Deselect a class</summary>
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
        _errorAdviceAddClass.gameObject.SetActive(false);
        ServiceLocator.Instance.GetService<UIManager>()._popupAddClass.SetActive(!ServiceLocator.Instance.GetService<UIManager>()._popupAddClass.activeSelf);
    }

    /// <summary>Shows how many characters has the class name</summary>
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
    public void AddingTwoClassesWithSameName()
    {
        _errorAdviceAddClass.gameObject.SetActive(true);
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
        _errorAdviceAddStudent.gameObject.SetActive(false);
        ServiceLocator.Instance.GetService<UIManager>()._popupAddStudent.SetActive(!ServiceLocator.Instance.GetService<UIManager>()._popupAddStudent.activeSelf);
    }

    /// <summary>Shows how many character has the name</summary>
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

    /// <summary>Shows clearly the name that is writting</summary>
    public void UpdateNameShownDeleteStudent()
    {
        _studentNameWritingDelete.text = _writingStudentName.text;
    }

    /// <summary>Desactivates the student delete button</summary>
    public void DesactivateOkeyDeleteStudentutton()
    {
        _confirmDeleteStudentButton.interactable = false;
    }

    /// <summary>Put together the different methods I use in this button</summary>
    public void OkeyDeleteStudent()
    {
        DesactivateOkeyDeleteStudentutton();
        PopupDeleteStudent();
        BeginDeleteStudent();
    }

    /// <summary>Open/close the popup delete student and assing the name we want to delete</summary>
    /// <param name="studentName">The name we want to delete, it's for having a doble check if she is sure of deleting that</param>
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
    public void AddingTwoStudentsWithSameName()
    {
        _errorAdviceAddStudent.gameObject.SetActive(true);
    }

    public void ShowLoading()
    {
        //_loading.SetActive(true);
        StartCoroutine(Loading());
    }
    IEnumerator Loading()
    {
        
        _loading.SetActive(true);
        yield return new WaitForSeconds(2f);

        yield return null;
    }
    public void HideLoading()
    {
        _loading.SetActive(false);
    }
    #endregion
    #region GameConnection
    /// <summary>Show the IP and port from the device</summary>
    public void GetIpPort()
    {
        _ip.text = "IP: " + ServiceLocator.Instance.GetService<INetworkManager>()._ip;
        _port.text = "Puerto: " + ServiceLocator.Instance.GetService<INetworkManager>()._port;
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
        ServiceLocator.Instance.GetService<IGameManager>().SetServerState(IGameManager.GAME_STATE_SERVER.teamConfiguration);

        for (int i = 0; i < ServiceLocator.Instance.GetService<NetworkManager>().GetConnectedTablets(); ++i)
        {
            GameObject newButton = Instantiate(_tabletButton, _tabletsPanel.transform);
            newButton.GetComponent<TabletButton>().index_rocket = i;
            newButton.GetComponentInChildren<Image>().sprite = newButton.GetComponent<TabletButton>()._rocketSprites[i];
            _tabletButtonAssociation.Add(newButton, ServiceLocator.Instance.GetService<NetworkManager>().GetTablets(i));// a lo mejor no lo necesito
            ServiceLocator.Instance.GetService<NetworkManager>()._studentsToTablets.Add(ServiceLocator.Instance.GetService<NetworkManager>().GetTablets(i));
        }
    }

    /// <summary>Add new tablet on Add student deppending on the number of clients that are newly connected</summary>
    public void UpdateTabletsAddStudent()
    {
        GameObject newButton = Instantiate(_tabletButton, _tabletsPanel.transform);
        newButton.GetComponentInChildren<TextMeshProUGUI>().text = ServiceLocator.Instance.GetService<NetworkManager>().GetTablets(ServiceLocator.Instance.GetService<NetworkManager>().GetConnectedTablets() - 1)._id.ToString();
        _tabletButtonAssociation.Add(newButton, ServiceLocator.Instance.GetService<NetworkManager>().GetTablets(ServiceLocator.Instance.GetService<NetworkManager>().GetConnectedTablets()));// a lo mejor no lo necesito
        ServiceLocator.Instance.GetService<NetworkManager>()._studentsToTablets.Add(ServiceLocator.Instance.GetService<NetworkManager>().GetTablets(ServiceLocator.Instance.GetService<NetworkManager>().GetConnectedTablets() - 1));
    }

    /// <summary>If we go back destroy all the tablets and clear the lists of the association between tablets and students </summary>
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

    /// <summary>Quit the highlight for every tablet in add student</summary>
    public void QuitHighlightTablets()
    {
        for (int i = 0; i < _tabletsPanel.transform.childCount; ++i)
        {
            _tabletsPanel.transform.GetChild(i).GetComponentInChildren<TabletButton>()._highlighted.gameObject.SetActive(false);
        }
    }

    /// <summary>Shows the number of mininautas that ahas a specific tablet</summary>
    public void UpdateNumberMininautas()
    {
        if (ServiceLocator.Instance.GetService<NetworkManager>()._selectedTablet > 0)
        {
            _numMininautas.text = ServiceLocator.Instance.GetService<NetworkManager>()._studentsToTablets[ServiceLocator.Instance.GetService<NetworkManager>()._selectedTablet - 1]._students.Count + "/12";

        }
        else
        {
            _numMininautas.text = "0/12";
        }
    }

    /// <summary>Disable the student button of add student screen</summary>
    public void StudentButtonDisableInteractuable()
    {
        for (int i = 0; i < _studentsPanel.transform.childCount; ++i)
        {
            _studentsPanel.transform.GetChild(i).GetComponentInChildren<Button>().interactable = false;
        }
    }

    /// <summary>Enable the student button of add student screen</summary>
    public void StudentButtonAbleInteractuable()
    {
        for (int i = 0; i < _studentsPanel.transform.childCount; ++i)
        {
            _studentsPanel.transform.GetChild(i).GetComponentInChildren<Button>().interactable = true;
        }
    }

    /// <summary>Enable/disable the continue button of add student screen</summary>
    public void ContinueButtonAddStudent(bool interactuable)
    {
        _continueButtonAddStudent.interactable = interactuable;
    }
    #endregion

    #region GameTime
    /// <summary>Get the values for the timers if it's not their first time playing. Acts like placeholders</summary>
    public void GetTimesSaved()
    {
        if (PlayerPrefs.HasKey("SessionMinutes"))
        {
            _sessionMinutes.text = PlayerPrefs.GetString("SessionMinutes");
        }
        if (PlayerPrefs.HasKey("MinigamesMinutes") && PlayerPrefs.HasKey("MinigamesSeconds"))
        {
            _minigameMinutes.text = PlayerPrefs.GetString("MinigamesMinutes");
            _minigameSeconds.text = PlayerPrefs.GetString("MinigamesSeconds");
        }
    }
    /// <summary>Set the time for the whole session passing values to the GM</summary>
    public void SetTimeSession()
    {
        ServiceLocator.Instance.GetService<UIManager>().SetTimeSession(_sessionMinutes.text);
        PlayerPrefs.SetString("SessionMinutes", _sessionMinutes.text);
    }

    /// <summary>Set the time for all minigames passing values to the GM</summary>
    public void SetTimeMinigames()
    {
        ServiceLocator.Instance.GetService<UIManager>().SetTimeMinigames(_minigameMinutes.text, _minigameSeconds.text);
        PlayerPrefs.SetString("MinigamesMinutes", _minigameMinutes.text);
        PlayerPrefs.SetString("MinigamesSeconds", _minigameSeconds.text);
    }
    #region Buttons
    public void SessionMinutesPlus()
    {
        int minutes;
        if (int.TryParse(_sessionMinutes.text, out minutes))
        {
            if ((minutes + 1) <= 99)
            {
                _sessionMinutes.text = (minutes + 1).ToString();
            }
        }
       
    }
    public void SessionMinutesMinus()
    {
        int minutes;
        if (int.TryParse(_sessionMinutes.text, out minutes))
        {
            if ((minutes - 1) >= 0)
            {
                _sessionMinutes.text = (minutes - 1).ToString();
            }           
        }
    }
    public void MinigameMinutesPlus()
    {
        int minutes;
        if (int.TryParse(_minigameMinutes.text, out minutes))
        {
            if ((minutes + 1) <= 99)
            {
                _minigameMinutes.text = (minutes + 1).ToString();
            }
            
        }
    }
    public void MinigameMinutesMinus()
    {
        int minutes;
        if (int.TryParse(_minigameMinutes.text, out minutes))
        {
            if((minutes - 1) >= 0)
            {
                _minigameMinutes.text = (minutes - 1).ToString();
            }          
        }
    }
    public void MinigameSecondsPlus()
    {
        int seconds;
        int minutes;
        if (int.TryParse(_minigameSeconds.text, out seconds))
        {
            if ((seconds + 15) < 60)
            {
                _minigameSeconds.text = (seconds + 15).ToString();
            }else if ((seconds + 15) == 60)
            {
                if (int.TryParse(_minigameMinutes.text, out minutes))
                {
                    if(minutes <= 99)
                    {
                        _minigameMinutes.text = (minutes + 1).ToString();
                    }                    
                }
            }
            
        }
    }
    public void MinigameSecondsMinus()
    {
        int seconds;
        if (int.TryParse(_minigameSeconds.text, out seconds))
        {
            if ((seconds-15) >= 0 )
            {
                _minigameSeconds.text = (seconds - 15).ToString();
            }
            
        }
    }
    #endregion

    public void CreateReadyRockets()
    {
        for (int i = 0; i < ServiceLocator.Instance.GetService<ServerUtility>()._connectedTablets; ++i)
        {
            GameObject newRocket = Instantiate(_rocketReady,_rocketsReady.transform);
            newRocket.GetComponent<Image>().sprite = _rocketReadySprites[i];
            newRocket.GetComponent<Image>().color = new Color(newRocket.GetComponent<Image>().color.r, newRocket.GetComponent<Image>().color.g, newRocket.GetComponent<Image>().color.b,0.5f);
        }
    }
    public void UpdateReadyRockets(int id)
    {
        id -= 1;
        _rocketsReady.transform.GetChild(id).GetComponent<Image>().color = new Color(_rocketsReady.transform.GetChild(id).GetComponent<Image>().color.r, _rocketsReady.transform.GetChild(id).GetComponent<Image>().color.g, _rocketsReady.transform.GetChild(id).GetComponent<Image>().color.b, 1);
    }
    #endregion

    #region Stadistics

    /// <summary>Show the timer</summary>
    private void ShowCountDown()
    {
        int secondsCorrected = ServiceLocator.Instance.GetService<UIManager>()._timeSessionSeconds;
        if (secondsCorrected < 10)
        {
            _countdown.text = ServiceLocator.Instance.GetService<UIManager>()._timeSessionMinutes + ":0" + secondsCorrected;
        }
        else
        {
            _countdown.text = ServiceLocator.Instance.GetService<UIManager>()._timeSessionMinutes + ":" + secondsCorrected;
        }
       
    }

    /// <summary>Quit the highlight of every tablet in stadistics screen</summary>
    public void QuitHighlightTabletsStadistics()
    {
        _allTabletsButtonStadistics.GetComponentInChildren<TabletButton>()._highlighted.gameObject.SetActive(false);
        for (int i = 0; i < _tabletsPanelStadistics.transform.childCount; ++i)
        {
            _tabletsPanelStadistics.transform.GetChild(i).GetComponentInChildren<TabletButton>()._highlighted.gameObject.SetActive(false);
        }
    }

    /// <summary>Open the panel that shows who is playing in which game</summary>
    /// <param name="studentName">student's name that is currently playing</param>
    /// <param name="gameName">game's name that played by the student</param>
    public void OpenInfoTabletStudentGamePanel(int index)
    {
        _studentNameText.text = ServiceLocator.Instance.GetService<NetworkManager>()._studentsToTablets[index - 1]._currentStudent;
        _gameNameText.text = ServiceLocator.Instance.GetService<NetworkManager>()._studentsToTablets[index - 1]._currentGame;
        _studentGamePanel.SetActive(true);
    }

    /// <summary>Closes the panel that shows who is playing in which game</summary>
    public void CloseInfoTabletStudentGamePanel()
    {
        _studentGamePanel.SetActive(false);
    }

    /// <summary>Shows the final score screen and send a package to do the same in clients</summary>
    public void ShownFinalScoreScreen()
    {
        ServiceLocator.Instance.GetService<ServerUtility>().FinishSession();
        OpenNextWindow();
    }

    /// <summary>Pause/unpause the session and change the button's sprite</summary>
    public void PauseGame()
    {
        ServiceLocator.Instance.GetService<IGameManager>().SetPause(!ServiceLocator.Instance.GetService<IGameManager>().GetPause());
        ServiceLocator.Instance.GetService<ServerUtility>().PauseSession(ServiceLocator.Instance.GetService<IGameManager>().GetPause());
        if (ServiceLocator.Instance.GetService<IGameManager>().GetPause())
        {
            _pauseButton.gameObject.GetComponent<Image>().sprite = _playSprite;
        }
        else
        {
            _pauseButton.gameObject.GetComponent<Image>().sprite = _pauseSprite;
        }
    }
    #endregion

    #region FinalScore
    /// <summary>Turn off the devices</summary>
    public void TurnOffAll()
    {
        ServiceLocator.Instance.GetService<ServerUtility>().TurnOff();
        QuitGame();
    }

    public void UpdateNumberTabletsLookingFinalScore()
    {
        _numTabletsViewingFinalScore.text = ServiceLocator.Instance.GetService<ServerUtility>()._numberTabletsViewFinalScore + "/" + ServiceLocator.Instance.GetService<ServerUtility>()._connectedTablets;
     }
    #endregion
}