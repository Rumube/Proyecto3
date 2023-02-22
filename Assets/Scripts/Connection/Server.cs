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
    const int MAX_TABLETS = 10; //Maybe change to 6

    ServerPackage _serverPackage;

    /// <summary>It's called when a client is connected. Assing the first information</summary>
    protected override void OnOpen()
    {
        base.OnOpen();
        //No more tablets can connect
        if (ServiceLocator.Instance.GetService<ServerUtility>()._connectedTablets == 6)
        {
            return;
        }
        if (!ServiceLocator.Instance.GetService<ServerUtility>().fistTime)
        {
            //EDebug.Log("++ Alguien se ha conectado. " + (Sessions.Count - 1));
            ServiceLocator.Instance.GetService<ServerUtility>()._connectedTablets = (Sessions.Count - 1);
            ServiceLocator.Instance.GetService<ServerUtility>()._updateConnectedTablets = true;
            
            ServiceLocator.Instance.GetService<ServerUtility>()._ids.Add(ID); 

            StartedPackage();

            //Hay que arreglar lo de darles una id unica porque si le doy la 1 y se desconecta alguien al siguiente se le da 1 tambien
            for (int i = 0; i < MAX_TABLETS - 1; ++i)
            {
                //Assign new tablet on empty slot
                if (ServiceLocator.Instance.GetService<ServerUtility>()._tablets[i]._id == -1)
                {
                    ServiceLocator.Instance.GetService<ServerUtility>()._tablets[i]._id = Sessions.Count-1; //Provisional
                    ServiceLocator.Instance.GetService<ServerUtility>()._tablets[i]._students = new List<Student>();
                    ServiceLocator.Instance.GetService<ServerUtility>()._tablets[i]._currentStudent = "";
                    ServiceLocator.Instance.GetService<ServerUtility>()._tablets[i]._currentGame = "";
                    ServiceLocator.Instance.GetService<ServerUtility>()._tablets[i]._score = -1;
                    return;
                }
            }
        }
        else
        {
            //Avoid adding server like a client
            ServiceLocator.Instance.GetService<ServerUtility>().fistTime = false;
        }
      
    }
    /// <summary>It's called when a client is disconnected. Get that client and rewrite the info for a default one</summary>
    protected override void OnClose(CloseEventArgs e)
    {
        base.OnClose(e);
        //EDebug.Log("-- Se ha desconectado alguien. " + (Sessions.Count-1));
        for (int i = 0; i < MAX_TABLETS - 1; ++i)
        {
            if (ServiceLocator.Instance.GetService<ServerUtility>()._tablets[i]._id == (Sessions.Count-1))
            {
                ServiceLocator.Instance.GetService<ServerUtility>()._tablets[i]._id = -1;
                ServiceLocator.Instance.GetService<ServerUtility>()._tablets[i]._students = new List<Student>();
                ServiceLocator.Instance.GetService<ServerUtility>()._tablets[i]._currentStudent = "";
                ServiceLocator.Instance.GetService<ServerUtility>()._tablets[i]._currentGame = "";
                ServiceLocator.Instance.GetService<ServerUtility>()._tablets[i]._score = -1;
            }
        }
        ServiceLocator.Instance.GetService<ServerUtility>()._connectedTablets = (Sessions.Count-1);
        ServiceLocator.Instance.GetService<ServerUtility>()._updateConnectedTablets = true;
        ServiceLocator.Instance.GetService<ServerUtility>()._ids.Remove(ID);

    }
    /// <summary>It's called when a message is received and the server do a broadcast for every client</summary>
    protected override void OnMessage(MessageEventArgs e)
    {
        base.OnMessage(e);
        Sessions.Broadcast(e.Data);
    }

    #region ServerSender

    /// <summary>Send the tablet's id</summary>
    public void StartedPackage()
    {
        _serverPackage = new ServerPackage();

        _serverPackage._toUser = ServiceLocator.Instance.GetService<ServerUtility>()._ids[(Sessions.Count - 2)];
        _serverPackage._typePackageServer = ServerPackets.IdTablet;
        _serverPackage._tabletInfo._idTablet = (Sessions.Count-1);

        Send(JsonConvert.SerializeObject(_serverPackage));
    }

    #endregion
}
