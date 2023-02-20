using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GMSinBucle : MonoBehaviour, IGameManager
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
    public IGameManager.GAME_STATE_CLIENT _gameStateClient;

    void Start()
    {
        _gameStateServer = GAME_STATE_SERVER.init;
    }
    /// <summary>
    /// Set the previous configuration to <see cref="_gameStateServer"/>
    /// </summary>
    public void PreviousConfigurationState()
    {
        _gameStateServer = GAME_STATE_SERVER.previousConfiguration;
    }
    /// <summary>
    /// Set the <see cref="_gameStateServer"/> to <see cref="GAME_STATE_SERVER.connection"/>
    /// </summary>
    public void ConnectionState()
    {
        _gameStateServer = GAME_STATE_SERVER.connection;
    }
    /// <summary>
    /// Button call: Start <see cref="Shuffle{T}(IList{T})"/>
    /// </summary>
    public void RandomizeStudentsList()
    {
        Shuffle(Client._tablet._students);
    }
    /// <summary>
    /// Randomize the list gived
    /// </summary>
    /// <typeparam name="T">List type</typeparam>
    /// <param name="list">List to randomize</param>
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
    /// <summary>
    /// Don't need to work
    /// </summary>
    public void SelectStudentAndGame()
    {
        throw new System.NotImplementedException();
    }
    /// <summary>
    /// Returns the value <see cref="_gameStateClient"/>
    /// </summary>
    /// <returns><see cref="_gameStateClient"/></returns>
    public IGameManager.GAME_STATE_CLIENT GetClientState()
    {
        return _gameStateClient;
    }
    /// <summary>
    /// Set a value to <see cref="_gameStateClient"/>
    /// </summary>
    /// <param name="gameStateClient">new <see cref="IGameManager.GAME_STATE_CLIENT"/></param>
    public void SetClientState(IGameManager.GAME_STATE_CLIENT gameStateClient)
    {
        _gameStateClient = gameStateClient;
    }
    /// <summary>
    /// Set a value to <see cref="_returnToCommonScene"/>
    /// </summary>
    /// <param name="value">new value</param>
    public void SetReturnToCommonScene(bool value)
    {
        _returnToCommonScene = value;
    }
    /// <summary>
    /// Don't need to work
    /// </summary>
    public IGameManager.GAME_STATE_CLIENT GetServerState()
    {
        throw new System.NotImplementedException();
    }
    /// <summary>
    /// Don't need to work
    /// </summary>
    public void SetServerState(IGameManager.GAME_STATE_SERVER gameStateServer)
    {
        throw new System.NotImplementedException();
    }
    /// <summary>
    /// Don't need to work
    /// </summary>
    IGameManager.GAME_STATE_SERVER IGameManager.GetServerState()
    {
        throw new System.NotImplementedException();
    }
    /// <summary>
    /// Don't need to work
    /// </summary>
    public void SetMinigames(string _names)
    {
        throw new System.NotImplementedException();
    }
    /// <summary>
    /// Don't need to work
    /// </summary>
    public List<string> GetMinigames()
    {
        throw new System.NotImplementedException();
    }
    /// <summary>
    /// Don't need to work
    /// </summary>
    public GameObject GetGameObject()
    {
        throw new System.NotImplementedException();
    }
    /// <summary>
    /// Don't need to work
    /// </summary>
    public void SetPause(bool value)
    {
        throw new System.NotImplementedException();
    }
    /// <summary>
    /// Don't need to work
    /// </summary>
    public bool GetPause()
    {
        throw new System.NotImplementedException();
    }
    /// <summary>
    /// Don't need to work
    /// </summary>
    public void SetEndSessionTablet(bool value)
    {
        throw new System.NotImplementedException();
    }
    /// <summary>
    /// Don't need to work
    /// </summary>
    public bool GetEndSessionTablet()
    {
        throw new System.NotImplementedException();
    }
    /// <summary>
    /// Don't need to work
    /// </summary>
    public void SetCurrentGameName(string name)
    {
        throw new System.NotImplementedException();
    }
    /// <summary>
    /// Don't need to work
    /// </summary>
    public string GetCurrentGameName()
    {
        throw new System.NotImplementedException();
    }
    /// <summary>
    /// Don't need to work
    /// </summary>
    public void SetCurrentStudentName(string name)
    {
        throw new System.NotImplementedException();
    }
    /// <summary>
    /// Returns the current student name
    /// </summary>
    /// <returns><see cref="_currentstudentName"/></returns>
    public string GetCurrentStudentName()
    {
        return _currentstudentName;
    }
    /// <summary>
    /// Don't need to work
    /// </summary>
    public void SetMinigamesMaximumLevel(int level)
    {
        throw new System.NotImplementedException();
    }
    /// <summary>
    /// Don't need to work
    /// </summary>
    public int GetMinigamesMaximumLevel()
    {
        throw new System.NotImplementedException();
    }
    /// <summary>
    /// Don't need to work
    /// </summary>
    public bool GetReturnToCommonScene()
    {
        throw new System.NotImplementedException();
    }
    /// <summary>
    /// Don't need to work
    /// </summary>
    public void SetTeamPoints(int index, int points)
    {
        throw new System.NotImplementedException();
    }
    /// <summary>
    /// Don't need to work
    /// </summary>
    public Dictionary<int, int> GetTeamPoints()
    {
        Dictionary<int, int> points = new Dictionary<int, int>();
        points.Add(0, 100);
        points.Add(1, 50);
        points.Add(2, 100);
        points.Add(3, 200);
        points.Add(4, 107);
        points.Add(5, 250);
        return points;
    }
    /// <summary>
    /// Don't need to work
    /// </summary>
    public void AddTeam(int index)
    {
        throw new System.NotImplementedException();
    }
    /// <summary>
    /// Don't need to work
    /// </summary>
    public void SetDictionaryPoints(Dictionary<int, int> teamsPoints)
    {
        throw new System.NotImplementedException();
    }
    /// <summary>
    /// Don't need to work
    /// </summary>
    public List<string> GetNotPresentsStudents()
    {
        throw new System.NotImplementedException();
    }
    /// <summary>
    /// Don't need to work
    /// </summary>
    public void SetNotPresentsStudents(string name)
    {
        throw new System.NotImplementedException();
    }
    /// <summary>
    /// Don't need to work
    /// </summary>
    public void DeleteNotPresentsStudent(string name)
    {
        throw new System.NotImplementedException();
    }
}
