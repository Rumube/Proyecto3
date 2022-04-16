using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTimeConfiguration : MonoBehaviour, IGameTimeConfiguration
{
    public Image _timeImage;
    public float _currentTime;
    [SerializeField]
    public float _maxTime;
    public float _finishTime;
    private float _startTime;
    public bool _canStartTime = false;

    private void Update()
    {
        if(_canStartTime && ServiceLocator.Instance.GetService<GameManager>()._gameStateClient == GameManager.GAME_STATE_CLIENT.playing)
            TimeProgress();
    }

    /// <summary>
    /// Playing time begins.
    /// </summary>
    public void StartGameTime()
    {
        _timeImage = GameObject.FindGameObjectWithTag("CountDown").GetComponent<Image>();
        _maxTime = (ServiceLocator.Instance.GetService<NetworkManager>()._minigameMinutes * 60) + ServiceLocator.Instance.GetService<NetworkManager>()._minigameSeconds;
        _finishTime = Time.realtimeSinceStartup + _maxTime;
        _currentTime = Time.realtimeSinceStartup;
        _startTime = Time.realtimeSinceStartup;
    }

    /// <summary>
    /// Controll the game time and change the UI
    /// </summary>
    void TimeProgress()
    {
        _currentTime += Time.deltaTime;
        _timeImage.fillAmount -= 1.0f/_maxTime * Time.deltaTime;
        if (_currentTime >= _finishTime)
        {
            ServiceLocator.Instance.GetService<GameManager>()._gameStateClient = GameManager.GAME_STATE_CLIENT.ranking;
            _canStartTime = false;
        }         
    }
    public void SetStartTime(bool state)
    {
        _canStartTime = state;
    }
}
