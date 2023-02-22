using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AliensController : MonoBehaviour
{
    [Header("Team Aliens GameObjects")]
    public List<GameObject> _teamAliensGO = new List<GameObject>();
    private int _conectedTablet = 0;

    private void Start()
    {
        StartAliens();
    }
    /// <summary>
    /// Hide all Aliens in the scene
    /// </summary>
    private void HideAllAliens()
    {
        foreach (GameObject alien in _teamAliensGO)
        {
            alien.SetActive(false);
        }
    }
    /// <summary>
    /// And shows the alien associated with the current equipment
    /// </summary>
    public void StartAliens()
    {
        _conectedTablet = ServiceLocator.Instance.GetService<INetworkManager>().GetTeamColor();
        HideAllAliens();
        if(_conectedTablet >= 0)
        {
            _teamAliensGO[_conectedTablet].SetActive(true);
        }
    }
}
