using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ClientPack;
public class ClientHandle
{
    public static void AssignID(ClientPackage.TabletInfo info){
        Client._tablet._id = info._idTablet;
        EDebug.Log(Client._tablet._id);
    }
}
