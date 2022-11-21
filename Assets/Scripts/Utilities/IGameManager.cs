using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameManager
{

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
    public void PreviousConfigurationState();
    public void ConnectionState();
    public void RandomizeStudentsList();
    public void Shuffle<T>(IList<T> list);
    public void SelectStudentAndGame();
    public GAME_STATE_CLIENT GetClientState();
    public GAME_STATE_SERVER GetServerState();
    public void SetClientState(GAME_STATE_CLIENT gameStateClient);
    public void SetServerState(GAME_STATE_SERVER gameStateServer);
    public void SetReturnToCommonScene(bool value);
    public void SetMinigames(string _names);
    public List<string> GetMinigames();
    public GameObject GetGameObject();
    public void SetPause(bool value);
    public bool GetPause();
    public void SetEndSessionTablet(bool value);
    public bool GetEndSessionTablet();
    public void SetCurrentGameName(string name);
    public string GetCurrentGameName();
    public void SetCurrentStudentName(string name);
    public string GetCurrentStudentName();
    public void SetMinigamesMaximumLevel(int level);
    public int GetMinigamesMaximumLevel();
    public bool GetReturnToCommonScene();
    public void SetTeamPoints(int index, int points);
    public void SetDictionaryPoints(Dictionary<int, int> teamsPoints);
    public void AddTeam(int index);
    public Dictionary<int, int> GetTeamPoints();
    public List<string> GetNotPresentsStudents();
    public void SetNotPresentsStudents(string name);
    public void DeleteNotPresentsStudent(string name);
}
