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

    public int GetMinigameMinutes()
    {
        return _minigameMinutes;
    }

    public int GetMinigameSeconds()
    {
        return _minigameSeconds;
    }

    public int GetMinigameLevel()
    {
        return _minigameLevel;
    }

    public void SendMatchData()
    {
        return;
    }

    public void SetIp(string value)
    {
        throw new System.NotImplementedException();
    }

    public string GetIp()
    {
        throw new System.NotImplementedException();
    }

    public void SetPort(string value)
    {
        throw new System.NotImplementedException();
    }

    public string GetPort()
    {
        throw new System.NotImplementedException();
    }

    public void SetMinigamesMinutes(int minutes)
    {
        throw new System.NotImplementedException();
    }

    public int GetMinigamesMinutes()
    {
        throw new System.NotImplementedException();
    }

    public void SetMinigamesSeconds(int seconds)
    {
        throw new System.NotImplementedException();
    }

    public int GetMinigamesSeconds()
    {
        throw new System.NotImplementedException();
    }

    public void SetMinigamesLevel(int level)
    {
        throw new System.NotImplementedException();
    }

    public int GetMinigamesLevel()
    {
        throw new System.NotImplementedException();
    }

    public void SetSelectedTablet(int selectedTablet)
    {
        throw new System.NotImplementedException();
    }

    public int GetSelectedTablet()
    {
        throw new System.NotImplementedException();
    }

    public void SetStudentToTablet(Tablet tablet)
    {
        throw new System.NotImplementedException();
    }

    public List<Tablet> GetStudentsToTablets()
    {
        throw new System.NotImplementedException();
    }

    public void SendViewingFinalScore()
    {
        throw new System.NotImplementedException();
    }

    public void AddRemoveChildrenToTablet(Student student, bool add)
    {
        throw new System.NotImplementedException();
    }

    public int GetConnectedTablets()
    {
        throw new System.NotImplementedException();
    }

    public bool GetUpdateConnectedTablets()
    {
        throw new System.NotImplementedException();
    }

    public void SetUpdateConnectedTablets(bool state)
    {
        throw new System.NotImplementedException();
    }

    public bool CheckIfTabletsHasStudents()
    {
        throw new System.NotImplementedException();
    }

    public void SendEndCalling()
    {
        throw new System.NotImplementedException();
    }

    public void SendStudentGame()
    {
        throw new System.NotImplementedException();
    }

    public Tablet GetTablets(int i)
    {
        throw new System.NotImplementedException();
    }
}
