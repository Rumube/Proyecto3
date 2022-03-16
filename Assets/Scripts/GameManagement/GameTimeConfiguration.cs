using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTimeConfiguration : MonoBehaviour, IGameTimeConfiguration
{
    public Image _timeImage;
    [SerializeField]
    private float _maxTime;
    private float _currentTime;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            StartGameTime();
    }


    /// <summary>
    /// Playing time begins.
    /// </summary>
    public void StartGameTime()
    {
        _currentTime = _maxTime;
        StartCoroutine(TimeProgress());
    }

    /// <summary>
    /// Reduce the time of the game until it reaches 0.
    /// </summary>
    IEnumerator TimeProgress()
    {
        do
        {
            if(ServiceLocator.Instance.GetService<GameManager>()._gameStateClient == GameManager.GAME_STATE_CLIENT.playing)
            {
                yield return new WaitForSeconds(1f);
                _currentTime--;
                _timeImage.fillAmount = _currentTime / _maxTime;
            }

        } while (_currentTime > 0f);
        ServiceLocator.Instance.GetService<GameManager>()._gameStateClient = GameManager.GAME_STATE_CLIENT.ranking;
    }
}
