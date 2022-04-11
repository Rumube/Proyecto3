using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ClientPack;
public class ClientHandle
{
    public static void AssignID(ClientPackage package){
        Client._id = package._toUser;
        Client._tablet._id = package._tabletInfo._idTablet;
        ServiceLocator.Instance.GetService<TabletUI>().AssingTeamColor();
    }

    public static void AssignStudents(ClientPackage package)
    {
        Client._tablet._students = new List<Student>();
        Client._tablet._students = package._studentsInfo._studentsToTablets._students;
    }
    public static void StartGame(ClientPackage package)
    {
        ServiceLocator.Instance.GetService<NetworkManager>()._minigameMinutes = package._minigameTime._minutes;
        ServiceLocator.Instance.GetService<NetworkManager>()._minigameSeconds = package._minigameTime._seconds;

    }

    public static void SpecificGameDifficulty(ClientPackage package)
    {
        //TODO: saber que recibo y tratar la dificultad
    }

    public static void PauseGame(ClientPackage package)
    {
        ServiceLocator.Instance.GetService<GameManager>()._pause = package._pauseGame._pause;
        Debug.Log("Pausando? " + ServiceLocator.Instance.GetService<GameManager>()._pause);
    }
    public static void QuitGame()
    {
        Debug.Log("Quitando el juego");
    }

    public static void TurnOff()
    {
        QuitGame();
    }
}
