using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("TabletWindows")]
    public GameObject _initialScreen;
    public GameObject _credits;
    public GameObject _mainMenu;
    public GameObject _popupAddClass;
    public GameObject _popupDeleteClass;
    public GameObject _popupAddStudent;
    public GameObject _popupDeleteStudent;
    public GameObject _class;
    public GameObject _gameConnection;
    public GameObject _addStudent;
    public GameObject _gameTime;
    public GameObject _stadistics;
    public GameObject _finalScore;

    [Header("DatabaseUtilities")]
    public string _classNamedb;

    [Header("DatabaseUtilitiesClass")]
    public Text _classNameDeleting;
    public GameObject _classPanel;
    public GameObject _classButton;

    [Header("DatabaseUtilitiesStudent")]
    public Text _studentNameDeleting;
    public GameObject _studentPanel;
    public Text _classNameStudents;

    [Header("Game Configuration")]
    [HideInInspector]
    public int _timeSessionMinutes;
    [HideInInspector]
    public int _timeSessionSeconds;
    [HideInInspector]
    public int _timeMinigamesMinutes;
    [HideInInspector]
    public int _timeMinigamesSeconds;

    #region GameTime
    /// <summary>Set the time for the whole session</summary>
    /// <param name="minutes">The session's minutes</param>
    /// <param name="seconds">The session's seconds</param>
    public void SetTimeSession(string minutes, string seconds)
    {
        _timeSessionMinutes = int.Parse(minutes);
        _timeSessionSeconds = int.Parse(seconds);
        print("t " + _timeSessionMinutes + " : " + _timeSessionSeconds);
    }

    /// <summary>Set the time for all minigames</summary>
    /// <param name="minutes">The minigames's minutes </param>
    /// <param name="seconds">The minigames's seconds </param>
    public void SetTimeMinigames(string minutes, string seconds)
    {
        _timeMinigamesMinutes = int.Parse(minutes);
        _timeMinigamesSeconds = int.Parse(seconds);
        print("t " + _timeMinigamesMinutes + " : " + _timeMinigamesSeconds);
    }

    /// <summary>Just start the timer when the configuration is done</summary>
    public void StartGameSession()
    {
        ServiceLocator.Instance.GetService<GameManager>()._gameStateServer = GameManager.GAME_STATE_SERVER.playing;
        StartCoroutine(StartCountdown());
    }
    #endregion

    #region Stadistics
    /// <summary>Countdown session</summary>
    public IEnumerator StartCountdown()
    {
        while ((_timeSessionMinutes > 0 && _timeSessionSeconds >= 0) || (_timeSessionMinutes == 0 && _timeSessionSeconds > 0))
        {
            if (_timeSessionSeconds >= 1)
            {
                _timeSessionSeconds--;
            }
            if (_timeSessionMinutes > 0 && _timeSessionSeconds == 0)
            {
                _timeSessionMinutes--;
                _timeSessionSeconds = 59;
            }
            yield return new WaitForSeconds(1f);
        }
        //TODO: End session, send packages end
    }
    #endregion

}
