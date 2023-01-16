using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AliensController : MonoBehaviour
{
    [Header("Team Aliens GameObjects")]
    public List<GameObject> _teamAliensGO = new List<GameObject>();
    private int _conectedTablet = 0;

    private void Awake()
    {

    }

    private void Start()
    {
        StartAliens();
    }

    // Update is called once per frame
    void Update()
    {
        
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

    public void StartAliens()
    {
        _conectedTablet = ServiceLocator.Instance.GetService<INetworkManager>().GetTeamColor();
        HideAllAliens();
        if(_conectedTablet >= 0)
        {
            _teamAliensGO[_conectedTablet-1].SetActive(true);
        }
    }
}
