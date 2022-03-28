using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using Newtonsoft.Json;
using ClientPack;
public class Client
{
    public static Tablet _tablet;

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
        Debug.Log("OnMessageClient " +e.Data);
        ClientPackage _clientPackage = JsonConvert.DeserializeObject<ClientPackage>(e.Data);
        _allPackages.Add(_clientPackage);
    }

    public static void DoUpdate()
    {
        EDebug.Log(_ws.IsAlive);
        switch (_allPackages[0]._typePackageServer)
        {
            case ServerPackets.IdTablet:
                ClientHandle.AssignID(_allPackages[0]._tabletInfo);
                break;
            case ServerPackets.StudentSelection:
                EDebug.Log("He llegado");
                break;
            case ServerPackets.StartGame:

                break;
            case ServerPackets.GameDifficulty:

                break;
            case ServerPackets.UpdateTeamPoints:

                break;
            case ServerPackets.PauseGame:

                break;
            case ServerPackets.Quit:

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
    public static void OnDisable()
    {
        if (_ws != null)
            _ws.Close();
    }
}
