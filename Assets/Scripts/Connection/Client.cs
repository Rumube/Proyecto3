using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using Newtonsoft.Json;
using ClientPack;

public class Client
{
    public static string _id;
    public static Tablet _tablet;
    
    public static WebSocket _ws;
    public static List<ClientPackage> _allPackages;

    /// <summary>Create a new client with web socket, connect to a specific server and initialize the list of packages</summary>
    /// <param name="ip">Server IP</param>
    /// <param name="port">Server port</param>
    public void CreateClient(string ip, string port)
    {
        _ws = new WebSocket("ws://" + ip + ":" + port);
        _ws.OnMessage += Ws_OnMessage;
        _ws.Connect();

        if (_ws.Ping())
        {
            EDebug.Log("me he conectado al servidor... listo para enviar y recibir");
            ClientHandle.CanConnectServer();

            _allPackages = new List<ClientPackage>();

            InitializeData();
        }
        else
        {
            ClientHandle.CantConnectServer();
            if (_ws != null)
                _ws.Close();
        }     
    }

    /// <summary>Initialize a tablet and assing -1 for tablet id</summary>
    public void InitializeData()
    {
        _tablet = new Tablet();
        _tablet._id = -1;
    }

    /// <summary>Receives a package from server</summary>
    private void Ws_OnMessage(object sender, MessageEventArgs e)
    {
        ClientPackage _clientPackage = JsonConvert.DeserializeObject<ClientPackage>(e.Data);
        _allPackages.Add(_clientPackage);
    }

    /// <summary>It's called when the list of packages has at least one package. Process it deppending on the type of package and finally its removed</summary>
    public static void DoUpdate()
    {
        switch (_allPackages[0]._typePackageServer)
        {
            case ServerPackets.IdTablet:
                ClientHandle.AssignID(_allPackages[0]);
                break;
            case ServerPackets.StudentSelection:
                if (_allPackages[0]._toUser == _id)
                {
                    ClientHandle.AssignStudents(_allPackages[0]);
                }
                break;
            case ServerPackets.StartGame:
                ClientHandle.StartGame(_allPackages[0]); 
                break;
            case ServerPackets.GameDifficulty: //TO FINISH
                if (_allPackages[0]._toUser == _id)
                {
                    ClientHandle.SpecificGameDifficulty(_allPackages[0]);
                }
                break;
            case ServerPackets.UpdateTeamPoints: //TODO

                break;
            case ServerPackets.PauseGame:
                ClientHandle.PauseGame(_allPackages[0]);
                break;
            case ServerPackets.Quit:
                ClientHandle.QuitGame();
                break;
            case ServerPackets.Disconnect:
                ClientHandle.TurnOff();
                break;
        }
        _allPackages.Remove(_allPackages[0]);
    }

    #region Client sender
    /// <summary>Send a package when all students has been called in connection screen</summary>
    public void EndCallingStudents()
    {
        ClientPackage package = new ClientPackage();

        package._typePackageClient = ClientPackets.studentsEndCall;
        package._tabletInfo._idTablet = _tablet._id;
        package._callingDone._isDone = true;

        _ws.Send(JsonConvert.SerializeObject(package));
    }

    /// <summary>Send a package when a student and game has been selected</summary>
    public void StudentGameSelection(string studentName, string gameName)
    {
        ClientPackage package = new ClientPackage();

        package._typePackageClient = ClientPackets.selectedStudentGame;
        package._fromUser = _id;
        package._selectStudentGame._studentName = studentName;
        package._selectStudentGame._gameName = gameName;

        _ws.Send(JsonConvert.SerializeObject(package));
    }

    public void ViewFinalScore()
    {
        ClientPackage package = new ClientPackage();

        package._typePackageClient = ClientPackets.viewFinalScore;

        _ws.Send(JsonConvert.SerializeObject(package));
    }

    public void StudentScore()
    {
        ClientPackage package = new ClientPackage();

        package._typePackageClient = ClientPackets.matchData;
        package._matchData._studentName = ServiceLocator.Instance.GetService<IGameManager>().GetCurrentGameName();
        package._matchData._gameName = ServiceLocator.Instance.GetService<IGameManager>().GetCurrentGameName();
        package._matchData._team = _tablet._id;
        package._matchData._averageSuccess = (int)ServiceLocator.Instance.GetService<ICalculatePoints>().GetAverage().averageSuccess;
        package._matchData._averageErrors = (int)ServiceLocator.Instance.GetService<ICalculatePoints>().GetAverage().averageFails;
        package._matchData._averageGameTime = ServiceLocator.Instance.GetService<ICalculatePoints>().GetAverage().averageTime;
        package._matchData._averagePoints = (int)ServiceLocator.Instance.GetService<ICalculatePoints>().GetAverage().averagePoints;
        package._matchData._gameLevel = ServiceLocator.Instance.GetService<NetworkManager>()._minigameLevel;
        _ws.Send(JsonConvert.SerializeObject(package));
    }
    #endregion

    /// <summary>Close the web socket</summary>
    public static void OnDisable()
    {
        if (_ws != null)
            _ws.Close();
    }
}
