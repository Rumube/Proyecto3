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

    ServerPackage _serverPackage;
    
    protected override void OnOpen()
    {
        base.OnOpen();
             
        if (!ServiceLocator.Instance.GetService<ServerUtility>().fistTime)
        {
            EDebug.Log("++ Alguien se ha conectado. " + (Sessions.Count - 1));
            ServiceLocator.Instance.GetService<ServerUtility>()._connectedTablets = (Sessions.Count - 1);
            ServiceLocator.Instance.GetService<ServerUtility>()._updateConnectedTablets = true;
            

            ServiceLocator.Instance.GetService<ServerUtility>()._ids.Add(ID); //In the future could not work, because I saw that ID could change within the same session
            EDebug.Log("ids OPEN: " + ServiceLocator.Instance.GetService<ServerUtility>()._ids[Sessions.Count - 2]);

            StartedPackage();

            //Hay que arreglar lo de darles una id unica porque si le doy la 1 y se desconecta alguien al siguiente se le da 1 tambien
            for (int i = 0; i < MAX_TABLETS - 1; ++i)
            {
                //Assign new tablet on empty slot
                if (ServiceLocator.Instance.GetService<ServerUtility>()._tablets[i]._id == -1)
                {
                    ServiceLocator.Instance.GetService<ServerUtility>()._tablets[i]._id = Sessions.Count-1; //Provisional
                    ServiceLocator.Instance.GetService<ServerUtility>()._tablets[i]._students = new List<Student>();
                    ServiceLocator.Instance.GetService<ServerUtility>()._tablets[i]._currentStudent = -1;
                    ServiceLocator.Instance.GetService<ServerUtility>()._tablets[i]._currentGame = -1;
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
    protected override void OnClose(CloseEventArgs e)
    {
        base.OnClose(e);
        Debug.Log("-- Se ha desconectado alguien. " + (Sessions.Count-1));
        EDebug.Log("ids CLOSE: " + ServiceLocator.Instance.GetService<ServerUtility>()._ids);
        for (int i = 0; i < MAX_TABLETS - 1; ++i)
        {
            if (ServiceLocator.Instance.GetService<ServerUtility>()._tablets[i]._id == (Sessions.Count-1))
            {
                ServiceLocator.Instance.GetService<ServerUtility>()._tablets[i]._id = -1;
                ServiceLocator.Instance.GetService<ServerUtility>()._tablets[i]._students = new List<Student>();
                ServiceLocator.Instance.GetService<ServerUtility>()._tablets[i]._currentStudent = -1;
                ServiceLocator.Instance.GetService<ServerUtility>()._tablets[i]._currentGame = -1;
                ServiceLocator.Instance.GetService<ServerUtility>()._tablets[i]._score = -1;
            }
        }
        ServiceLocator.Instance.GetService<ServerUtility>()._connectedTablets = (Sessions.Count-1);
        ServiceLocator.Instance.GetService<ServerUtility>()._updateConnectedTablets = true;
        ServiceLocator.Instance.GetService<ServerUtility>()._ids.Remove(ID);

    }
    protected override void OnMessage(MessageEventArgs e)
    {
        base.OnMessage(e);
        Debug.Log("OnMessageBroadcast: " + e.Data);
        Sessions.Broadcast(e.Data);
    }
    
    #region ServerSender
    public void StartedPackage()
    {
        EDebug.Log("SendStarted");
        _serverPackage = new ServerPackage();
        _serverPackage._toUser = ServiceLocator.Instance.GetService<ServerUtility>()._ids[(Sessions.Count - 2)];
        _serverPackage._typePackageServer = ServerPackets.IdTablet;
        _serverPackage._tabletInfo._idTablet = (Sessions.Count-1);
        string packageJson = JsonConvert.SerializeObject(_serverPackage);
        EDebug.Log("Envio paquete antes:" + (Sessions.Count - 1));
        //Sessions.SendTo(packageJson,_ids[Sessions.Count - 1]);
        Send(packageJson);
        EDebug.Log("Envio paquete");
    }

    #endregion
}
