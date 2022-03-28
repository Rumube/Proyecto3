using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ClientPack;
public class ClientHandle
{
    public static void AssignID(ClientPackage package){
        Client._id = package._toUser;
        Client._tablet._id = package._tabletInfo._idTablet;
        EDebug.Log(Client._tablet._id);
    }
}
