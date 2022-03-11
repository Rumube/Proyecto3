using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

public class Client
{
    WebSocket _ws;
    Package _package;
    List<Package> _allPackages;
    public void CreateClient(string ip, string port)
    {
        string path = "ws://" + ip + ":" + port;
        _ws = new WebSocket(path);
        _ws.OnMessage += Ws_OnMessage;   // evento para recibir los mensajer
        _ws.Connect();
        EDebug.Log("me he conectado al servidor... listo para enviar y recibir");
        _allPackages = new List<Package>();

    }
    private void Ws_OnMessage(object sender, MessageEventArgs e)
    {
        _package = JsonUtility.FromJson<Package>(e.Data);
        EDebug.Log("Message received! " + sender.ToString() + " data: " +_package._sendID._idTablet);
    }
    private void OnDisable()
    {
        if (_ws != null)
            _ws.Close();
    }
}
