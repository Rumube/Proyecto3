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

    public void SendMatchData()
    {
        throw new System.NotImplementedException();
    }
}
