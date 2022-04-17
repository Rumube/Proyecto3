using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstallerGames : MonoBehaviour
{
    [Header("UI")]
    public Canvas _canvasGUI;


    [Header("Utilities")]
    public GameTimeConfiguration _gameTimeConfiguration;
    public AndroidInputAdapter _inputAndroid;
    public Error _error;
    public FrogMessage _frogMessage;
    public Positive _positive;


    private IInput _inputUsed;
    private IError _IError;
    private IFrogMessage _IFrogMessage;
    private IPositive _IPositive;

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

        ServiceLocator.Instance.RegisterService<IGameTimeConfiguration>(_gameTimeConfiguration);
        ServiceLocator.Instance.RegisterService<IInput>(_inputAndroid);
        ServiceLocator.Instance.RegisterService<IError>(_error);
        ServiceLocator.Instance.RegisterService(_canvasGUI);
        ServiceLocator.Instance.RegisterService<IFrogMessage>(_frogMessage);
        ServiceLocator.Instance.RegisterService<IPositive>(_positive);
    }
}
