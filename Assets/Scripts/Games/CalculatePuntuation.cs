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
    private int _success;
    private int _fails;

    GameTimeConfiguration _gameTimeConfiguration;

    public struct Average
    {
        public int averagePoints;
        public float averageTime;
        public int averageSuccess;
        public int averageFails;
    }
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
            EDebug.Log("operacion: time anterior "+ _timeList[_attempt - 1] +"Finish"+_gameTimeConfiguration._maxTime +"- Current"+ _gameTimeConfiguration._currentTime);
            _timeAttemptList.Add( _timeList[_attempt - 1] - (_gameTimeConfiguration._maxTime - _gameTimeConfiguration._currentTime));
            _timeList.Add(_gameTimeConfiguration._maxTime - _gameTimeConfiguration._currentTime);
        }
        else
        {
            _timeAttemptList.Add(_gameTimeConfiguration._currentTime);
            _timeList.Add(_gameTimeConfiguration._maxTime - _gameTimeConfiguration._currentTime);
        }
        
        _total = success + fails;
        _success += success;
        _fails += fails;
        EDebug.Log("tiempo" + (_gameTimeConfiguration._maxTime - _gameTimeConfiguration._currentTime) +"porcentaje "+ Mathf.Round(50 * ((_gameTimeConfiguration._maxTime - _gameTimeConfiguration._currentTime) / _gameTimeConfiguration._maxTime)));
        
        if (success >0)
        {
            _points = Mathf.Round((50 * (success / _total)) + (50 * (_timeList[_attempt] / _gameTimeConfiguration._maxTime))+ _points);
            EDebug.Log("puntos por Intento " + Mathf.Round((50 * (success / _total)) + (50 * (_timeList[_attempt] / _gameTimeConfiguration._maxTime))));
           

        }
        _attempt++;


        //EDebug.Log("puntos por Intento "+Mathf.Round((50 * (success / _total)) + (50 * (_timeList[_attempt] / _gameTimeConfiguration._finishTime))));
        EDebug.Log("Intento "+_attempt+"puntos "+_points);
        ServiceLocator.Instance.GetService<GameManager>()._gameStateClient = GameManager.GAME_STATE_CLIENT.playing;
        
    }
  

    public void CalculateAverage()
    {

    }
    
}
