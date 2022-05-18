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
    public NetworkManager _networkSinBucle;
    public GameManager _GameManager;
    public Positive _positive;
    public CalculatePuntuation _calculatePoints;

    //private ICalculatePoints _ICalculatePoints;
    //private IInput _inputUsed;
    //private IError _IError;
    //private IFrogMessage _IFrogMessage;
    //private IPositive _IPositive;
    // Start is called before the first frame update
    void Awake()
    {     
        ServiceLocator.Instance.RegisterService<IGameTimeConfiguration>(_gameTimeConfiguration);
        ServiceLocator.Instance.RegisterService<IInput>(_inputAndroid);
        ServiceLocator.Instance.RegisterService<IError>(_error);
        ServiceLocator.Instance.RegisterService(_canvasGUI);
        ServiceLocator.Instance.RegisterService<IFrogMessage>(_frogMessage);
        ServiceLocator.Instance.RegisterService<INetworkManager>(_networkSinBucle);
        ServiceLocator.Instance.RegisterService<GameManager>(_GameManager);
        ServiceLocator.Instance.RegisterService<IPositive>(_positive);
        ServiceLocator.Instance.RegisterService<ICalculatePoints>(_calculatePoints);
    }
}
