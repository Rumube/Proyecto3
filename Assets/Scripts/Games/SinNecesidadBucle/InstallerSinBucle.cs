using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstallerSinBucle : MonoBehaviour
{
    [Header("UI")]
    public Canvas _canvasGUI;


    [Header("Utilities")]
    public GameTimeConfiguration _gameTimeConfiguration;
    public AndroidInputAdapter _inputAndroid;
    public Error _error;
    public FrogMessage _frogMessage;
    public NetMSinBucle _networkSinBucle;
    public GMSinBucle _gmSinBucle;

    private IInput _inputUsed;
    private IError _IError;
    private IFrogMessage _IFrogMessage;

    // Start is called before the first frame update
    void Awake()
    {     
        ServiceLocator.Instance.RegisterService<IGameTimeConfiguration>(_gameTimeConfiguration);
        ServiceLocator.Instance.RegisterService<IInput>(_inputAndroid);
        ServiceLocator.Instance.RegisterService<IError>(_error);
        ServiceLocator.Instance.RegisterService(_canvasGUI);
        ServiceLocator.Instance.RegisterService<IFrogMessage>(_frogMessage);
        ServiceLocator.Instance.RegisterService<NetMSinBucle>(_networkSinBucle);
        ServiceLocator.Instance.RegisterService<GMSinBucle>(_gmSinBucle);
    }
}
