using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTimeConfiguration : MonoBehaviour, IGameTimeConfiguration
{
    Image _timeImage;
    public float _currentTime;
    private float _maxTime;
    [HideInInspector]
    public float _finishTime;
    private float _startTime;
    private bool _canStartTime = false;
    private bool _lastSeconds = false;
    private bool _halfTime = false;
    private Animator _anim;
    private AudioSource _timerSound;
    public AudioClip _tickTack;

    private void Update()
    {
        if(_canStartTime && ServiceLocator.Instance.GetService<IGameManager>().GetClientState() == IGameManager.GAME_STATE_CLIENT.playing)
        {
            TimeProgress();
        }
    }

    /// <summary>
    /// Playing time begins.
    /// </summary>
    public void StartGameTime()
    {
        _canStartTime = true;
        _timeImage = GameObject.FindGameObjectWithTag("CountDown").GetComponent<Image>();
        _anim = GameObject.FindGameObjectWithTag("Timer").GetComponent<Animator>();
        _maxTime = (ServiceLocator.Instance.GetService<INetworkManager>().GetMinigameMinutes() * 60) + ServiceLocator.Instance.GetService<INetworkManager>().GetMinigameSeconds();
        _finishTime = Time.realtimeSinceStartup + _maxTime;
        _currentTime = Time.realtimeSinceStartup;
        _startTime = Time.realtimeSinceStartup;
        _lastSeconds = false;
        _halfTime = false;
        _anim.Play("Timer");
        _timerSound = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Controll the game time and change the UI
    /// </summary>
    void TimeProgress()
    {
        _currentTime += Time.deltaTime;
        _timeImage.fillAmount -= 1.0f/_maxTime * Time.deltaTime;

        
        //_startTime = 10;
        //_finishTime = 30;

        if (_currentTime >= _startTime + _finishTime / 2 && !_halfTime)
        {
            _halfTime = true;
            _anim.Play("Temporizador_Half_Time");
            
        }

        if (_currentTime >= _finishTime - 5 && !_lastSeconds)
        {
            //Activamos anim;
            _lastSeconds = true;
            _anim.Play("Temporizador_Poco_Tiempo");
            _timerSound.PlayOneShot(_tickTack, 5.0f);

        }

        if (_currentTime >= _finishTime)
        {
            ServiceLocator.Instance.GetService<IFrogMessage>().StopFrogSpeaker();
            ServiceLocator.Instance.GetService<IGameManager>().SetClientState(IGameManager.GAME_STATE_CLIENT.ranking);
            _canStartTime = false;
            ServiceLocator.Instance.GetService<INetworkManager>().SendMatchData();
            _timerSound.Stop();
        }         
    }
    public void SetStartTime(bool state)
    {
        _canStartTime = state;
    }

    public float GetCurrentTime()
    {
        return _currentTime;
    }

    public float GetFinishTime()
    {
        return _finishTime;
    }
}
