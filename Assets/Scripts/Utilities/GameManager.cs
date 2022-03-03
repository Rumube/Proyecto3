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

    public InputField _IP;
    public InputField _port;
    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartServer()
    {
        Server server = new Server();
        server.createServer();
    }
    public void StartClient()
    {
        Client client = new Client();
        client.CreateClient(_IP.text,_port.text);
    }
}
