using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Installer : MonoBehaviour
{
    public bool _server;

    public Test _test;
    public GameTimeConfiguration _gameTimeConfiguration;

    private IInput _inputUsed;
    private IDatabase _databaseUsed;

    // Start is called before the first frame update
    void Awake()
    {
        //Register services to use globally
        //ServiceLocator.Instance.RegisterService<ITest>(new Test()); //If doesnt have monobehaveour hederitance
        ServiceLocator.Instance.RegisterService<ITest>(_test);
        ServiceLocator.Instance.RegisterService(this);
        ServiceLocator.Instance.RegisterService<IGameTimeConfiguration>(_gameTimeConfiguration);

        SetDatabase();
        SetInput();
    }

    // Update is called once per frame
    void Update()
    {
        _inputUsed.Drag();

        //Just for showing how to use serviceLocator
        Debug.Log(ServiceLocator.Instance.GetService<ITest>().Hello());
    }

    private void SetInput()
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN	
        _inputUsed = new DesktopInputAdapter();
#elif UNITY_ANDROID || UNITY_STANDALONE_WIN	
        _inputUsed = new AndroidInputAdapter();
#endif
    }

    private void SetDatabase()
    {
#if UNITY_EDITOR
        _databaseUsed = new DesktopDatabaseAdapter();
#elif UNITY_ANDROID
        _databaseUsed = new AndroidDatabaseAdapter();
#endif
    }
}
