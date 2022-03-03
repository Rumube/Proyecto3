using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using WebSocketSharp;
using WebSocketSharp.Server;
public class Server : WebSocketBehavior
{
    const int MAX_PLAYERS = 10;

    public static Tablet[] _tablets;

    Package _serverPackage;
    List<Package> _allPackages;

    WebSocketServer _server;

    public void InitializeData()
    {
        _serverPackage = new Package();
        _tablets = new Tablet[MAX_PLAYERS];

        for (int i = 0; i < MAX_PLAYERS - 1; ++i)
        {
            Tablet nuevo = new Tablet();
            _tablets[i] = nuevo;
            _tablets[i]._id = -1;
        }
    }

    //  servidor websocket
    public void createServer()
    {
        _server = new WebSocketServer(8088);
        _server.Start();
        _server.AddWebSocketService<Server>("/");

       
        EDebug.Log("IP:" + GetLocalIPAddress() + " Port:" + _server.Port);

        EDebug.Log("Servidor iniciado.");

        _allPackages = new List<Package>();

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
        Debug.Log("++ Alguien se ha conectado. " + Sessions.Count);
    }
    protected override void OnClose(CloseEventArgs e)
    {
        base.OnClose(e);
        Debug.Log("-- Se ha desconectado alguien. " + Sessions.Count);
    }
    protected override void OnMessage(MessageEventArgs e)
    {
        base.OnMessage(e);
        Debug.Log("Mensaje recibido: " + e.Data);
    }
    private void OnDisable()
    {
        if (_server != null)
            _server.Stop();
    }
}
