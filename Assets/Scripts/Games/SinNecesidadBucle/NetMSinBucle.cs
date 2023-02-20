using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetMSinBucle : MonoBehaviour,INetworkManager
{
    [Header("Minigame timer client")]
    public int _minigameMinutes;
    public int _minigameSeconds;

    [Header("Minigame difficulty server")]
    public int _minigameLevel = -1;
    /// <summary>
    /// Returns the minigame duration in minutes
    /// </summary>
    /// <returns>minutes</returns>
    public int GetMinigameMinutes()
    {
        return _minigameMinutes;
    }
    /// <summary>
    /// Don't need to work
    /// </summary>
    public int GetMinigameSeconds()
    {
        return _minigameSeconds;
    }
    /// <summary>
    /// Returns minigame level
    /// </summary>
    /// <returns>level</returns>
    public int GetMinigameLevel()
    {
        return _minigameLevel;
    }
    /// <summary>
    /// Don't need to work
    /// </summary>
    public void SendMatchData()
    {
        return;
    }
    /// <summary>
    /// Don't need to work
    /// </summary>
    public void SetIp(string value)
    {
        throw new System.NotImplementedException();
    }
    /// <summary>
    /// Don't need to work
    /// </summary>
    public string GetIp()
    {
        throw new System.NotImplementedException();
    }
    /// <summary>
    /// Don't need to work
    /// </summary>
    public void SetPort(string value)
    {
        throw new System.NotImplementedException();
    }
    /// <summary>
    /// Don't need to work
    /// </summary>
    public string GetPort()
    {
        throw new System.NotImplementedException();
    }
    /// <summary>
    /// Don't need to work
    /// </summary>
    public void SetMinigamesMinutes(int minutes)
    {
        throw new System.NotImplementedException();
    }
    /// <summary>
    /// Don't need to work
    /// </summary>
    public int GetMinigamesMinutes()
    {
        throw new System.NotImplementedException();
    }
    /// <summary>
    /// Don't need to work
    /// </summary>
    public void SetMinigamesSeconds(int seconds)
    {
        throw new System.NotImplementedException();
    }
    /// <summary>
    /// Don't need to work
    /// </summary>
    public int GetMinigamesSeconds()
    {
        throw new System.NotImplementedException();
    }
    /// <summary>
    /// Don't need to work
    /// </summary>
    public void SetMinigamesLevel(int level)
    {
        throw new System.NotImplementedException();
    }
    /// <summary>
    /// Don't need to work
    /// </summary>
    public int GetMinigamesLevel()
    {
        throw new System.NotImplementedException();
    }
    /// <summary>
    /// Don't need to work
    /// </summary>
    public void SetSelectedTablet(int selectedTablet)
    {
        throw new System.NotImplementedException();
    }
    /// <summary>
    /// Don't need to work
    /// </summary>
    public int GetSelectedTablet()
    {
        return Random.Range(0, 6);
    }
    /// <summary>
    /// Don't need to work
    /// </summary>
    public void SetStudentToTablet(Tablet tablet)
    {
        throw new System.NotImplementedException();
    }
    /// <summary>
    /// Don't need to work
    /// </summary>
    public List<Tablet> GetStudentsToTablets()
    {
        throw new System.NotImplementedException();
    }
    /// <summary>
    /// Don't need to work
    /// </summary>
    public void SendViewingFinalScore()
    {
        throw new System.NotImplementedException();
    }
    /// <summary>
    /// Don't need to work
    /// </summary>
    public void AddRemoveChildrenToTablet(Student student, bool add)
    {
        throw new System.NotImplementedException();
    }
    /// <summary>
    /// Don't need to work
    /// </summary>
    public int GetConnectedTablets()
    {
        throw new System.NotImplementedException();
    }
    /// <summary>
    /// Don't need to work
    /// </summary>
    public bool GetUpdateConnectedTablets()
    {
        throw new System.NotImplementedException();
    }
    /// <summary>
    /// Don't need to work
    /// </summary>
    public void SetUpdateConnectedTablets(bool state)
    {
        throw new System.NotImplementedException();
    }
    /// <summary>
    /// Don't need to work
    /// </summary>
    public bool CheckIfTabletsHasStudents()
    {
        throw new System.NotImplementedException();
    }
    /// <summary>
    /// Don't need to work
    /// </summary>
    public void SendEndCalling()
    {
        throw new System.NotImplementedException();
    }
    /// <summary>
    /// Don't need to work
    /// </summary>
    public void SendStudentGame()
    {
        throw new System.NotImplementedException();
    }
    /// <summary>
    /// Don't need to work
    /// </summary>
    public Tablet GetTablets(int i)
    {
        throw new System.NotImplementedException();
    }
    /// <summary>
    /// Don't need to work
    /// </summary>
    public void RandomizeStudents()
    {
        throw new System.NotImplementedException();
    }
    /// <summary>
    /// Don't need to work
    /// </summary>
    public void SetTeamColor(int teamColor)
    {
        throw new System.NotImplementedException();
    }
    /// <summary>
    /// Don't need to work
    /// </summary>
    public int GetTeamColor()
    {
        return Random.Range(0, 6);
    }
}
