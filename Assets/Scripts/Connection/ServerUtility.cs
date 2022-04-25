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
    [Header("Server utilities")]
    const int MAX_TABLETS = 10;
    public  Tablet[] _tablets;

    public  List<string> _ids = new List<string>();
    ServerPackage _serverPackage;
    public  List<ServerPackage> _allPackages;

    public static WebSocketServer _server;
    public static WebSocket _ws;

    [Header("Actions")]
    public int _connectedTablets;
    public bool _updateConnectedTablets;

    public bool _sendAddStudents;
    public bool fistTime = true;
    public int _numberTabletsEndCall = 0;
    public int _numberTabletsViewFinalScore = 0;
    /// <summary>Creates a websocket server</summary>
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

    /// <summary>Initialize the tablets with default info</summary>
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

    /// <summary>Get the device's IP</summary>
    private string GetLocalIPAddress()
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

    /// <summary>Receives a message and add it to the packages list</summary>
    private void Ws_OnMessage(object sender, MessageEventArgs e)
    {
        ServerPackage _serverHandlePackage = JsonConvert.DeserializeObject<ServerPackage>(e.Data);
        _allPackages.Add(_serverHandlePackage);
    }

    /// <summary>Close connections when the app is closed</summary>
    private void OnDisable()
    {
        if (_server != null)
            _server.Stop();
        if (_ws != null)
            _ws.Close();
    }

    /// <summary>Close connections when the teacher goes back from the connection screen</summary>
    public void ResetConnections()
    {
        if (_server != null)
            _server.Stop();
        if (_ws != null)
            _ws.Close();
    }

    /// <summary>Process the packages when the list is not empty and after that is removed.</summary>
    private void Update()
    {
        if (_allPackages != null &&_allPackages.Count > 0)
        {

            switch (_allPackages[0]._typePackageClient)
            {
                case ClientPackets.studentsEndCall:
                    _numberTabletsEndCall++;
                    ServerHandle.UpdateReadyRockets(_allPackages[0]._tabletInfo._idTablet);
                    if (_numberTabletsEndCall == _connectedTablets)
                    {
                        ServerHandle.ContinueGameTime();
                    }                    
                    break;
                case ClientPackets.selectedStudentGame:
                    ServerHandle.FindDificulty(_allPackages[0]);
                    break;
                case ClientPackets.matchData:
                    ServerHandle.MatchData(_allPackages[0]);
                    break;
                case ClientPackets.viewFinalScore:
                    _numberTabletsViewFinalScore++;
                    ServerHandle.UpdateTabletsViewingFinalScore(_numberTabletsViewFinalScore);
                    break;
            }
            _allPackages.Remove(_allPackages[0]);
        }
    }
    #region ServerSender
    /// <summary>Send the student list its specific tablet</summary>
    public void AddingStudents()
    {
        _serverPackage = new ServerPackage();

        _serverPackage._typePackageServer = ServerPackets.StudentSelection;

        for (int i = 0; i < _ids.Count; ++i)
        {
            Tablet specificTablet = ServiceLocator.Instance.GetService<NetworkManager>()._studentsToTablets[i];
            _serverPackage._toUser = _ids[i];
            _serverPackage._studentsInfo._studentsToTablets = specificTablet;

            _ws.Send(JsonConvert.SerializeObject(_serverPackage));
        }
    }

    /// <summary>Send the minigame time</summary>
    public void MinigameTime()
    {
        _serverPackage = new ServerPackage();

        _serverPackage._typePackageServer = ServerPackets.StartGame;
        _serverPackage._minigameTime._minutes = ServiceLocator.Instance.GetService<UIManager>()._timeMinigamesMinutes;
        _serverPackage._minigameTime._seconds = ServiceLocator.Instance.GetService<UIManager>()._timeMinigamesSeconds;

        _ws.Send(JsonConvert.SerializeObject(_serverPackage));
    }

    /// <summary>Send the specific minigame difficulty</summary>
    public void MinigameDifficulty(string toUser, int level)
    {
        _serverPackage = new ServerPackage();

        _serverPackage._typePackageServer = ServerPackets.GameDifficulty;
        _serverPackage._toUser = toUser;
        _serverPackage._gameDifficulty._level = level;

        _ws.Send(JsonConvert.SerializeObject(_serverPackage));
    }

    /// <summary>Finish the minigames</summary>
    public void FinishSession()
    {
        _serverPackage = new ServerPackage();

        _serverPackage._typePackageServer = ServerPackets.Quit;

        _ws.Send(JsonConvert.SerializeObject(_serverPackage));
    }

    /// <summary>Pause/unpause the session</summary>
    public void PauseSession(bool pause)
    {
        _serverPackage = new ServerPackage();

        _serverPackage._typePackageServer = ServerPackets.PauseGame;
        _serverPackage._pauseGame._pause = pause;

        _ws.Send(JsonConvert.SerializeObject(_serverPackage));
    }

    /// <summary>Send the action of disconnect the device</summary>
    public void TurnOff()
    {
        _serverPackage = new ServerPackage();

        _serverPackage._typePackageServer = ServerPackets.Disconnect;

        _ws.Send(JsonConvert.SerializeObject(_serverPackage));
    }
    #endregion
}
