using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using WebSocketSharp;
using WebSocketSharp.Server;
public class Server : WebSocketBehavior
{
    const int MAX_TABLETS = 10;

    public static Tablet[] _tablets;
    public static int _connectedTablets;
    public static bool _updateConnectedTablets;
    Package _serverPackage;
    List<Package> _allPackages;

    WebSocketServer _server;

    public void InitializeData()
    {
        _serverPackage = new Package();
        _tablets = new Tablet[MAX_TABLETS];

        for (int i = 0; i < MAX_TABLETS - 1; ++i)
        {
            Tablet nuevo = new Tablet();
            _tablets[i] = nuevo;
            _tablets[i]._id = -1;
        }
    }

    //  servidor websocket
    public string[] createServer()
    {
        _connectedTablets = 0;
        _updateConnectedTablets = false;

        string[] connectionData = new string[2];
        _server = new WebSocketServer(8088);
        _server.Start();
        _server.AddWebSocketService<Server>("/");

       
        EDebug.Log("IP:" + GetLocalIPAddress() + " Port:" + _server.Port);

        EDebug.Log("Servidor iniciado.");

        _allPackages = new List<Package>();

        connectionData[0] = GetLocalIPAddress();
        connectionData[1] = _server.Port.ToString();

        InitializeData();

        return connectionData;
    }
    public string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        throw new System.Exception("No network adapters with an IPv4 address in the system!");
    }

    protected override void OnOpen()
    {
        base.OnOpen();
        EDebug.Log("++ Alguien se ha conectado. " + Sessions.Count);
        _connectedTablets = Sessions.Count;
        _updateConnectedTablets = true;

        SendStartedPackage();
    }
    protected override void OnClose(CloseEventArgs e)
    {
        base.OnClose(e);
        Debug.Log("-- Se ha desconectado alguien. " + Sessions.Count);
        _connectedTablets = Sessions.Count;
        _updateConnectedTablets = true;
    }
    protected override void OnMessage(MessageEventArgs e)
    {
        base.OnMessage(e);
        Debug.Log("Mensaje recibido: " + e.Data);
        
    }
    
    private void SendStartedPackage()
    {
        EDebug.Log("SendStarted");
        _serverPackage = new Package();
        _serverPackage._sendID._idTablet = Sessions.Count;
        string packageJson = JsonUtility.ToJson(_serverPackage);
        EDebug.Log("Envio paquete antes");
        Sessions.SendTo(packageJson, ID);
        EDebug.Log("Envio paquete");
    }
    private void OnDisable()
    {
        if (_server != null)
            _server.Stop();
    }
}
