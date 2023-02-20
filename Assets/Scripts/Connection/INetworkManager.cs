using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INetworkManager
{
    /// <summary>
    /// Send the following match data
    /// </summary>
    public void SendMatchData();
    /// <summary>
    /// Returns the minutes of the mini-game
    /// </summary>
    /// <returns>Minutes in int</returns>
    public int GetMinigameMinutes();
    /// <summary>
    /// Returns the seconds of the mini-game (No longer in use)
    /// </summary>
    /// <returns>Minutes in int</returns>
    public int GetMinigameSeconds();
    /// <summary>
    /// Returns the level of the mini-game using the children stats
    /// </summary>
    /// <returns>Level in int</returns>
    public int GetMinigameLevel();
    /// <summary>
    /// Set the ip value
    /// </summary>
    /// <param name="value">The new ip</param>
    public void SetIp(string value);
    /// <summary>
    /// Returns the table ip
    /// </summary>
    /// <returns>The ip</returns>
    public string GetIp();
    /// <summary>
    /// Set the port value
    /// </summary>
    /// <param name="value">The port</param>
    public void SetPort(string value);
    /// <summary>
    /// Return the port
    /// </summary>
    /// <returns>The port</returns>
    public string GetPort();
    /// <summary>
    /// Set the mini-game minutes
    /// </summary>
    /// <param name="minutes">The minutes</param>
    public void SetMinigamesMinutes(int minutes);
    /// <summary>
    /// Return the mini-games minutes
    /// </summary>
    /// <returns>The minutes</returns>
    public int GetMinigamesMinutes();
    /// <summary>
    /// Set the mini-game seconds (No longer in use)
    /// </summary>
    /// <param name="seconds">The seconds</param>
    public void SetMinigamesSeconds(int seconds);
    /// <summary>
    /// Returns the seconds of the mini-game (No longer in use)
    /// </summary>
    /// <returns>Minutes in int</returns>
    public int GetMinigamesSeconds();
    /// <summary>
    /// Set the level of the mini-game
    /// </summary>
    /// <param name="level">The new level</param>
    public void SetMinigamesLevel(int level);
    /// <summary>
    /// Returns the mini-game level
    /// </summary>
    /// <returns>The level</returns>
    public int GetMinigamesLevel();
    /// <summary>
    /// Set the selected tablet to assign students
    /// </summary>
    /// <param name="selectedTablet">The selected tablet</param>
    public void SetSelectedTablet(int selectedTablet);
    /// <summary>
    /// Returns the selected tablet to assing students
    /// </summary>
    /// <returns>The selected tablet</returns>
    public int GetSelectedTablet();
    /// <summary>
    /// The process of assigning a student to the current tablet begins
    /// </summary>
    /// <param name="tablet">The current tablet</param>
    public void SetStudentToTablet(Tablet tablet);
    /// <summary>
    /// Returns the list of tablets
    /// </summary>
    /// <returns>Tablets</returns>
    public List<Tablet> GetStudentsToTablets();
    /// <summary>
    /// Send a package when a student finish the games and needs to view the final score
    /// </summary>
    public void SendViewingFinalScore();
    /// <summary>Add or remove childrens on selected tablet</summary>
    /// <param name="student">student data</param>
    /// <param name="add">Add or remove student</param>
    public void AddRemoveChildrenToTablet(Student student, bool add);
    /// <summary>
    /// Returns the total of connected tablets
    /// </summary>
    /// <returns>The number of tablets</returns>
    public int GetConnectedTablets();
    /// <summary>Get if a new tablet is connected</summary>
    public bool GetUpdateConnectedTablets();
    /// <summary>Rewrite the variable</summary>
    /// <param name="state">The state of the variable</param>
    public void SetUpdateConnectedTablets(bool state);
    /// <summary>Check if every tablet has at least one student added</summary>
    public bool CheckIfTabletsHasStudents();
    /// <summary>It's called when every student has been called to the rocket</summary>
    public void SendEndCalling();
    /// <summary>It's called when a student and game has been selected to play</summary>
    public void SendStudentGame();
    /// <summary>Get a specific Tablet</summary>
    /// <param name="i">Tablet index</param>
    public Tablet GetTablets(int i);
    /// <summary>
    /// Randomize the active students in the class and distribute
    /// them among the different tablets equally. 
    /// </summary>
    public void RandomizeStudents();
    /// <summary>
    /// Set the color of the tablet
    /// </summary>
    /// <param name="teamColor">The color</param>
    public void SetTeamColor(int teamColor);
    /// <summary>
    /// Returns the color of the team
    /// </summary>
    /// <returns>The color</returns>
    public int GetTeamColor();
}
