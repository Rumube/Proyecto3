using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculatePuntuation : MonoBehaviour, ICalculatePoints
{
    public float _points;
    public float _success;
    public float _fails;
    public int _attempt=0;
    public List<float> _timeList = new List<float>();
    //Lista con el tiempo que tarda por cada intento
    public List<float> _timeAttemptList = new List<float>();
    private int _total;
    

    GameTimeConfiguration _gameTimeConfiguration;
    [Header("Average")]
   // public List<Average> _averages;
    public Average _average;
    [Serializable]
    public struct Average
    {
        public float averagePoints;
        public float averageTime;
        public float averageSuccess;
        public float averageFails;
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
        
        ServiceLocator.Instance.GetService<GMSinBucle>()._gameStateClient = GMSinBucle.GAME_STATE_CLIENT.ranking;
       
        if (_attempt>0)
        {
            EDebug.Log("operacion: time anterior "+ _timeList[_attempt - 1] +"Finish"+_gameTimeConfiguration._finishTime +"- Current"+ _gameTimeConfiguration._currentTime);
            _timeAttemptList.Add( _timeList[_attempt - 1] - (_gameTimeConfiguration._finishTime - _gameTimeConfiguration._currentTime));
            _timeList.Add(_gameTimeConfiguration._finishTime - _gameTimeConfiguration._currentTime);
        }
        else
        {
            _timeAttemptList.Add(_gameTimeConfiguration._currentTime);
            _timeList.Add(_gameTimeConfiguration._finishTime - _gameTimeConfiguration._currentTime);
        }
        
        _total = success + fails;
        _success += success;
        _fails += fails;
        EDebug.Log("tiempo" + (_gameTimeConfiguration._finishTime - _gameTimeConfiguration._currentTime) +"porcentaje "+ Mathf.Round(50 * ((_gameTimeConfiguration._finishTime - _gameTimeConfiguration._currentTime) / _gameTimeConfiguration._finishTime)));
        
        if (success >0)
        {
            _points = Mathf.Round((50 * (success / _total)) + (50 * (_timeList[_attempt] / _gameTimeConfiguration._finishTime))+ _points);
            EDebug.Log("puntos por Intento " + Mathf.Round((50 * (success / _total)) + (50 * (_timeList[_attempt] / _gameTimeConfiguration._finishTime))));
           

        }
        _attempt++;

        CalculateAverage();
        //EDebug.Log("puntos por Intento "+Mathf.Round((50 * (success / _total)) + (50 * (_timeList[_attempt] / _gameTimeConfiguration._finishTime))));
        EDebug.Log("Intento "+_attempt+"puntos "+_points);
        ServiceLocator.Instance.GetService<GMSinBucle>()._gameStateClient = GMSinBucle.GAME_STATE_CLIENT.playing;
        
    }
    /// <summary>Calculates the average of the points , time, success fails</summary>
    public void CalculateAverage()
    {
        _average.averagePoints = _points / _attempt;
        _average.averageTime = _gameTimeConfiguration._finishTime / _attempt;
        _average.averageSuccess = _success / (_success + _fails);
        _average.averageFails = _fails / (_success + _fails);
    }

    public Average GetAverage()
    {
        return _average;
    }
}
