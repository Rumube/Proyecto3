using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Installer : MonoBehaviour
{
    [Tooltip("Set if the build is server or client")]
    public bool _server;

    [Header("UI")]
    public MobileUI _canvasMobile;
    public TabletUI _canvasTablet;

    [Header("Utilities")]
    public GameTimeConfiguration _gameTimeConfiguration;
    public GameManager _gameManager;

    private IInput _inputUsed;
    private IDatabase _databaseUsed;
    private IUI _UIUsed;
    // Start is called before the first frame update
    void Awake()
    {
        //Register services to use globally
        ServiceLocator.Instance.RegisterService(this);
        ServiceLocator.Instance.RegisterService<IGameTimeConfiguration>(_gameTimeConfiguration);
        ServiceLocator.Instance.RegisterService(_gameManager);
        if (_server)
        {
            ServiceLocator.Instance.RegisterService<IUI>(_canvasMobile);
            ServiceLocator.Instance.RegisterService(_canvasMobile);
        }
        else
        {
            ServiceLocator.Instance.RegisterService<IUI>(_canvasTablet);
        }

        SetDatabase();
        SetInput();
        SetUI();
    }

    // Update is called once per frame
    void Update()
    {
        _inputUsed.Drag();

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

    /// <summary>Set the database method deppending on the platform</summary>
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
        if (_server)
        {
            _canvasMobile.gameObject.SetActive(true);
            _canvasTablet.gameObject.SetActive(false);
        }
        else
        {
            _canvasMobile.gameObject.SetActive(false);
            _canvasTablet.gameObject.SetActive(true);
        }
    }
}
