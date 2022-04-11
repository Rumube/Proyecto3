using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Crosstales.RTVoice;
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
    public InputField _ipServer;
    public InputField _portServer;

    [Header("Connection")]
    public Text _studentsText;
    public Text _idText;
    public Text _currentStudentName;
    public Button _continueCallingButton;
    private bool _callingStudents;
    private bool _continuecallingStudent;
    public Animator _astrounautAnimator;
    public Animator _rocketAnimator;
    public Animator _doorsClosed;
    public AudioSource _audioSource;
    public Text _toTheRocket;
    public Image _rocket;

    [Header("Student selection")]
    public Text _studentName;
    public Text _teamColorText;
    public GameObject _panelInfo;   
    //public Text _gameName;
    //public Text _gameInfo;

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
        //Non root windows
        ServiceLocator.Instance.GetService<UIManager>()._creditsTablet.SetActive(false);

        //Desactive all windows
        for (int i = 0; i < _windowsTree.Count; ++i)
        {
            _windowsTree[i].SetActive(false);
        }
        //Active just the first one
        _windowsTree[_uiIndex].SetActive(true);
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
    // Update is called once per frame
    void Update()
    {
        if (Client._tablet != null && Client._tablet._students != null && Client._tablet._students.Count > 0 && !_callingStudents)
        {
            _callingStudents = true;
            StartCoroutine(CallingStudents());
        }
    }

    public void AssingTeamColor()
    {
        _idText.text = ((TEAMCOLOR)Client._tablet._id).ToString();
        _rocket.sprite = _rocketColors[Client._tablet._id - 1];
    }

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
        _doorsClosed.gameObject.SetActive(true);
        _doorsClosed.Play("PuertasTransicionCerrar");
        yield return new WaitForSeconds(4.0f);
        ServiceLocator.Instance.GetService<NetworkManager>().SendEndCalling();
    }
    IEnumerator AstronautAnimation()
    {
        _astrounautAnimator.Play("Astronaut");
        yield return new WaitForSeconds(8.0f);
        _studentsText.text += _currentStudentName.text + "\n";
    }
    public void ButtonContinueCallingStudent()
    {
        _continuecallingStudent = true;
    }

    public void NewStudentGame()
    {
        SelectStudentGame();
        ShowStudentSelectGame();
    }
    void SelectStudentGame()
    {
        ServiceLocator.Instance.GetService<NetworkManager>()._minigameLevel = -1;
        ServiceLocator.Instance.GetService<GameManager>().SelectStudentAndGame();
        ServiceLocator.Instance.GetService<NetworkManager>().SendStudentGame();
    }
    void ShowStudentSelectGame()
    {      
        _studentName.text = ServiceLocator.Instance.GetService<GameManager>()._currentstudentName;
        _teamColor = (TEAMCOLOR)(int)Client._tablet._id;
        _teamColorText.text = "EQUIPO " + _teamColor;
        //_gameName.text = ServiceLocator.Instance.GetService<GameManager>()._currentgameName;
    }
    public void ShowCommonScenario()
    {
        OpenNextWindow();
        //_panelInfo.SetActive(false);
        _doorsOpen.Play("PuertasTransicionAbrir"); //No funciona bien
        StartCoroutine(ShowGameSelected());
    }
    IEnumerator ShowGameSelected()
    {
        //yield return new WaitUntil(() => ServiceLocator.Instance.GetService<NetworkManager>()._minigameLevel != -1); // esperar a que funcione bien la bbdd
        yield return new WaitForSeconds(3.0f);
        _blackTransition.SetActive(true);
        //No funciona bien porque a veces aunque toque un juego enfoca a otro. La transicion va muy rapido y no te enteras de que enfoca
        switch (ServiceLocator.Instance.GetService<GameManager>()._currentstudentName)
        {
            case "CabinGeometry":
                _blackTransition.GetComponent<Animator>().Play("BlackScreen_Cabin");
                break;
            case "TelescopeGeometry":
                _blackTransition.GetComponent<Animator>().Play("BlackScreen_Telescope");
                break;
            case "PanelButtonAssociation":
                _blackTransition.GetComponent<Animator>().Play("BlackScreen_Button");
                break;

        }
        yield return null;
    }

}

