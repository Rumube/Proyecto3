using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstallerGames : MonoBehaviour
{
    [Tooltip("Tick if you are testing your games in your scene")]
    public bool _notGameLoop;
    public NetMSinBucle _netManagerWithOutLoop;
    public GMSinBucle _gameManagerWithOutLoop;

    [Header("UI")]
    public Canvas _canvasGUI;


    [Header("Utilities")]
    public GameTimeConfiguration _gameTimeConfiguration;
    public AndroidInputAdapter _inputAndroid;
    public Error _error;
    public FrogMessage _frogMessage;
    public Positive _positive;
    public CalculatePuntuation _calculatePuntiation;
    public RankingClient _rankingClient;

    // Start is called before the first frame update
    void Awake()
    {
        if (ServiceLocator.Instance.Contains<IGameTimeConfiguration>())
        {
            ServiceLocator.Instance.UnregisterService<IGameTimeConfiguration>();
        }
        if (ServiceLocator.Instance.Contains<IInput>())
        {
            ServiceLocator.Instance.UnregisterService<IInput>();
        }
        if (ServiceLocator.Instance.Contains<IError>())
        {
            ServiceLocator.Instance.UnregisterService<IError>();
        }
        if (ServiceLocator.Instance.Contains<IFrogMessage>())
        {
            ServiceLocator.Instance.UnregisterService<IFrogMessage>();
        }
        if (ServiceLocator.Instance.Contains<Canvas>())
        {
            ServiceLocator.Instance.UnregisterService<Canvas>();
        }
        if (ServiceLocator.Instance.Contains<IPositive>())
        {
            ServiceLocator.Instance.UnregisterService<IPositive>();
        }
        if (ServiceLocator.Instance.Contains<ICalculatePoints>())
        {
            ServiceLocator.Instance.UnregisterService<ICalculatePoints>();
        }
        if (ServiceLocator.Instance.Contains<RankingClient>())
        {
            ServiceLocator.Instance.UnregisterService<RankingClient>();
        }

        ServiceLocator.Instance.RegisterService<IGameTimeConfiguration>(_gameTimeConfiguration);
        ServiceLocator.Instance.RegisterService<IInput>(_inputAndroid);
        ServiceLocator.Instance.RegisterService<IError>(_error);
        ServiceLocator.Instance.RegisterService(_canvasGUI);
        ServiceLocator.Instance.RegisterService<IFrogMessage>(_frogMessage);
        ServiceLocator.Instance.RegisterService<IPositive>(_positive);
        ServiceLocator.Instance.RegisterService<ICalculatePoints>(_calculatePuntiation);
        ServiceLocator.Instance.RegisterService(_rankingClient);

        if (_notGameLoop)
        {
            ServiceLocator.Instance.RegisterService<IGameManager>(_gameManagerWithOutLoop);
            ServiceLocator.Instance.RegisterService<INetworkManager>(_netManagerWithOutLoop);
            ServiceLocator.Instance.RegisterService(_rankingClient);
        }
    }
}
