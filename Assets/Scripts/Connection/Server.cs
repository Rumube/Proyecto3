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

    //public static Tablet[] _tablets;
    //public static int _connectedTablets;
    //public static bool _updateConnectedTablets;
    //public static List<string> _ids;
    //public static List<ServerPackage> _allPackages;
    ServerPackage _serverPackage;
    

    //private bool _firstTime = false;

    //public static WebSocketServer _server;

    //public static WebSocket _ws;

    ////Unity actions
    //public static bool _sendAddStudents;

    ////  servidor websocket
    //public string[] createServer()
    //{
    //    _connectedTablets = 0;
    //    _updateConnectedTablets = false;

    //    string[] connectionData = new string[2];
    //    _server = new WebSocketServer(8088);      
    //    _server.AddWebSocketService<Server>("/");
    //    _server.Start();

    //    _ws = new WebSocket("ws://localhost:8088");
    //    //_ws.OnMessage += Ws_OnMessage;   // evento para recibir los mensajer
    //    _ws.Connect();

    //    EDebug.Log("IP:" + GetLocalIPAddress() + " Port:" + _server.Port);
    //    EDebug.Log("Listening: " + _server.IsListening);
    //    EDebug.Log("Servidor iniciado.");

    //    _allPackages = new List<ServerPackage>();

    //    connectionData[0] = GetLocalIPAddress();
    //    connectionData[1] = _server.Port.ToString();

    //    InitializeData();

    //    return connectionData;
    //}

    protected override void OnOpen()
    {
        base.OnOpen();
             
        if (!ServiceLocator.Instance.GetService<Prueba>().fistTime)
        {
            EDebug.Log("++ Alguien se ha conectado. " + (Sessions.Count - 1));
            ServiceLocator.Instance.GetService<Prueba>()._connectedTablets = (Sessions.Count - 1);
            ServiceLocator.Instance.GetService<Prueba>()._updateConnectedTablets = true;
            

            ServiceLocator.Instance.GetService<Prueba>()._ids.Add(ID); //In the future could not work, because I saw that ID could change within the same session
            EDebug.Log("ids OPEN: " + ServiceLocator.Instance.GetService<Prueba>()._ids[Sessions.Count - 2]);

            StartedPackage();

            //Hay que arreglar lo de darles una id unica porque si le doy la 1 y se desconecta alguien al siguiente se le da 1 tambien
            for (int i = 0; i < MAX_TABLETS - 1; ++i)
            {
                //Assign new tablet on empty slot
                if (ServiceLocator.Instance.GetService<Prueba>()._tablets[i]._id == -1)
                {
                    ServiceLocator.Instance.GetService<Prueba>()._tablets[i]._id = Sessions.Count-1; //Provisional
                    ServiceLocator.Instance.GetService<Prueba>()._tablets[i]._students = new List<Student>();
                    ServiceLocator.Instance.GetService<Prueba>()._tablets[i]._currentStudent = -1;
                    ServiceLocator.Instance.GetService<Prueba>()._tablets[i]._currentGame = -1;
                    ServiceLocator.Instance.GetService<Prueba>()._tablets[i]._score = -1;
                    return;
                }
            }
        }
        else
        {
            Debug.Log("First time");
            ServiceLocator.Instance.GetService<Prueba>().fistTime = false;
        }
      
    }
    protected override void OnClose(CloseEventArgs e)
    {
        base.OnClose(e);
        Debug.Log("-- Se ha desconectado alguien. " + (Sessions.Count-1));
        EDebug.Log("ids CLOSE: " + ServiceLocator.Instance.GetService<Prueba>()._ids);
        for (int i = 0; i < MAX_TABLETS - 1; ++i)
        {
            if (ServiceLocator.Instance.GetService<Prueba>()._tablets[i]._id == (Sessions.Count-1))
            {
                ServiceLocator.Instance.GetService<Prueba>()._tablets[i]._id = -1;
                ServiceLocator.Instance.GetService<Prueba>()._tablets[i]._students = new List<Student>();
                ServiceLocator.Instance.GetService<Prueba>()._tablets[i]._currentStudent = -1;
                ServiceLocator.Instance.GetService<Prueba>()._tablets[i]._currentGame = -1;
                ServiceLocator.Instance.GetService<Prueba>()._tablets[i]._score = -1;
            }
        }
        ServiceLocator.Instance.GetService<Prueba>()._connectedTablets = (Sessions.Count-1);
        ServiceLocator.Instance.GetService<Prueba>()._updateConnectedTablets = true;
        ServiceLocator.Instance.GetService<Prueba>()._ids.Remove(ID);

    }
    protected override void OnMessage(MessageEventArgs e)
    {
        base.OnMessage(e);
        Debug.Log("OnMessageBroadcast: " + e.Data);
        Sessions.Broadcast(e.Data);
    }
    //    private void Ws_OnMessage(object sender, MessageEventArgs e)
    //{
    //    base.OnMessage(e);
    //    Debug.Log("ey: " + e.Data);
    //    ServerPackage _serverHandlePackage = JsonConvert.DeserializeObject<ServerPackage>(e.Data);
    //    _allPackages.Add(_serverHandlePackage);
    //}

    public void DoUpdate()
    {
        //if (ServiceLocator.Instance.GetService<Prueba>()._allPackages != null && ServiceLocator.Instance.GetService<Prueba>()._allPackages.Count > 0)
        //{

        //    switch (ServiceLocator.Instance.GetService<Prueba>()._allPackages[0]._typePackageClient)
        //    {
        //        case ClientPackets.studentsEndCall:
        //            ServerHandle.ContinueGameTime();
        //            break;
        //        case ClientPackets.selectedStudentGame:

        //            break;
        //        case ClientPackets.matchData:

        //            break;
        //    }
        //    ServiceLocator.Instance.GetService<Prueba>()._allPackages.Remove(ServiceLocator.Instance.GetService<Prueba>()._allPackages[0]);
        //}
        //if (ServiceLocator.Instance.GetService<Prueba>()._sendAddStudents)
        //{
        //    ServiceLocator.Instance.GetService<Prueba>()._sendAddStudents = false;
        //    AddingStudents();
        //}
    }

    #region ServerSender
    public void StartedPackage()
    {
        EDebug.Log("SendStarted");
        _serverPackage = new ServerPackage();
        _serverPackage._toUser = ServiceLocator.Instance.GetService<Prueba>()._ids[(Sessions.Count - 2)];
        _serverPackage._typePackageServer = ServerPackets.IdTablet;
        _serverPackage._tabletInfo._idTablet = (Sessions.Count-1);
        string packageJson = JsonConvert.SerializeObject(_serverPackage);
        EDebug.Log("Envio paquete antes:" + (Sessions.Count - 1));
        //Sessions.SendTo(packageJson,_ids[Sessions.Count - 1]);
        Send(packageJson);
        EDebug.Log("Envio paquete");
    }

    public void AddingStudents()
    {
        //EDebug.Log("Listening: " + _server.IsListening);
        //_serverPackage = new ServerPackage();
        //_serverPackage._info._nameStudent = "Hola";
        //string packageJson = JsonConvert.SerializeObject(_serverPackage);
        //Debug.Log("Hola");
        //Sessions.Broadcast("probando");
        //_serverPackage._typePackageServer = ServerPackets.StudentSelection;

        //for (int i = 0; i < _ids.Count; ++i)
        //{
        //    Tablet specificTablet = ServiceLocator.Instance.GetService<NetworkManager>()._studentsToTablets[i];
        //    _serverPackage._studentsInfo._studentsToTablets = specificTablet;
        //    string packageJson = JsonConvert.SerializeObject(_serverPackage);
        //    //Sessions.SendTo(packageJson, _ids[i]);
        //    EDebug.Log("Enviando ninos");
        //    Send(packageJson);
        //    EDebug.Log("Enviados");
        //}
    }
    #endregion
}
