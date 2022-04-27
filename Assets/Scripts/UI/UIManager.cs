using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("MovileWindows")]
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

    [Header("TabletWindows")]
    public GameObject _initialScreenTablet;
    public GameObject _creditsTablet;
    public GameObject _connection;
    public GameObject _studentSelection;
    public GameObject _gameSelection;
    public GameObject _finalScoreTablet;

    [Header("DatabaseUtilities")]
    public string _classNamedb;

    [Header("DatabaseUtilitiesClass")]
    public TextMeshProUGUI _classNameDeleting;
    public GameObject _classPanel;
    public GameObject _classButton;

    [Header("DatabaseUtilitiesStudent")]
    public TextMeshProUGUI _studentNameDeleting;
    public GameObject _studentPanel;
    public TextMeshProUGUI _classNameStudents;

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
    }

    /// <summary>Set the time for all minigames</summary>
    /// <param name="minutes">The minigames's minutes </param>
    /// <param name="seconds">The minigames's seconds </param>
    public void SetTimeMinigames(string minutes, string seconds)
    {
        _timeMinigamesMinutes = int.Parse(minutes);
        _timeMinigamesSeconds = int.Parse(seconds);
    }

    /// <summary>Just start the timer when the configuration is done and show the number of tablets that are connected</summary>
    public void StartGameSession()
    {
        ServiceLocator.Instance.GetService<GameManager>()._gameStateServer = GameManager.GAME_STATE_SERVER.playing;
        StartCoroutine(StartCountdown());

        RectTransform rt = ServiceLocator.Instance.GetService<MobileUI>()._tabletsPanelStadistics.GetComponent(typeof(RectTransform)) as RectTransform;
        switch (ServiceLocator.Instance.GetService<NetworkManager>().GetConnectedTablets())
        {
            //Change measures for bigger ones
            case 1:
            case 2:
               rt.sizeDelta = new Vector2(620,rt.sizeDelta.y);
                break;
            case 3:
                rt.sizeDelta = new Vector2(745, rt.sizeDelta.y);
                break;
            case 4:
                rt.sizeDelta = new Vector2(1006, rt.sizeDelta.y);
                break;
            case 5:
                rt.sizeDelta = new Vector2(1275, rt.sizeDelta.y);
                break;
            case 6:
                rt.sizeDelta = new Vector2(1550, rt.sizeDelta.y);
                break;
        }
        for (int i = 0; i < ServiceLocator.Instance.GetService<NetworkManager>().GetConnectedTablets(); ++i)
        {
            GameObject newButton = Instantiate(ServiceLocator.Instance.GetService<MobileUI>()._tabletButtonPrefabStadistics.gameObject, ServiceLocator.Instance.GetService<MobileUI>()._tabletsPanelStadistics.gameObject.transform);
            newButton.transform.GetChild(1).GetComponentInChildren<Image>().sprite = newButton.GetComponent<TabletButton>()._rocketSprites[i];
            newButton.GetComponent<TabletButton>().index_rocket = i+1;
        }      
    }
    #endregion

    #region Stadistics
    /// <summary>Countdown session</summary>
    public IEnumerator StartCountdown()
    {
        while ((_timeSessionMinutes > 0 && _timeSessionSeconds >= 0) || (_timeSessionMinutes == 0 && _timeSessionSeconds > 0))
        {
            yield return new WaitUntil(() => ServiceLocator.Instance.GetService<GameManager>()._pause == false);
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
        ServiceLocator.Instance.GetService<MobileUI>().ShownFinalScoreScreen();
    }
    #endregion

}
