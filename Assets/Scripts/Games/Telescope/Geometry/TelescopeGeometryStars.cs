using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelescopeGeometryStars : MonoBehaviour
{
    [Header("Configuration")]
    public bool _needsHold;
    private bool _isConnected = false;
    public float _timePressed = 0;
    public float _timeToBePressed;
    private GameObject _gm;
    [Header("States")]
    private bool _touched;
    [SerializeField]
    private int _order = 0;
    [Header("References")]
    private AudioSource _audio;
    public AudioClip _clipStarSelected;
    public GameObject _light;
    public Animator _anim;
    public GameObject _nextStar;
    public GameObject _prevStar;
    public GameObject _starConnected;
    private bool _correctConection = false;

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

        if (_gm.GetComponent<TelescopeGeometryConstelationGenerator>().GetStarsSelecteds() > 0)
        {
            _needsHold = true;
        }

        if (_needsHold)
        {
            if (_touched)
            {
                _timePressed += Time.deltaTime;
                if (_timePressed >= _timeToBePressed && !_isConnected)
                {
                    _light.SetActive(true);
                    _isConnected = true;

                    List<GameObject> playerStarList = _gm.GetComponent<TelescopeGeometryConstelationGenerator>().GetplayerStarList();
                    
                    if (playerStarList.Count != 0)
                    {
                        int lastPos = playerStarList.Count - 1;
                        GameObject lastStar = playerStarList[lastPos];
                        _starConnected = lastStar;
                        if (lastStar == _prevStar || lastStar == _nextStar)
                        {
                            _correctConection = true;
                        }
                        else
                        {
                            _correctConection = false;
                        }
                    }
                    _gm.GetComponent<TelescopeGeometryConstelationGenerator>().AddStars(gameObject);
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
            if (_touched)
            {

                _light.SetActive(true);
                _gm.GetComponent<TelescopeGeometryConstelationGenerator>().AddStars(gameObject);
                _audio.clip = _clipStarSelected;
                _audio.Play();
                _anim.gameObject.SetActive(true);
                _anim.Play("Star_Slected_Rotation");
                _correctConection = true;
            }
            else
            {
                _timePressed = 0;
            }
        }
        _touched = false;
    }
    /// <summary>
    /// Set <see cref="_touched"/> to true
    /// </summary>
    public void CollisionDetected()
    {
        _touched = true;
    }
    /// <summary>
    /// Init the values
    /// </summary>
    /// <param name="gm">GameManager</param>
    /// <param name="order">Position in serie</param>
    public void InitStart(GameObject gm, int order)
    {
        _gm = gm;
        _timeToBePressed = 0.5f;
        _order = order;
    }
    /// <summary>
    /// Set or destroy the connection
    /// </summary>
    /// <param name="value">True connected / False disconnected</param>
    public void SetIsConnected(bool value)
    {
        _isConnected = value;
        _light.SetActive(value);
        _anim.Play("Static");
        _anim.gameObject.SetActive(value);
    }
    /// <summary>
    /// Return <see cref="_isConnected"/>
    /// </summary>
    /// <returns><see cref="_isConnected"/></returns>
    public bool GetIsConnected()
    {
        return _isConnected;
    }
    /// <summary>
    /// Return <see cref="_order"/>
    /// </summary>
    /// <returns><see cref="_order"/></returns>
    public int GetOrder()
    {
        return _order;
    }
    /// <summary>
    /// Return if is <see cref="_correctConection"/>
    /// </summary>
    /// <returns><see cref="_correctConection"/></returns>
    public bool GetCorrectConnection()
    {
        return _correctConection;
    }
    /// <summary>
    /// Set <see cref="_correctConection"/> to false
    /// </summary>
    /// <param name="value">New value (Not used)</param>
    public void SetCorrectConnection(bool value)
    {
        _correctConection = false;
    }
}
