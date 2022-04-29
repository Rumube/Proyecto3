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

}
