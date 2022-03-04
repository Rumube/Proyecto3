using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public enum GAME_STATE_SERVER
    {
        init = 0,
        previousConfiguration = 1,
        connection = 2,
        teamConfiguration = 3,
        playing = 4,
        pause = 5,
        quit = 6,
        disconnect = 7

    }
    public GAME_STATE_SERVER _gameStateServer;

    public enum GAME_STATE_CLIENT
    {
        init = 0,
        searching = 1,
        selectStudent = 2,
        playing = 3,
        pause = 4,
        gameOver = 5,
        ranking = 6,
        globalRanking = 7,

    }
    public GAME_STATE_CLIENT _gameStateClient;

    [Header("Windows")]
    public GameObject _initialScreen;
    public GameObject _credits;
    public GameObject _mainMenu;
    public GameObject _class;
    public GameObject _gameConnection;
    public GameObject _addStudent;
    public GameObject _gameTime;
    public GameObject _stadistics;
    public GameObject _finalScore;

    //Testing 
    public InputField _IPTest;
    public InputField _portTest;

    [Header("Game Configuration")]
    public int _timeSessionMinutes;
    public int _timeSessionSeconds;
    public int _timeMinigamesMinutes;
    public int _timeMinigamesSeconds;

    [Header("Game Connection")]
    public string _ip;
    public string _port;
    Server server;
    // Start is called before the first frame update
    void Start()
    {
        _gameStateServer = GAME_STATE_SERVER.init;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartServer()
    {
        string[] connectionData = new string[2];

        server = new Server();
        connectionData = server.createServer();

        _ip = connectionData[0];
        _port = connectionData[1];

        _gameStateServer = GAME_STATE_SERVER.connection;
    }
    public int GetConnectedTablets()
    {
        return Server._connectedTablets;
    }
    public bool GetUpdateConnectedTablets()
    {
        return Server._updateConnectedTablets;
    }
    public void SetUpdateConnectedTablets(bool state)
    {
        Server._updateConnectedTablets = state;
    }
    public void StartClient()
    {
        Client client = new Client();
        client.CreateClient(_IPTest.text,_portTest.text);
    }
    /** 
      * @desc Set the time for the whole session
      * @param int minutes - The session's minutes 
      * @param int seconds - The session's seconds 
      */
    public void SetTimeSession(string minutes, string seconds)
    {
        _timeSessionMinutes = int.Parse(minutes);
        _timeSessionSeconds = int.Parse(seconds);
        print("t "+ _timeSessionMinutes + " : "+ _timeSessionSeconds);
    }
    /** 
     * @desc Set the time for all minigames
     * @param int minutes - The minigames's minutes 
     * @param int seconds - The minigames's seconds 
     */
    public void SetTimeMinigames(string minutes, string seconds)
    {
        _timeMinigamesMinutes = int.Parse(minutes);
        _timeMinigamesSeconds = int.Parse(seconds);
        print("t " + _timeMinigamesMinutes + " : " + _timeMinigamesSeconds);
    }
    /** 
    * @desc Just start the timer when the configuration is done
    */
    public void StartGameSession()
    {
        StartCoroutine(StartCountdown());
    }
    /** 
   * @desc Countdown
   */
    public IEnumerator StartCountdown()
    {
        while((_timeSessionMinutes > 0 && _timeSessionSeconds >= 0) || (_timeSessionMinutes == 0 && _timeSessionSeconds > 0))
        {
            if (_timeSessionSeconds >= 1)
            {
                _timeSessionSeconds--;
            }
            if(_timeSessionMinutes > 0 &&_timeSessionSeconds == 0)
            {
                _timeSessionMinutes--;
                _timeSessionSeconds = 59;
            }
            yield return new WaitForSeconds(1f);
        }
        //TODO: End session, send packages end
    }
}
