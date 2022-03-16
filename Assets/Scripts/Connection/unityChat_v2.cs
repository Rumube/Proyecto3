using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;
using WebSocketSharp.Server;

public class unityChat_v2 : MonoBehaviour
{
    WebSocket ws;
    WebSocketServer server;

    PackageUser package;
    List<PackageUser> allPackages;

    public bool _newPackageReady = false;

    public Player localPlayer;

    public GameObject local;
    public TextMeshProUGUI localUsername;

    public GameObject multiPlayer;
    public TextMeshProUGUI multiplayerUsername;

    public Text ipPort;
    public Text inputIP;
    public Text inputPort;
    public Text inputUsername;
    public GameObject ipPortGO;

    public GameObject initialCanvas;
    public GameObject finishCanvas;
    public GameObject waitingCanvas;

    public Text winner;

    string winnerName;

    public enum GameState
    {
        Initiating,
        WaitingPlayers,
        StartGame,
        FinishGame
    }
    public GameState gameState;

    public struct PackageUser
    {
        public Message.typePackage typePackage;
        public int id;
        public Vector3 posicion;
        public string username;

        public bool isWinner;
    }

    private void Start()
    {
        gameState = GameState.Initiating;
       // local.GetComponent<PlayerController>().stopPlaying = true;
    }

    //  servidor websocket
    public void crearServidor()
    {
        server = new WebSocketServer(8088);
        server.Start();
        server.AddWebSocketService<miServidor_v2>("/");

        ipPortGO.SetActive(true);
        ipPort.text = "IP:" + GetLocalIPAddress() + " Port:" + server.Port;

        Debug.Log("Servidor iniciado.");

        ws = new WebSocket("ws://localhost:8088");
        ws.OnMessage += Ws_OnMessage;   // evento para recibir los mensajer
        ws.Connect();

        Debug.Log("me he conectado al servidor... listo para enviar y recibir");

        initialCanvas.SetActive(false);

        allPackages = new List<PackageUser>();

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

    //  cliente
    public void conectarAlServidor()
    {
        string path = "ws://" + inputIP.text + ":" + inputPort.text;
        ws = new WebSocket(path);
        ws.OnMessage += Ws_OnMessage;   // evento para recibir los mensajer
        ws.Connect();

        Debug.Log("me he conectado al servidor... listo para enviar y recibir");

        initialCanvas.SetActive(false);

        allPackages = new List<PackageUser>();

    }
   
    private void Ws_OnMessage(object sender, MessageEventArgs e)
    {
        package = JsonUtility.FromJson<PackageUser>(e.Data);

        _newPackageReady = true;

        if (localPlayer._id != package.id && package.typePackage == Message.typePackage.LOCAL_PLAYER)
        {
            allPackages.Add(package);
            print("tambien llego aqui " + allPackages[0].username);
        }
            if (allPackages != null && allPackages.Count == 1 && localPlayer._id != package.id && package.typePackage == Message.typePackage.NEW_PLAYER)
        {
            allPackages.Add(package);
            print("llego aqui " +allPackages[1].username);
        }
    }

    // Update is called once per frame
    void Update()
    {
       
        if (_newPackageReady)
        {
            switch (gameState)
            {
                case GameState.Initiating:

                   // PlayerController.winner = false;

                    if (localPlayer._id != allPackages[0].id && allPackages[0].typePackage == Message.typePackage.LOCAL_PLAYER)
                    {
                        localPlayer._id = allPackages[0].id;
                        localPlayer._position = allPackages[0].posicion;
                        localPlayer._username = inputUsername.text;
                        localUsername.text = localPlayer._username;

                        PackageUser newPackage;
                        newPackage.id = localPlayer._id;
                        newPackage.posicion = localPlayer._position;
                        newPackage.typePackage = Message.typePackage.NEW_PLAYER;
                        newPackage.username = localPlayer._username;
                        newPackage.isWinner = false;
                        string packageJson = JsonUtility.ToJson(newPackage);

                        ws.Send(packageJson);

                        gameState = GameState.WaitingPlayers;
                    }
                    break;
                case GameState.WaitingPlayers:
                   waitingCanvas.SetActive(true);
                   //Envia su paquete de info mientras espera por el resto de jugadores
                    PackageUser newPackage2;
                    newPackage2.id = localPlayer._id;
                    newPackage2.posicion = localPlayer._position;
                    newPackage2.typePackage = Message.typePackage.NEW_PLAYER;
                    newPackage2.username = localPlayer._username;
                    newPackage2.isWinner = false;
                    string packageJson2 = JsonUtility.ToJson(newPackage2);

                    ws.Send(packageJson2);

                    if (allPackages.Count == 2 && localPlayer._id != allPackages[1].id && allPackages[1].typePackage == Message.typePackage.NEW_PLAYER)
                    {
                        multiplayerUsername.text = allPackages[1].username;

                        gameState = GameState.StartGame;

                        waitingCanvas.SetActive(false);
                    }
                    break;
                case GameState.StartGame:
                   
                    //local.GetComponent<PlayerController>().stopPlaying = false;
                    print("Starting");
                    //Send local position                            
                    if (localPlayer._id != -1 && ws != null)
                    {
                        PackageUser newPackage;
                        newPackage.id = localPlayer._id;
                        newPackage.posicion = local.transform.position;
                        newPackage.username = localPlayer._username;
                        newPackage.typePackage = Message.typePackage.UPDATE_POSITION;
                       // newPackage.isWinner = PlayerController.winner;
                        //string packageJson = JsonUtility.ToJson(newPackage);
                        //ws.Send(packageJson);
                    }
                    switch (package.typePackage)
                    {

                        case Message.typePackage.UPDATE_POSITION:
                            if(package.isWinner){
                                print("Alguien ha ganado");
                                winnerName = package.username;

                                gameState = GameState.FinishGame;
                            }
                            //Update multiplayer position
                            if (package.id != localPlayer._id)
                            {
                                multiPlayer.transform.position = package.posicion;
                            }
                            break;
                        case Message.typePackage.DISCONNECT_PLAYER:
                            break;
                    }
                    break;
                case GameState.FinishGame:
                   // local.GetComponent<PlayerController>().stopPlaying = true;
                    finishCanvas.SetActive(true);
                    winner.text = "El ganador es: " + winnerName;
                    allPackages.Clear();
                    break;
            }         
            _newPackageReady = false;
        }
    }

    private void OnDisable()
    {
        if (server != null)
            server.Stop();
        if(ws != null)
            ws.Close();
    }
}
