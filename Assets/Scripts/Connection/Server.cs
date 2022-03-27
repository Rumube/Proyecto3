using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using WebSocketSharp;
using WebSocketSharp.Server;
using Newtonsoft.Json;
using ServerPack;
public class Server : WebSocketBehavior
{
    const int MAX_TABLETS = 10;

    public static Tablet[] _tablets;
    public static int _connectedTablets;
    public static bool _updateConnectedTablets;
    public static List<string> _ids = new List<string>();

    ServerPackage _serverPackage;
    public static List<ServerPackage> _allPackages;

    static WebSocketServer _server;

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

        _allPackages = new List<ServerPackage>();

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

        

        EDebug.Log("Initialized packets.");
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

        StartedPackage();

        _ids.Add(ID); //In the future could not work, because I saw that ID could change within the same session
        EDebug.Log("ids OPEN: " + _ids[Sessions.Count - 1]);
        //Hay que arreglar lo de darles una id unica porque si le doy la 1 y se desconecta alguien al siguiente se le da 1 tambien
        for (int i = 0; i < MAX_TABLETS - 1; ++i)
        {
            //Assign new tablet on empty slot
            if (_tablets[i]._id == -1)
            {
                _tablets[i]._id = Sessions.Count; //Provisional
                _tablets[i]._students = new List<Student>();
                _tablets[i]._currentStudent = -1;
                _tablets[i]._currentGame = -1;
                _tablets[i]._score = -1;
                return;
            }
        }
    }
    protected override void OnClose(CloseEventArgs e)
    {
        base.OnClose(e);
        Debug.Log("-- Se ha desconectado alguien. " + Sessions.Count);
        EDebug.Log("ids CLOSE: " + _ids);
        for (int i = 0; i < MAX_TABLETS - 1; ++i)
        {
            if (_tablets[i]._id == Sessions.Count)
            {
                _tablets[i]._id = -1;
                _tablets[i]._students = new List<Student>();
                _tablets[i]._currentStudent = -1;
                _tablets[i]._currentGame = -1;
                _tablets[i]._score = -1;
            }
        }
        _connectedTablets = Sessions.Count;
        _updateConnectedTablets = true;
        _ids.Remove(ID);

    }
    protected override void OnMessage(MessageEventArgs e)
    {
        base.OnMessage(e);
        Debug.Log("Mensaje recibido: " + e.Data);
        ServerPackage _serverHandlePackage = JsonConvert.DeserializeObject<ServerPackage>(e.Data);
        _allPackages.Add(_serverHandlePackage);
    }

    public static void DoUpdate()
    {
        switch (_allPackages[0]._typePackageClient)
        {
            case ClientPackets.studentsEndCall:
                ServerHandle.ContinueGameTime();
                break;
            case ClientPackets.selectedStudentGame:

                break;
            case ClientPackets.matchData:

                break;
        }


        _allPackages.Remove(_allPackages[0]);
    }

    public static void OnDisable()
    {
        if (_server != null)
            _server.Stop();
    }

    #region ServerSender
    public void StartedPackage()
    {
        EDebug.Log("SendStarted");
        _serverPackage = new ServerPackage();
        _serverPackage._typePackageServer = ServerPackets.IdTablet;
        _serverPackage._tabletInfo._idTablet = Sessions.Count;
        //string packageJson = JsonUtility.ToJson(_serverPackage._info);
        string packageJson = JsonConvert.SerializeObject(_serverPackage);
        EDebug.Log("Envio paquete antes:" + Sessions.Count);
        //Sessions.SendTo(packageJson,_ids[Sessions.Count - 1]);
        Send(packageJson);
        EDebug.Log("Envio paquete");
    }

    public void AddingStudents()
    {
        _serverPackage = new ServerPackage();
        _serverPackage._typePackageServer = ServerPackets.StudentSelection;

        for (int i = 0; i < _ids.Count; ++i)
        {
            Tablet specificTablet = ServiceLocator.Instance.GetService<NetworkManager>()._studentsToTablets[i];
            _serverPackage._studentsInfo._studentsToTablets = specificTablet;
            string packageJson = JsonConvert.SerializeObject(_serverPackage);
            Sessions.SendTo(packageJson, _ids[i]);
        }
    }
    #endregion
}
