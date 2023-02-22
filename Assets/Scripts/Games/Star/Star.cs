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
    public Animator _anim;

    // Start is called before the first frame update
    void Start()
    {
        _audio = GetComponent<AudioSource>();
        _anim.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        _needsHold = false;
        if (_gm.GetComponent<ConstelationGenerator>().GetStarsSelecteds() > 0)
            _needsHold = true;

        if (_needsHold)
        {
            if (_touched)
            {
                _timePressed += Time.deltaTime;
                if (_timePressed >= _timeToBePressed && !_isConnected)
                {
                    _light.SetActive(true);
                    _isConnected = true;
                    _gm.GetComponent<ConstelationGenerator>().CheckIfIsCorrect(gameObject);
                    _audio.clip = _clipStarSelected;
                    _audio.Play();
                    _anim.gameObject.SetActive(true);
                    _anim.Play("Star_Slected_Rotation");
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
            if (_touched)
            {

                _light.SetActive(true);
                _isConnected = true;
                _gm.GetComponent<ConstelationGenerator>().CheckIfIsCorrect(gameObject);
                _audio.clip = _clipStarSelected;
                _audio.Play();
                _anim.gameObject.SetActive(true);
                _anim.Play("Star_Slected_Rotation");
            }
            else
            {
                _timePressed = 0;
            }
        }
        _touched = false;
    }

    /// <summary>
    /// Detects if exists collision
    /// </summary>
    public void CollisionDetected()
    {
        _touched = true;
    }
    /// <summary>
    /// Init values of the star
    /// </summary>
    /// <param name="gm">GameManager</param>
    public void InitStart(GameObject gm)
    {
        _gm = gm;
        _timeToBePressed = 0.5f;
    }
    /// <summary>
    /// Set the connection
    /// </summary>
    /// <param name="value">new value</param>
    public void SetIsConnected(bool value)
    {
        _isConnected = value;
        _light.SetActive(value);
        _anim.Play("Static");
        _anim.gameObject.SetActive(value);
    }
    /// <summary>
    /// Returns if is connected
    /// </summary>
    /// <returns><see cref="_isConnected"/></returns>
    public bool GetIsConnected()
    {
        return _isConnected;
    }
}
