using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectDB : MonoBehaviour
{

    private DataService _dataService;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartDataBase()
    {
        _dataService = new DataService("Mininautas.db");
        _dataService.CreateDB();
    }
}
