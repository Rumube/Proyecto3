using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelescopeAddAndSubstractStars : MonoBehaviour
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
   

    // Start is called before the first frame update
    void Start()
    {
        _audio = GetComponent<AudioSource>();
        _anim.gameObject.SetActive(false);      
    }

    // Update is called once per frame
    void Update()
    {/*
       _needsHold = false;

        if (_gm.GetComponent<TelescopeAddAndSubstractConstelationGenerator>().GetStarsSelecteds() > 0)
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

                    List<GameObject> playerStarList = _gm.GetComponent<TelescopeAddAndSubstractConstelationGenerator>().GetplayerStarList();

                    if (playerStarList.Count != 0)
                    {
                        print("Entra otros");
                        int lastPos = playerStarList.Count - 1;
                        GameObject lastStar = playerStarList[lastPos];
                    }


                    print("Siempre");
                    _gm.GetComponent<TelescopeAddAndSubstractConstelationGenerator>().AddStars(gameObject);
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
                //_isConnected = true;
                _gm.GetComponent<TelescopeAddAndSubstractConstelationGenerator>().AddStars(gameObject);
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
        _touched = false;*/
    }
    /*

    public void CollisionDetected()
    {
        _touched = true;
    }*/

    public void InitStart(GameObject gm, int order)
    {
        _gm = gm;
        _timeToBePressed = 0.2f;
        _order = order;
    }

    public void SetIsConnected(bool value)
    {
        _isConnected = value;
        _light.SetActive(value);
        _anim.Play("Static");
        _anim.gameObject.SetActive(value);
    }

    public bool GetIsConnected()
    {
        return _isConnected;
    }

    public int GetOrder()
    {
        return _order;
    }

}
