using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculatePuntuation : MonoBehaviour
{
    public int _success;
    public int _total;
    public int _points;
    public int _attempt;

    void Start()
    {
        
    }
    public void Puntuation(int success, int fails)
    {
        ServiceLocator.Instance.GetService<GameManager>()._gameStateClient = GameManager.GAME_STATE_CLIENT.ranking;
        _total = success + fails;
        _points = (success / _total * 100)+_points;
        if (_success == _total)
        {
            EDebug.Log("Bien hecho");
        }
        _attempt++;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
