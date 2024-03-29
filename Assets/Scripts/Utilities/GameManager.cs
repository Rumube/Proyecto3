using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour, IGameManager
{
    [Header("Minigames scene names")]
    public List<string> _minigamesNames = new List<string>();
    public int _minigamesMaximumLevel = 5;
    [Header("Minigame student client")]
    public string _currentstudentName;
    public string _currentgameName;
    int _studentCounter = 0;
    public bool _pause = false;
    public bool _returnToCommonScene = false;
    public bool _endSessionTablet = false;
    private List<string> _notPresentStudents = new List<string>();

    public Dictionary<int, int> _teamPoints = new Dictionary<int, int>();

    public IGameManager.GAME_STATE_SERVER _gameStateServer;
    public IGameManager.GAME_STATE_CLIENT _gameStateClient;

    public static GameManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    void Start()
    {
      _gameStateServer = IGameManager.GAME_STATE_SERVER.init;
    }
    public void PreviousConfigurationState()
    {
        _gameStateServer = IGameManager.GAME_STATE_SERVER.previousConfiguration;
    }
    public void ConnectionState()
    {
        _gameStateServer = IGameManager.GAME_STATE_SERVER.connection;
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
    public IGameManager.GAME_STATE_CLIENT GetClientState()
    {
        return _gameStateClient;
    }
    public void SetClientState(IGameManager.GAME_STATE_CLIENT gameStateClient)
    {
        _gameStateClient = gameStateClient;
    }
    public void SetServerState(IGameManager.GAME_STATE_SERVER gameStateServer)
    {
        _gameStateServer = gameStateServer;
    }
    public void SetReturnToCommonScene(bool value)
    {
        _returnToCommonScene = value;
    }
    IGameManager.GAME_STATE_SERVER IGameManager.GetServerState()
    {
        return _gameStateServer;
    }
    public void SetMinigames(string _names)
    {
        _minigamesNames.Add(_names);
    }
    public List<string> GetMinigames()
    {
        return _minigamesNames;
    }
    public GameObject GetGameObject()
    {
        return gameObject;
    }
    public void SetPause(bool value)
    {
        _pause = value;
    }
    public bool GetPause()
    {
        return _pause;
    }
    public void SetEndSessionTablet(bool value)
    {
        _endSessionTablet = value;
    }
    public bool GetEndSessionTablet()
    {
        return _endSessionTablet;
    }
    public void SetCurrentGameName(string name)
    {
        _currentgameName = name;
    }
    public string GetCurrentGameName()
    {
        return _currentgameName;
    }
    public void SetCurrentStudentName(string name)
    {
        _currentstudentName = name;
    }
    public string GetCurrentStudentName()
    {
        return _currentstudentName;
    }
    public void SetMinigamesMaximumLevel(int level)
    {
        _minigamesMaximumLevel = level;
    }
    public int GetMinigamesMaximumLevel()
    {
        return _minigamesMaximumLevel;
    }
    public bool GetReturnToCommonScene()
    {
        return _returnToCommonScene;
    }
    public void SetTeamPoints(int index, int points)
    {
        _teamPoints[index-1] += points;
        ServiceLocator.Instance.GetService<RankingServer>().UpdateRankingPoints(_teamPoints);
    }
    public void SetDictionaryPoints(Dictionary<int,int> teamsPoints)
    {
        _teamPoints = new Dictionary<int, int>(teamsPoints);
    }
    public Dictionary<int, int> GetTeamPoints()
    {
        return _teamPoints;
    }
    public void AddTeam(int index)
    {
        _teamPoints.Add(index, 0);
    }
    public List<string> GetNotPresentsStudents()
    {
        return _notPresentStudents;
    }
    public void SetNotPresentsStudents(string name)
    {
        _notPresentStudents.Add(name);
    }
    public void DeleteNotPresentsStudent(string name)
    {
        _notPresentStudents.Remove(name);
    }
}
