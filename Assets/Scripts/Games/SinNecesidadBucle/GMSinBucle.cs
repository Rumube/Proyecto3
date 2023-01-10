using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GMSinBucle : MonoBehaviour,IGameManager
{
    [Header("Minigame student client")]
    public string _currentstudentName;
    public bool _pause = false;
    public bool _returnToCommonScene = false;

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

    //public enum GAME_STATE_CLIENT
    //{
    //    init = 0,
    //    searching = 1,
    //    selectStudent = 2,
    //    playing = 3,
    //    pause = 4,
    //    gameOver = 5,
    //    ranking = 6,
    //    globalRanking = 7,

    //}
    public IGameManager.GAME_STATE_CLIENT _gameStateClient;

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
        throw new System.NotImplementedException();
    }

    public IGameManager.GAME_STATE_CLIENT GetClientState()
    {
        return _gameStateClient;
    }
    public void SetClientState(IGameManager.GAME_STATE_CLIENT gameStateClient)
    {
        _gameStateClient = gameStateClient;
    }
    public void SetReturnToCommonScene(bool value)
    {
        _returnToCommonScene = value;
    }

    public IGameManager.GAME_STATE_CLIENT GetServerState()
    {
        throw new System.NotImplementedException();
    }

    public void SetServerState(IGameManager.GAME_STATE_SERVER gameStateServer)
    {
        throw new System.NotImplementedException();
    }

    IGameManager.GAME_STATE_SERVER IGameManager.GetServerState()
    {
        throw new System.NotImplementedException();
    }

    public void SetMinigames(string _names)
    {
        throw new System.NotImplementedException();
    }

    public List<string> GetMinigames()
    {
        throw new System.NotImplementedException();
    }

    public GameObject GetGameObject()
    {
        throw new System.NotImplementedException();
    }

    public void SetPause(bool value)
    {
        throw new System.NotImplementedException();
    }

    public bool GetPause()
    {
        throw new System.NotImplementedException();
    }

    public void SetEndSessionTablet(bool value)
    {
        throw new System.NotImplementedException();
    }

    public bool GetEndSessionTablet()
    {
        throw new System.NotImplementedException();
    }

    public void SetCurrentGameName(string name)
    {
        throw new System.NotImplementedException();
    }

    public string GetCurrentGameName()
    {
        throw new System.NotImplementedException();
    }

    public void SetCurrentStudentName(string name)
    {
        throw new System.NotImplementedException();
    }

    public string GetCurrentStudentName()
    {
        return _currentstudentName;
    }

    public void SetMinigamesMaximumLevel(int level)
    {
        throw new System.NotImplementedException();
    }

    public int GetMinigamesMaximumLevel()
    {
        throw new System.NotImplementedException();
    }

    public bool GetReturnToCommonScene()
    {
        throw new System.NotImplementedException();
    }

    public void SetTeamPoints(int index, int points)
    {
        throw new System.NotImplementedException();
    }

    public Dictionary<int, int> GetTeamPoints()
    {
        Dictionary<int, int> points = new Dictionary<int, int>();
        points.Add(0, 100);
        points.Add(1, 450);
        points.Add(2, 400);
        return points;
    }

    public void AddTeam(int index)
    {
        throw new System.NotImplementedException();
    }

    public void SetDictionaryPoints(Dictionary<int, int> teamsPoints)
    {
        throw new System.NotImplementedException();
    }

    public List<string> GetNotPresentsStudents()
    {
        throw new System.NotImplementedException();
    }

    public void SetNotPresentsStudents(string name)
    {
        throw new System.NotImplementedException();
    }

    public void DeleteNotPresentsStudent(string name)
    {
        throw new System.NotImplementedException();
    }
}
