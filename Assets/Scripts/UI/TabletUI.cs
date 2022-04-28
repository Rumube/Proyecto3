using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Crosstales.RTVoice;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class TabletUI : UI
{   
    public enum TEAMCOLOR
    {
        ROSA = 1,
        AMARILLO = 2,
        NARANJA = 3,
        AZUL = 4,
        MORADO = 5,
        VERDE = 6
    }
    [Header("Color team")]
    public TEAMCOLOR _teamColor;
    public Sprite[] _rocketColors = new Sprite[6];

    [Header("Initial screen")]
    public TMP_InputField _ipServer;
    public TMP_InputField _portServer;
    public GameObject _cantConnectText;
    public VideoPlayer _video;
    public Animator[] _initialButtonAnims = new Animator[3];
    public Animator _conectionBackgroundAnims;
    public Animator _fadeOutInitialScreen;

    [Header("Connection")]
    public TextMeshProUGUI _studentsText;
    public TextMeshProUGUI _idText;
    public TextMeshProUGUI _currentStudentName;
    public Button _continueCallingButton;
    private bool _callingStudents;
    private bool _continuecallingStudent;
    public Animator _astrounautAnimator;
    public Animator _rocketAnimator;
    public Animator _doorsClosed;
    public AudioSource _audioSource;
    public TextMeshProUGUI _toTheRocket;
    public Image _rocket;
    public AudioSource _astronautAudio;
    public AudioSource _doorsClosedAudio;
    public AudioSource _spaceShipAudio;

    [Header("Student selection")]
    public TextMeshProUGUI _studentName;
    public TextMeshProUGUI _teamColorText;
    public GameObject _panelInfo;   

    [Header("Game selection")]
    public GameObject _blackTransition;
    public Animator _doorsOpen;
    // Start is called before the first frame update
    void Start()
    {
        //Get the last info saved
        if (PlayerPrefs.HasKey("IPServer") && PlayerPrefs.HasKey("PortServer"))
        {
            _ipServer.text = PlayerPrefs.GetString("IPServer");
            _portServer.text = PlayerPrefs.GetString("PortServer");
        }
        //Adding root windows in order of appearance
        _windowsTree.Add(ServiceLocator.Instance.GetService<UIManager>()._initialScreenTablet);
        _windowsTree.Add(ServiceLocator.Instance.GetService<UIManager>()._connection);
        _windowsTree.Add(ServiceLocator.Instance.GetService<UIManager>()._studentSelection);
        _windowsTree.Add(ServiceLocator.Instance.GetService<UIManager>()._gameSelection);
        _windowsTree.Add(ServiceLocator.Instance.GetService<UIManager>()._finalScoreTablet);
        //Non root windows
        ServiceLocator.Instance.GetService<UIManager>()._creditsTablet.SetActive(false);

        //Desactive all windows
        for (int i = 0; i < _windowsTree.Count; ++i)
        {
            _windowsTree[i].SetActive(false);
        }
        //If a minigame is finished but not the session, it calls another student
        if (ServiceLocator.Instance.GetService<GameManager>()._returnToCommonScene)
        {
            _uiIndex = 2;
            NewStudentGame();
            ServiceLocator.Instance.GetService<GameManager>()._returnToCommonScene = false;
        }
        //If the time session ends or the teacher decided it, the tablet opens the final score
        if (ServiceLocator.Instance.GetService<GameManager>()._endSessionTablet)
        {
            _uiIndex = 4;
            ServiceLocator.Instance.GetService<GameManager>()._endSessionTablet = false;
            ServiceLocator.Instance.GetService<NetworkManager>().SendViewingFinalScore();

        }
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
        _conectionBackgroundAnims.Play("FadeInConnection");
    }
    public void Alpha100InitialButtons()
    {
        for (int i = 0; i < _initialButtonAnims.Length; ++i)
        {
            _initialButtonAnims[i].enabled = false;
            _initialButtonAnims[i].gameObject.GetComponent<Image>().color = Color.white;
        }
    }
    /// <summary>
    /// Save the values places in the input in order to have direct access to the connection if
    /// server port and ip doesn't change
    /// </summary>
    public void SaveIPPortInfo()
    {
        if(_ipServer.text != PlayerPrefs.GetString("IPServer"))
        {
            PlayerPrefs.SetString("IPServer", _ipServer.text);
        }
        if (_portServer.text != PlayerPrefs.GetString("PortServer"))
        {
            PlayerPrefs.SetString("PortServer", _portServer.text);
        }      
    }

    /// <summary>
    /// When the student's package is received, this starts the calling to the rocket
    /// </summary>
    void Update()
    {
        if (Client._tablet != null && Client._tablet._students != null && Client._tablet._students.Count > 0 && !_callingStudents)
        {
            _callingStudents = true;
            StartCoroutine(CallingStudents());
        }
    }

    /// <summary>
    /// Assigns the color of the team and the rocket deppending on the tablet id
    /// </summary>
    public void AssingTeamColor()
    {
        _idText.text = ((TEAMCOLOR)Client._tablet._id).ToString();
        _rocket.sprite = _rocketColors[Client._tablet._id - 1];
    }

    /// <summary>
    /// Student's call to the rocket. Once everyone has called, send a package to the server
    /// </summary>
    public IEnumerator CallingStudents()
    {
        _toTheRocket.gameObject.SetActive(true);
        for (int i = 0; i < Client._tablet._students.Count; ++i)
        {
            _currentStudentName.text = Client._tablet._students[i]._name;
            Speaker.Instance.Speak(_currentStudentName.text + " a la nave", _audioSource);
            _continuecallingStudent = false;
            _continueCallingButton.gameObject.SetActive(true);
            yield return new WaitUntil(() => _continuecallingStudent == true);
            _continueCallingButton.gameObject.SetActive(false);
            yield return AstronautAnimation();
        }     
        _rocketAnimator.Play("NaveDespegue");
        _spaceShipAudio.Play();
        _doorsClosed.gameObject.SetActive(true);
        _doorsClosed.Play("PuertasTransicionCerrar");
        yield return new WaitForSeconds(3.0f);
        _doorsClosedAudio.Play();
        yield return new WaitForSeconds(3.0f);
        ServiceLocator.Instance.GetService<NetworkManager>().SendEndCalling();
    }

    /// <summary>
    /// Astronaut anim and write the student's name into the panel
    /// </summary>
    IEnumerator AstronautAnimation()
    {
        _astronautAudio.Play();
        _astrounautAnimator.Play("Astronaut");
        yield return new WaitForSeconds(8.0f);
        _astronautAudio.Stop();
        _studentsText.text += _currentStudentName.text + "\n";
    }

    /// <summary>
    /// Continue button enable
    /// </summary>
    public void ButtonContinueCallingStudent()
    {
        _continuecallingStudent = true;
    }

    /// <summary>
    /// Select new student and game
    /// </summary>
    public void NewStudentGame()
    {
        SelectStudentGame();
        ShowStudentSelectGame();
    }

    /// <summary>
    /// Select student and game and send it to the server
    /// </summary>
    void SelectStudentGame()
    {
        ServiceLocator.Instance.GetService<NetworkManager>()._minigameLevel = -1;
        ServiceLocator.Instance.GetService<GameManager>().SelectStudentAndGame();
        ServiceLocator.Instance.GetService<NetworkManager>().SendStudentGame();
    }

    /// <summary>
    /// Shows the info of who is going to play 
    /// </summary>
    void ShowStudentSelectGame()
    {      
        _studentName.text = ServiceLocator.Instance.GetService<GameManager>()._currentstudentName;
        _teamColor = (TEAMCOLOR)(int)Client._tablet._id;
        _teamColorText.text = "EQUIPO " + _teamColor;
    }

    /// <summary>
    /// Shows the common scenario
    /// </summary>
    public void ShowCommonScenario()
    {
        OpenNextWindow();
        _doorsOpen.Play("PuertasTransicionAbrir");
        StartCoroutine(ShowGameSelected());
    }

    /// <summary>
    /// Wait until receives the difficulty from the server and shows the transition to the minigame
    /// </summary>
    IEnumerator ShowGameSelected()
    {
        yield return new WaitUntil(() => ServiceLocator.Instance.GetService<NetworkManager>()._minigameLevel != -1);
        yield return new WaitForSeconds(3.0f);
        _blackTransition.SetActive(true);

        switch (ServiceLocator.Instance.GetService<GameManager>()._currentgameName)
        {
            case "Cabina Geometr�a":
            case "Cabina Espacio/Tiempo":
            case "Cabina Asociaci�n":
            case "Cabina Sumas/Restas":
                _blackTransition.GetComponent<Animator>().Play("BlackScreen_Cabin");
                break;
            case "Telescopio Geometr�a":
            case "Telescopio Espacio/Tiempo":
            case "Telescopio Asociaci�n":
            case "Telescopio Sumas/Restas":
                _blackTransition.GetComponent<Animator>().Play("BlackScreen_Telescope");
                break;
            case "Panel botones Geometr�a":
            case "Panel botones Espacio/Tiempo":
            case "Panel botones Asociaci�n":
            case "Panel botones Sumas/Restas":
                _blackTransition.GetComponent<Animator>().Play("BlackScreen_Button");
                break;
            case "Panel tapa Geometr�a":
            case "Panel tapa Espacio/Tiempo":
            case "Panel tapa Asociaci�n":
            case "Panel tapa Sumas/Restas":
                _blackTransition.GetComponent<Animator>().Play("BlackScreen_Door");
                break;

        }
        yield return new WaitForSeconds(3.0f);
        
        SceneManager.LoadScene("RubenSpaceTimeCabin");
        //Cuando este listo es el de abajo, llamar a los minijuegos tal cual estan aqui
        //SceneManager.LoadScene(ServiceLocator.Instance.GetService<GameManager>()._currentgameName);
        yield return null;
    }
}

