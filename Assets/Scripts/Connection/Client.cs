using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using Newtonsoft.Json;
using ClientPack;
public class Client
{
    public static Tablet _tablet;
    public static string _id;
    public static WebSocket _ws;
    ClientPackage _clientPackage;
    public static List<ClientPackage> _allPackages;

    public void InitializeData()
    {
        _clientPackage = new ClientPackage();

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
        _allPackages = new List<ClientPackage>();

        InitializeData();

    }
    private void Ws_OnMessage(object sender, MessageEventArgs e)
    {
        ClientPackage _clientPackage = JsonConvert.DeserializeObject<ClientPackage>(e.Data);
        _allPackages.Add(_clientPackage);
    }

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
                ServiceLocator.Instance.GetService<GameManager>().RandomizeStudentsList();
                ServiceLocator.Instance.GetService<TabletUI>().OpenNextWindow();
                ServiceLocator.Instance.GetService<TabletUI>().NewStudentGame();
                break;
            case ServerPackets.GameDifficulty:
                ClientHandle.SpecificGameDifficulty(_allPackages[0]);
                break;
            case ServerPackets.UpdateTeamPoints:

                break;
            case ServerPackets.PauseGame:
                ClientHandle.PauseGame(_allPackages[0]);
                break;
            case ServerPackets.Quit:
                ClientHandle.QuitGame();
                break;
            case ServerPackets.Disconnect:

                break;
        }
        _allPackages.Remove(_allPackages[0]);
    }

    public void Hola()
    {
        _ws.Send("Hola");
    }
    #region Client sender
    public void EndCallingStudents()
    {
        ClientPackage package = new ClientPackage();
        package._typePackageClient = ClientPackets.studentsEndCall;
        package._callingDone._isDone = true;
        string packageJson = JsonConvert.SerializeObject(package);
        _ws.Send(packageJson);
    }

    public void StudentGameSelection(string studentName, string gameName)
    {
        ClientPackage package = new ClientPackage();
        package._typePackageClient = ClientPackets.selectedStudentGame;
        package._selectStudentGame._studentName = studentName;
        package._selectStudentGame._gameName = gameName;
        string packageJson = JsonConvert.SerializeObject(package);
        _ws.Send(packageJson);
    }
    #endregion
    public static void OnDisable()
    {
        if (_ws != null)
            _ws.Close();
    }
}
