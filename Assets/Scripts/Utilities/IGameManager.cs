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
    /// <summary>
    /// Set <see cref="_gameStateServer"/> to <see cref="IGameManager.GAME_STATE_SERVER.previousConfiguration"/>
    /// </summary>
    public void PreviousConfigurationState();
    /// <summary>
    /// Set <see cref="_gameStateServer"/> to <see cref="IGameManager.GAME_STATE_SERVER.connection"/>
    /// </summary>
    public void ConnectionState();
    /// <summary>
    /// Call to the randomize students function
    /// </summary>
    public void RandomizeStudentsList();
    /// <summary>
    /// Randomize the students list
    /// </summary>
    /// <typeparam name="T">List type</typeparam>
    /// <param name="list">The list to randomize</param>
    public void Shuffle<T>(IList<T> list);
    /// <summary>
    /// Select a student and assign him/her a mini-game
    /// </summary>
    public void SelectStudentAndGame();
    /// <summary>
    /// Returns <see cref="_gameStateClient"/>
    /// </summary>
    /// <returns><see cref="_gameStateClient"/></returns>
    public GAME_STATE_CLIENT GetClientState();
    /// <summary>
    /// Return <see cref="_gameStateServer"/>
    /// </summary>
    /// <returns><see cref="_gameStateServer"/></returns>
    public GAME_STATE_SERVER GetServerState();
    /// <summary>
    /// Set to <see cref="_gameStateClient"/> the value gived
    /// </summary>
    /// <param name="gameStateClient">The new value</param>
    public void SetClientState(GAME_STATE_CLIENT gameStateClient);
    /// <summary>
    /// Set to <see cref="_gameStateServer"/> the value gived
    /// </summary>
    /// <param name="gameStateServer">The new value</param>
    public void SetServerState(GAME_STATE_SERVER gameStateServer);
    /// <summary>
    /// Set <see cref="_returnToCommonScene"/> value
    /// If is true the game move to Common Scene
    /// </summary>
    /// <param name="value">Value</param>
    public void SetReturnToCommonScene(bool value);
    /// <summary>
    /// Add a minigame to list of minigames
    /// </summary>
    /// <param name="_names">The minigame name</param>
    public void SetMinigames(string _names);
    /// <summary>
    /// Return a list of minigame names
    /// </summary>
    /// <returns><see cref="_minigamesNames"/></returns>
    public List<string> GetMinigames();
    /// <summary>
    /// Return the current GameObject
    /// </summary>
    /// <returns><see cref="GameObject"/></returns>
    public GameObject GetGameObject();
    /// <summary>
    /// Change the value of <see cref="_pause"/>
    /// </summary>
    /// <param name="value">The new value</param>
    public void SetPause(bool value);
    /// <summary>
    /// Returns <see cref="_pause"/>
    /// </summary>
    /// <returns><see cref="_pause"/></returns>
    public bool GetPause();
    /// <summary>
    /// Set <see cref="_endSessionTablet"/>
    /// </summary>
    /// <param name="value">The new value</param>
    public void SetEndSessionTablet(bool value);
    /// <summary>
    /// Return <see cref="_endSessionTablet"/>
    /// </summary>
    /// <returns><see cref="_endSessionTablet"/></returns>
    public bool GetEndSessionTablet();
    /// <summary>
    /// Set the name of the current game
    /// </summary>
    /// <param name="name">The new value</param>
    public void SetCurrentGameName(string name);
    /// <summary>
    /// Get the name of the current game
    /// </summary>
    /// <returns><see cref="_currentgameName"/></returns>
    public string GetCurrentGameName();
    /// <summary>
    /// Set the name of the current student
    /// </summary>
    /// <param name="name">The new name</param>
    public void SetCurrentStudentName(string name);
    /// <summary>
    /// Get the name of the current student
    /// </summary>
    /// <returns><see cref="_currentstudentName"/></returns>
    public string GetCurrentStudentName();
    /// <summary>
    /// Set the minigames maximun level
    /// </summary>
    /// <param name="level">The int lvl</param>
    public void SetMinigamesMaximumLevel(int level);
    /// <summary>
    /// Get the minigames minimun level
    /// </summary>
    /// <returns><see cref="_minigamesMaximumLevel"/></returns>
    public int GetMinigamesMaximumLevel();
    /// <summary>
    /// Returns <see cref="_returnToCommonScene"/>
    /// </summary>
    /// <returns><see cref="_returnToCommonScene"/></returns>
    public bool GetReturnToCommonScene();
    /// <summary>
    /// Add points to the gived team
    /// </summary>
    /// <param name="index">Team to add points</param>
    /// <param name="points">Points to add</param>
    public void SetTeamPoints(int index, int points);
    /// <summary>
    /// Updates the value of <see cref="_teamPoints"/>
    /// </summary>
    /// <param name="teamsPoints">New values</param>
    public void SetDictionaryPoints(Dictionary<int, int> teamsPoints);
    /// <summary>
    /// Add a team to the teams list <see cref="_teamPoints"/>
    /// </summary>
    /// <param name="index">New index = new team</param>
    public void AddTeam(int index);
    /// <summary>
    /// Returns <see cref="_teamPoints"/>
    /// </summary>
    /// <returns><see cref="_teamPoints"/></returns>
    public Dictionary<int, int> GetTeamPoints();
    /// <summary>
    /// Returns a list of not presents students
    /// </summary>
    /// <returns><see cref="_notPresentStudents"/></returns>
    public List<string> GetNotPresentsStudents();
    /// <summary>
    /// Add to <see cref="_notPresentStudents"/> the student gived
    /// </summary>
    /// <param name="name">The student name</param>
    public void SetNotPresentsStudents(string name);
    /// <summary>
    /// Remove from <see cref="_notPresentStudents"/> the student gived 
    /// </summary>
    /// <param name="name">The student to remove</param>
    public void DeleteNotPresentsStudent(string name);
}
