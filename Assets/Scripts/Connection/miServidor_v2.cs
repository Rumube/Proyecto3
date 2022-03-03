using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using WebSocketSharp.Server;

public class miServidor_v2 : WebSocketBehavior
{
    const int MAX_PLAYERS = 5;

    public GameObject localPlayerPrefab;

    public static Player[] players;

    PackageUser package;
    Player localPlayer;

    bool firstTime = true;
    public struct PackageUser
    {
        public Message.typePackage typePackage;
        public int id;
        public Vector3 posicion;
        public string username;
         public bool isWinner;
    }

    // Start is called before the first frame update
    public void InitializeData()
    {
        //Inicializo la lista de jugadores
        players = new Player[MAX_PLAYERS];

        for (int i = 0; i < MAX_PLAYERS - 1; ++i)
        {
            Player nuevo = new Player();
            players[i] = nuevo;
            players[i]._id = -1;
        }
    }
    protected override void OnOpen()
    {
        base.OnOpen();
        Debug.Log("++ Alguien se ha conectado. "+Sessions.Count);
        sendStartedPackage();
    }

    protected override void OnClose(CloseEventArgs e)
    {
        base.OnClose(e);
        Debug.Log("-- Se ha desconectado alguien. "+Sessions.Count);
    }

    protected override void OnMessage(MessageEventArgs e)
    {
        base.OnMessage(e);
        Debug.Log("Mensaje recibido: " + e.Data);
        Sessions.Broadcast(e.Data); // reenviar el mensaje a todos

        package = JsonUtility.FromJson<PackageUser>(e.Data);

        switch (package.typePackage)
        {
            case Message.typePackage.NEW_PLAYER:
                Debug.Log("El player "+package.username+" se ha conectado");
                players[package.id]._username = package.username;
                players[package.id]._position = package.posicion;
                break;
            case Message.typePackage.UPDATE_POSITION:
                Debug.Log("Alguien actualiza su posicion");

                break;
            case Message.typePackage.DISCONNECT_PLAYER:
                break;
        }
    }
    public void sendStartedPackage()
    {
        Debug.Log("SendStarted");
        if (firstTime && players == null)
        {
            InitializeData();
            firstTime = false;
        }
        Debug.Log("Voy a enivar");
        localPlayer = new Player();
        for (int i = 0; i < MAX_PLAYERS; ++i)
        {
            if (players[i]._id == -1)
            {
                localPlayer._id = i;
                players[i] = localPlayer;
                break;
            }
        }
        PackageUser newPackage;
        newPackage.typePackage = Message.typePackage.LOCAL_PLAYER;
        newPackage.id = localPlayer._id;
        newPackage.posicion = new Vector3(0, 0, 0);
        newPackage.username = "";
        newPackage.isWinner = false;
        string packageJson = JsonUtility.ToJson(newPackage);

        Send(packageJson);
        Debug.Log("Envio paquete");
    }
}

