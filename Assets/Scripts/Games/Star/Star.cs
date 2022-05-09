using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : Geometry
{
    [Header("Configuration")]
    public bool _needsHold;
    private bool _isConnected = false;
    public float _timePressed = 0;
    public float _timeToBePressed;
    private GameObject _gm;
    [Header("States")]
    private bool _touched;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_needsHold)
        {
            if (_touched)
            {
                _timePressed += Time.deltaTime;
                if(_timePressed >= _timeToBePressed && !_isConnected)
                {
                    _isConnected = true;
                    _gm.GetComponent<ConstelationGenerator>().AddNewPosition(transform.position);
                }
            }
            else
            {
                _timePressed = 0;
            }
        }
        else
        {
            //TODO: NO NEED HOLD
        }
        _touched = false;
    }


    public void CollisionDetected()
    {
        print("Tocado");
        _touched = true;
    }

    public void InitStart(GameObject gm)
    {
        _gm = gm;
    }

    public void SetIsConnected(bool value)
    {
        _isConnected = value;
    }

    public bool GetIsConnected()
    {
        return _isConnected;
    }
}
