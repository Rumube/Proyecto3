using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ServerPack;
public class ServerHandle { 

    public static void ContinueGameTime()
    {
        ServiceLocator.Instance.GetService<MobileUI>()._adviceStudents.text = "¡Todos los mininautas están preparados!";
        ServiceLocator.Instance.GetService<MobileUI>()._continueGameTimer.interactable = true;
    }

    public static void FindDificulty(ServerPackage _package)
    {
        Debug.Log("Entra paquete nino minijuego: " +_package._selectStudentGame._studentName + " g " + _package._selectStudentGame._gameName);
    }
}
