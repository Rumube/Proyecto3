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
       _conectedTablet = ServiceLocator.Instance.GetService<INetworkManager>().GetSelectedTablet();
        HideAllAliens();
        _teamAliensGO[_conectedTablet].SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        
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
}
