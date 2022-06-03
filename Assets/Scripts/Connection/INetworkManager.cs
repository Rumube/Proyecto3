using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INetworkManager
{
    public void SendMatchData();
    public int GetMinigameMinutes();
    public int GetMinigameSeconds();
    public int GetMinigameLevel();
    public void SetIp(string value);
    public string GetIp();
    public void SetPort(string value);
    public string GetPort();
    public void SetMinigamesMinutes(int minutes);
    public int GetMinigamesMinutes();
    public void SetMinigamesSeconds(int seconds);
    public int GetMinigamesSeconds();
    public void SetMinigamesLevel(int level);
    public int GetMinigamesLevel();
    public void SetSelectedTablet(int selectedTablet);
    public int GetSelectedTablet();
    public void SetStudentToTablet(Tablet tablet);
    public List<Tablet> GetStudentsToTablets();
    public void SendViewingFinalScore();
    public void AddRemoveChildrenToTablet(Student student, bool add);
    public int GetConnectedTablets();
    public bool GetUpdateConnectedTablets();
    public void SetUpdateConnectedTablets(bool state);
    public bool CheckIfTabletsHasStudents();
    public void SendEndCalling();
    public void SendStudentGame();
    public Tablet GetTablets(int i);
}
