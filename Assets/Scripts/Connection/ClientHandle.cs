using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ClientPack;
using UnityEngine.SceneManagement;

public class ClientHandle
{
    /// <summary>Assign a specific id for this client and for the tablet that is transformed to a color</summary>
    /// <param name="package">The package that is received</param>
    public static void AssignID(ClientPackage package){
        Client._id = package._toUser;
        Client._tablet._id = package._tabletInfo._idTablet;
        ServiceLocator.Instance.GetService<TabletUI>().AssingTeamColor();
    }

    /// <summary>Assign students to the tablet</summary>
    /// <param name="package">The package that is received</param>
    public static void AssignStudents(ClientPackage package)
    {
        Client._tablet._students = new List<Student>();
        Client._tablet._students = package._studentsInfo._studentsToTablets._students;
    }

    /// <summary>Assign minigame time, open the common scenario screen and select a student randomly to play</summary>
    /// <param name="package">The package that is received</param>
    public static void StartGame(ClientPackage package)
    {
        ServiceLocator.Instance.GetService<NetworkManager>()._minigameMinutes = package._minigameTime._minutes;
        ServiceLocator.Instance.GetService<NetworkManager>()._minigameSeconds = package._minigameTime._seconds;
        ServiceLocator.Instance.GetService<GameManager>().RandomizeStudentsList();
        ServiceLocator.Instance.GetService<TabletUI>().OpenNextWindow();
        ServiceLocator.Instance.GetService<TabletUI>().NewStudentGame();
    }

    /// <summary>Receives the specific difficulty for the given student/game </summary>
    /// <param name="package">The package that is received</param>
    public static void SpecificGameDifficulty(ClientPackage package)
    {
        ServiceLocator.Instance.GetService<NetworkManager>()._minigameLevel = package._gameDifficulty._level;
    }

    /// <summary>Pause the session</summary>
    /// <param name="package">The package that is received</param>
    public static void PauseGame(ClientPackage package)
    {
        if (package._pauseGame._pause)
        {
            ServiceLocator.Instance.GetService<IGameManager>().SetClientState(IGameManager.GAME_STATE_CLIENT.pause);
        }
        else
        {
            ServiceLocator.Instance.GetService<IGameManager>().SetClientState(IGameManager.GAME_STATE_CLIENT.playing);
        }
        EDebug.Log("Pausando? " + package._pauseGame._pause);
    }

    /// <summary>Quit minigames and show the final score</summary>
    /// <param name="package">The package that is received</param>
    public static void QuitGame()
    {
        ServiceLocator.Instance.GetService<GameManager>()._endSessionTablet = true;
       SceneManager.LoadScene("CristinaTest");
        //Poner que vaya a la ventana de puntuaciones finales
        EDebug.Log("Quitando el juego");
    }

    /// <summary>Turn off the tablet</summary>
    /// <param name="package">The package that is received</param>
    public static void TurnOff()
    {
        EDebug.Log("Apagando");
        ServiceLocator.Instance.GetService<TabletUI>().QuitGame();
    }
    public static void CantConnectServer()
    {
        ServiceLocator.Instance.GetService<TabletUI>()._cantConnectText.SetActive(true);
        ServiceLocator.Instance.GetService<TabletUI>()._continueNextScreen = false;
    }
    public static void CanConnectServer()
    {
        ServiceLocator.Instance.GetService<TabletUI>()._cantConnectText.SetActive(false);
        ServiceLocator.Instance.GetService<TabletUI>()._continueNextScreen = true;
    }
}
