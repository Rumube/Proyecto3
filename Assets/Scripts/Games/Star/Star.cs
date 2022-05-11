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
    [Header("References")]
    private AudioSource _audio;
    public AudioClip _clipStarSelected;
    public GameObject _light;

    // Start is called before the first frame update
    void Start()
    {
        _audio = GetComponent<AudioSource>();
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
                    _light.SetActive(true);
                    _isConnected = true;
                    _gm.GetComponent<ConstelationGenerator>().CheckIfIsCorrect(gameObject);
                    _audio.clip = _clipStarSelected;
                    _audio.Play();
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
        _touched = true;
    }

    public void InitStart(GameObject gm)
    {
        _gm = gm;
        _timeToBePressed = 0.5f;
    }

    public void SetIsConnected(bool value)
    {
        _isConnected = value;
        _light.SetActive(value);
    }

    public bool GetIsConnected()
    {
        return _isConnected;
    }
}
