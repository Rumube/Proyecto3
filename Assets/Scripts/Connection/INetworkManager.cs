using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INetworkManager
{
    public void SendMatchData();
    public int GetMinigameMinutes();
    public int GetMinigameSeconds();
}
