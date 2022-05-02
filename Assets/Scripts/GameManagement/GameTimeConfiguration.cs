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
        _maxTime = (ServiceLocator.Instance.GetService<INetworkManager>().GetMinigameMinutes() * 60) + ServiceLocator.Instance.GetService<INetworkManager>().GetMinigameSeconds();
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
            ServiceLocator.Instance.GetService<IFrogMessage>().StopFrogSpeaker();
            ServiceLocator.Instance.GetService<IGameManager>().SetClientState(IGameManager.GAME_STATE_CLIENT.ranking);
            _canStartTime = false;
            ServiceLocator.Instance.GetService<INetworkManager>().SendMatchData();
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
