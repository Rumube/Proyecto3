using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

public class Client
{
    public static Tablet _tablet;

    static WebSocket _ws;
    Package _clientPackage;
    public static List<Package> _allPackages;

    public void InitializeData()
    {
        _clientPackage = new Package();

        _tablet = new Tablet();
        _tablet._id = -1;
    }

    public void CreateClient(string ip, string port)
    {
        string path = "ws://" + ip + ":" + port;
        _ws = new WebSocket(path);
        _ws.OnMessage += Ws_OnMessage;   // evento para recibir los mensajer
        _ws.Connect();
        EDebug.Log("me he conectado al servidor... listo para enviar y recibir");
        _allPackages = new List<Package>();

        InitializeData();

    }
    private void Ws_OnMessage(object sender, MessageEventArgs e)
    {
        _clientPackage = JsonUtility.FromJson<Package>(e.Data);
        EDebug.Log("Message received! " + sender.ToString() + " data: " + _clientPackage._sendID._idTablet);

        _allPackages.Add(_clientPackage);
    }

    public static void DoUpdate()
    {
        _tablet._id = _allPackages[0]._sendID._idTablet;
        EDebug.Log(_tablet._id);
        _allPackages.Remove(_allPackages[0]);
    }
    public static void OnDisable()
    {
        if (_ws != null)
            _ws.Close();
    }
}
