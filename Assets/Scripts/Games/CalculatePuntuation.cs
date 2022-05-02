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

    }
    /// <summary>Calculates the puntuation</summary>
    /// <param name="success">Number of success</param>
    /// <param name="fails">Number of fails</param>
    public void Puntuation(int success, int fails)
    {

        //ServiceLocator.Instance.GetService<GameManager>()._gameStateClient = GameManager.GAME_STATE_CLIENT.ranking;
        float finishTime = ServiceLocator.Instance.GetService<IGameTimeConfiguration>().GetFinishTime();
        float currentTime = ServiceLocator.Instance.GetService<IGameTimeConfiguration>().GetCurrentTime();
        if (_attempt>0)
        {
            EDebug.Log("operacion: time anterior "+ _timeList[_attempt - 1] +"Finish"+finishTime +"- Current"+ currentTime);
            _timeAttemptList.Add( _timeList[_attempt - 1] - (finishTime - currentTime));
            _timeList.Add(finishTime - currentTime);
        }
        else
        {
            _timeAttemptList.Add(currentTime);
            _timeList.Add(finishTime - currentTime);
        }
        
        _total = success + fails;
        _success += success;
        _fails += fails;
        EDebug.Log("tiempo" + (finishTime - currentTime) +"porcentaje "+ Mathf.Round(50 * ((finishTime - currentTime) / finishTime)));
        
        if (success >0)
        {
            _points = Mathf.Round((50 * (success / _total)) + (50 * (_timeList[_attempt] / finishTime))+ _points);
            EDebug.Log("puntos por Intento " + Mathf.Round((50 * (success / _total)) + (50 * (_timeList[_attempt] / finishTime))));
           

        }
        _attempt++;

        CalculateAverage();
        //EDebug.Log("puntos por Intento "+Mathf.Round((50 * (success / _total)) + (50 * (_timeList[_attempt] / finishTime))));
        EDebug.Log("Intento "+_attempt+"puntos "+_points);
        ServiceLocator.Instance.GetService<IGameManager>().SetClientState(IGameManager.GAME_STATE_CLIENT.playing);
        
    }
    /// <summary>Calculates the average of the points , time, success fails</summary>
    public void CalculateAverage()
    {
        float finishTime = ServiceLocator.Instance.GetService<IGameTimeConfiguration>().GetFinishTime();
        _average.averagePoints = _points / _attempt;
        _average.averageTime = finishTime / _attempt;
        _average.averageSuccess = _success / (_success + _fails);
        _average.averageFails = _fails / (_success + _fails);
    }

    public Average GetAverage()
    {
        return _average;
    }
}
