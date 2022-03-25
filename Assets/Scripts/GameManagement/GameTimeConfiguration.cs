using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTimeConfiguration : MonoBehaviour, IGameTimeConfiguration
{
    public Image _timeImage;
    public float _currentTime;
    [SerializeField]
    private float _maxTime;
    public float _finishTime;
    private float _startTime;


    private void Update()
    {
        if(ServiceLocator.Instance.GetService<GameManager>()._gameStateClient == GameManager.GAME_STATE_CLIENT.playing)
            TimeProgress();
    }

    /// <summary>
    /// Playing time begins.
    /// </summary>
    public void StartGameTime()
    {
        ServiceLocator.Instance.GetService<GameManager>()._gameStateClient = GameManager.GAME_STATE_CLIENT.playing;
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
            ServiceLocator.Instance.GetService<GameManager>()._gameStateClient = GameManager.GAME_STATE_CLIENT.ranking;
    }
}
