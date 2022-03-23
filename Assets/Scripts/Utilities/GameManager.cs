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

    Client client;
    public Text _idText;


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

    // Start is called before the first frame update
    void Start()
    {
        _gameStateServer = GAME_STATE_SERVER.init;
    }

    // Update is called once per frame
    void Update()
    {
        if (Client._allPackages != null && Client._allPackages.Count > 0)
        {
            Client.DoUpdate();
            _idText.text = Client._tablet._id.ToString();
        }
    }

    /// <summary>Starts a new server and provide the ip and port's device</summary>
    public void StartServer()
    {
        string[] connectionData = new string[2];

        server = new Server();
        connectionData = server.createServer();

        _ip = connectionData[0];
        _port = connectionData[1];

        _gameStateServer = GAME_STATE_SERVER.connection;
    }

    /// <summary>Get the number of tablets that are connected</summary>
    public int GetConnectedTablets()
    {
        return Server._connectedTablets;
    }
    public Tablet GetTablets(int i)
    {
        return Server._tablets[i];
    }
    /// <summary>Get if a new tablet is connected</summary>
    public bool GetUpdateConnectedTablets()
    {
        return Server._updateConnectedTablets;
    }

    /// <summary>Rewrite the variable</summary>
    /// <param name="state">The state of the variable</param>
    public void SetUpdateConnectedTablets(bool state)
    {
        Server._updateConnectedTablets = state;
    }

    /// <summary>Starts a new client connecting to a server with specific IP and port </summary>
    public void StartClient()
    {
        client = new Client();
        client.CreateClient(_IPTest.text,_portTest.text);
    }

    /// <summary>Set the time for the whole session</summary>
    /// <param name="minutes">The session's minutes</param>
    /// <param name="seconds">The session's seconds</param>
    public void SetTimeSession(string minutes, string seconds)
    {
        _timeSessionMinutes = int.Parse(minutes);
        _timeSessionSeconds = int.Parse(seconds);
        print("t "+ _timeSessionMinutes + " : "+ _timeSessionSeconds);
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
        _gameStateServer = GAME_STATE_SERVER.playing;
        StartCoroutine(StartCountdown());
    }

    /// <summary>Countdown</summary>
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
    private void OnDisable()
    {
        Server.OnDisable();
        Client.OnDisable();
    }
    public void ResetConnections()
    {
        Server.OnDisable();
        Client.OnDisable();
    }
}
