using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectDB : MonoBehaviour
{

    private DataService _dataService;
    public void StartDataBase()
    {
        _dataService = new DataService("Mininautas.db");
        _dataService.CreateDB();
    }
}
