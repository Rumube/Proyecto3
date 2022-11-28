using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crosstales.RTVoice;

public class Installer : MonoBehaviour
{
    [Tooltip("Set if the build is server or client")]
    public bool _server;

    [Header("UI")]
    public MobileUI _canvasMobile;
    public TabletUI _canvasTablet;

    public UIManager _uiManager;

    public Canvas _canvasGUI;


    [Header("Utilities")]
    public GameTimeConfiguration _gameTimeConfiguration;
    public GameManager _gameManager;
    public NetworkManager _networkManager;
    public AndroidInputAdapter _inputAndroid;
    public RankingServer _rankingServer;

    public ServerUtility _myServer;

    public Error _error;
    public FrogMessage _frogMessage;


    private IInput _inputUsed;
    private IDatabase _databaseUsed;
    private IUI _UIUsed;
    private IError _IError;
    private IFrogMessage _IFrogMessage;

    // Start is called before the first frame update
    void Awake()
    {
        //Register services to use globally
        //ServiceLocator.Instance.RegisterService(this);
        //ServiceLocator.Instance.RegisterService<IGameTimeConfiguration>(_gameTimeConfiguration);
         if (!ServiceLocator.Instance.Contains<IGameManager>())
        {
            ServiceLocator.Instance.RegisterService<IGameManager>(_gameManager);
        }
        //else if(ServiceLocator.Instance.GetService<GameManager>() == null)
        //{
        //    _gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        //}

        if (!ServiceLocator.Instance.Contains<INetworkManager>())
        {
            ServiceLocator.Instance.RegisterService<INetworkManager>(_networkManager);
        }

        if (ServiceLocator.Instance.Contains<UIManager>())
        {
            ServiceLocator.Instance.UnregisterService<UIManager>();
        }
        ServiceLocator.Instance.RegisterService(_uiManager);

        if (!ServiceLocator.Instance.Contains<ServerUtility>())
        {
            ServiceLocator.Instance.RegisterService(_myServer);
        }



        if (!ServiceLocator.Instance.Contains<IFrogMessage>())
        {
            ServiceLocator.Instance.RegisterService<IFrogMessage>(_frogMessage);
        }
        
        if (!ServiceLocator.Instance.Contains<RankingServer>())
        {
            ServiceLocator.Instance.RegisterService(_rankingServer);
        }

        if (!ServiceLocator.Instance.Contains<IUI>() &&_server && _canvasMobile != null)
        {
            ServiceLocator.Instance.RegisterService<IUI>(_canvasMobile);
            ServiceLocator.Instance.RegisterService(_canvasMobile);
        }
        else if(!ServiceLocator.Instance.Contains<IUI>() && !_server && _canvasTablet != null)
        {
            ServiceLocator.Instance.RegisterService<IUI>(_canvasTablet);
            ServiceLocator.Instance.RegisterService(_canvasTablet);
        }

        SetDatabase();
        //SetInput();
        SetUI();
        SetTextToSpeechConf();
        //Screen never turn off
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    void SetTextToSpeechConf()
    {
        //Speaker.Instance.VoiceForCulture("es-es-x-eea-local");
    }

    // Update is called once per frame
    void Update()
    {
        //_inputUsed.Drag();

    }
    /// <summary>Set the input method deppending on the platform</summary>
    private void SetInput()
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        _inputUsed = new DesktopInputAdapter();
#elif UNITY_ANDROID || UNITY_STANDALONE_WIN
        _inputUsed = new AndroidInputAdapter();
#endif
    }

    // <summary>Set the database method deppending on the platform</summary>
    private void SetDatabase()
    {
#if UNITY_EDITOR
        _databaseUsed = new DesktopDatabaseAdapter();
#elif UNITY_ANDROID
        _databaseUsed = new AndroidDatabaseAdapter();
#endif
    }

    /// <summary>Set the UI deppending if its server or not</summary>
    private void SetUI()
    {
        if (_server && _canvasMobile != null)
        {
            if (_canvasTablet != null)
            {
                _canvasTablet.gameObject.SetActive(false);
            }
            _canvasMobile.gameObject.SetActive(true);
        }
        else if (!_server && _canvasTablet != null)
        {
            if (_canvasMobile != null)
            {
                _canvasMobile.gameObject.SetActive(false);
            }
            _canvasTablet.gameObject.SetActive(true);
        }
    }
}
