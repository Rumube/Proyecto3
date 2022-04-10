using ServerPack;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using WebSocketSharp;
using WebSocketSharp.Server;
using Newtonsoft.Json;
public class ServerUtility : MonoBehaviour
{
    const int MAX_TABLETS = 10;

    public  Tablet[] _tablets;
    public  int _connectedTablets;
    public  bool _updateConnectedTablets;
    public  List<string> _ids = new List<string>();


    ServerPackage _serverPackage;
    public  List<ServerPackage> _allPackages;

    public static WebSocketServer _server;

    public static WebSocket _ws;

    //Unity actions
    public bool _sendAddStudents;

    public bool fistTime = true;

    public int _numberTabletsEndCall = 0;
    //  servidor websocket
    public string[] createServer()
    {
        //Initialize data
        _connectedTablets = 0;
        _updateConnectedTablets = false;
        _allPackages = new List<ServerPackage>();
        _ids = new List<string>();
        string[] connectionData = new string[2];

        //Create server
        _server = new WebSocketServer(8088);
        _server.AddWebSocketService<Server>("/");
        _server.Start();

        //Create its own client
        _ws = new WebSocket("ws://localhost:8088");
        _ws.OnMessage += Ws_OnMessage;   
        _ws.Connect();

        EDebug.Log("Servidor iniciado.");

        //Get ip and port
        connectionData[0] = GetLocalIPAddress();
        connectionData[1] = _server.Port.ToString();

        InitializeData();

        return connectionData;
    }

    public void InitializeData()
    {
        _tablets = new Tablet[MAX_TABLETS];

        for (int i = 0; i < MAX_TABLETS - 1; ++i)
        {
            Tablet nuevo = new Tablet();
            _tablets[i] = nuevo;
            _tablets[i]._id = -1;
        }
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
    private void Ws_OnMessage(object sender, MessageEventArgs e)
    {
        Debug.Log("OnMessagePruebaCliente: " + e.Data);
        ServerPackage _serverHandlePackage = JsonConvert.DeserializeObject<ServerPackage>(e.Data);
        _allPackages.Add(_serverHandlePackage);
    }

    private void OnDisable()
    {
        if (_server != null)
            _server.Stop();
        if (_ws != null)
            _ws.Close();
    }
    public void ResetConnections()
    {
        if (_server != null)
            _server.Stop();
        if (_ws != null)
            _ws.Close();
    }

    private void Update()
    {
        if (_allPackages != null &&_allPackages.Count > 0)
        {

            switch (_allPackages[0]._typePackageClient)
            {
                case ClientPackets.studentsEndCall:
                    _numberTabletsEndCall++;
                    if(_numberTabletsEndCall == _connectedTablets)
                    {
                        ServerHandle.ContinueGameTime();
                    }                    
                    break;
                case ClientPackets.selectedStudentGame:
                    ServerHandle.FindDificulty(_allPackages[0]);
                    break;
                case ClientPackets.matchData:

                    break;
            }
            _allPackages.Remove(_allPackages[0]);
        }
    }
# region ServerSender
    public void AddingStudents()
    {
        _serverPackage = new ServerPackage();
        _serverPackage._typePackageServer = ServerPackets.StudentSelection;

        for (int i = 0; i < _ids.Count; ++i)
        {
            Tablet specificTablet = ServiceLocator.Instance.GetService<NetworkManager>()._studentsToTablets[i];
            _serverPackage._toUser = _ids[i];
            _serverPackage._studentsInfo._studentsToTablets = specificTablet;
            string packageJson = JsonConvert.SerializeObject(_serverPackage);
            //Sessions.SendTo(packageJson, _ids[i]);
            EDebug.Log("Enviando ninos");
            _ws.Send(packageJson);
            EDebug.Log("Enviados");
        }
    }
    public void MinigameTime()
    {
        _serverPackage = new ServerPackage();
        _serverPackage._typePackageServer = ServerPackets.StartGame;
        _serverPackage._minigameTime._minutes = ServiceLocator.Instance.GetService<UIManager>()._timeMinigamesMinutes;
        _serverPackage._minigameTime._seconds = ServiceLocator.Instance.GetService<UIManager>()._timeMinigamesSeconds;
        string packageJson = JsonConvert.SerializeObject(_serverPackage);
        _ws.Send(packageJson);
    }
    public void MinigameDifficulty(string toUser, int level)
    {
        _serverPackage = new ServerPackage();
        _serverPackage._typePackageServer = ServerPackets.GameDifficulty;
        _serverPackage._toUser = toUser;
        _serverPackage._gameDifficulty._level = level;
        string packageJson = JsonConvert.SerializeObject(_serverPackage);
        _ws.Send(packageJson);
    }
    public void FinishSession()
    {
        _serverPackage = new ServerPackage();
        _serverPackage._typePackageServer = ServerPackets.Quit;
        string packageJson = JsonConvert.SerializeObject(_serverPackage);
        _ws.Send(packageJson);
    }

    public void PauseSession(bool pause)
    {
        _serverPackage = new ServerPackage();
        _serverPackage._typePackageServer = ServerPackets.PauseGame;
        _serverPackage._pauseGame._pause = pause;
        string packageJson = JsonConvert.SerializeObject(_serverPackage);
        _ws.Send(packageJson);
    }
    #endregion
}
