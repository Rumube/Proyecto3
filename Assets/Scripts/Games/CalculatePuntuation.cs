using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculatePuntuation : MonoBehaviour
{
 
    
    public float _points;
    public int _attempt=0;
    public List<float> _timeList = new List<float>();
    //Lista con el tiempo que tarda por cada intento
    public List<float> _timeAttemptList = new List<float>();
    private int _total;
    GameTimeConfiguration _gameTimeConfiguration;

    void Start()
    {
        _gameTimeConfiguration = GetComponent<GameTimeConfiguration>();
    }
    /// <summary>Calculates the puntuation</summary>
    /// <param name="success">Number of success</param>
    /// <param name="fails">Number of fails</param>
    public void Puntuation(int success, int fails)
    {
        ServiceLocator.Instance.GetService<GameManager>()._gameStateClient = GameManager.GAME_STATE_CLIENT.ranking;
       
        if (_attempt>0)
        {
            EDebug.Log("operacion: time anterior "+ _timeList[_attempt - 1] +"Finisg"+_gameTimeConfiguration._finishTime +"- Current"+ _gameTimeConfiguration._currentTime);
            _timeAttemptList.Add( _timeList[_attempt - 1] - (_gameTimeConfiguration._finishTime - _gameTimeConfiguration._currentTime));
            _timeList.Add(_gameTimeConfiguration._finishTime - _gameTimeConfiguration._currentTime);
        }
        else
        {
            _timeList.Add(_gameTimeConfiguration._finishTime - _gameTimeConfiguration._currentTime);
        }
        
        _total = success + fails;

        EDebug.Log("tiempo" + (_gameTimeConfiguration._finishTime - _gameTimeConfiguration._currentTime) +"porcentaje "+ Mathf.Round(50 * ((_gameTimeConfiguration._finishTime - _gameTimeConfiguration._currentTime) / _gameTimeConfiguration._finishTime)));
        
        if (success >0)
        {
            _points = Mathf.Round((50 * (success / _total)) + (50 * (_timeList[_attempt] / _gameTimeConfiguration._finishTime))+ _points);
            _attempt++;

        }
        else
        {
            _points = 0;
        }
       
        
        EDebug.Log("puntos por Intento "+Mathf.Round((50 * (success / _total)) + (50 * ((_gameTimeConfiguration._finishTime - _gameTimeConfiguration._currentTime) / _gameTimeConfiguration._finishTime))));
        EDebug.Log("Intento "+_attempt+"puntos "+_points);
        ServiceLocator.Instance.GetService<GameManager>()._gameStateClient = GameManager.GAME_STATE_CLIENT.playing;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
