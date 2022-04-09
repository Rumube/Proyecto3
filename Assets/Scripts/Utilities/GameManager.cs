using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Minigames scene names")]
    public List<string> _minigamesNames = new List<string>();
    [Header("Minigame student client")]
    public string _currentstudentName;
    public string _currentgameName;
    int _studentCounter = 0;
    public bool _pause = false;
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

    void Start()
    {
        _gameStateServer = GAME_STATE_SERVER.init;
    }

    public void PreviousConfigurationState()
    {
        _gameStateServer = GAME_STATE_SERVER.previousConfiguration;
    }
    public void ConnectionState()
    {
        _gameStateServer = GAME_STATE_SERVER.connection;
    }

    public void RandomizeStudentsList()
    {
        Shuffle(Client._tablet._students);
    }

    public void Shuffle<T>(IList<T> list)
    {
        System.Random random = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
    public void SelectStudentAndGame()
    {
        if (_studentCounter == Client._tablet._students.Count)
        {
            _studentCounter = 0;
        }
        _currentstudentName = Client._tablet._students[_studentCounter]._name;
        _currentgameName = _minigamesNames[Random.Range(0, _minigamesNames.Count)];
        _studentCounter++;
    }
}
