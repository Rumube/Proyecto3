using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ServerPack;
public class ServerHandle { 

    public static void ContinueGameTime()
    {
        ServiceLocator.Instance.GetService<MobileUI>()._adviceStudents.text = "�Todos los mininautas est�n preparados!";
        ServiceLocator.Instance.GetService<MobileUI>()._continueGameTimer.interactable = true;
    }

    public static void FindDificulty(ServerPackage _package)
    {
        for (int i = 0; i < ServiceLocator.Instance.GetService<ServerUtility>()._ids.Count; ++i)
        {
            if (_package._fromUser == ServiceLocator.Instance.GetService<ServerUtility>()._ids[i])
            {
               ServiceLocator.Instance.GetService<NetworkManager>()._studentsToTablets[i]._currentStudent = _package._selectStudentGame._studentName;
               ServiceLocator.Instance.GetService<NetworkManager>()._studentsToTablets[i]._currentGame = _package._selectStudentGame._gameName;
            }      
        }
        Debug.Log("Entra paquete nino minijuego: " +_package._selectStudentGame._studentName + " g " + _package._selectStudentGame._gameName);
        //ServiceLocator.Instance.GetService<ServerUtility>().MinigameDifficulty(_package._fromUser, ServiceLocator.Instance.GetService<GameManager>().gameObject.GetComponent<Android>().GetDifficulty(_package._selectStudentGame._studentName, _package._selectStudentGame._gameName));
        //ServiceLocator.Instance.GetService<ServerUtility>().MinigameDifficulty(_package._fromUser, ServiceLocator.Instance.GetService<GameManager>().gameObject.GetComponent<Android>().GetDifficulty(_package._selectStudentGame._studentName, "JGO1"));// el de arriba es el de verdad
    }
}
