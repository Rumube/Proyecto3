using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ServerPack;
public class ServerHandle {
    /// <summary>
    /// Activate the rocket that has already placed 
    /// all your students and is ready
    /// </summary>
    /// <param name="id">The rocket id</param>
    public static void UpdateReadyRockets(int id)
    {
        ServiceLocator.Instance.GetService<MobileUI>().UpdateReadyRockets(id);
    }
    /// <summary>Enable the continue button and change the help info</summary>
    public static void ContinueGameTime()
    {
        ServiceLocator.Instance.GetService<MobileUI>()._adviceStudents.text = "¡Todos los mininautas están preparados!";
        ServiceLocator.Instance.GetService<MobileUI>()._continueGameTimer.interactable = true;
    }
    /// <summary>Receives a student and game and proccess it for getting the difficulty</summary>
    /// <param name="package">The package that is received</param>
    public static void FindDificulty(ServerPackage _package) //NOT FINISHED
    {
        for (int i = 0; i < ServiceLocator.Instance.GetService<ServerUtility>()._ids.Count; ++i)
        {
            if (_package._fromUser == ServiceLocator.Instance.GetService<ServerUtility>()._ids[i])
            {
               ServiceLocator.Instance.GetService<INetworkManager>().GetStudentsToTablets()[i]._currentStudent = _package._selectStudentGame._studentName;
               ServiceLocator.Instance.GetService<INetworkManager>().GetStudentsToTablets()[i]._currentGame = _package._selectStudentGame._gameName;
            }      
        }
        int[] data = new int[2];
        data = ServiceLocator.Instance.GetService<IGameManager>().GetGameObject().GetComponent<Android>().GetDifficulty(_package._selectStudentGame._studentName, _package._selectStudentGame._gameName);

        ServiceLocator.Instance.GetService<ServerUtility>().MinigameDifficulty(_package._fromUser,data[0],data[1]);
    }
    /// <summary>
    /// Updates the number of tablets that are viewing the final point scene
    /// </summary>
    /// <param name="numberTabletsViewFinalScore"></param>
    public static void UpdateTabletsViewingFinalScore(int numberTabletsViewFinalScore)
    {
        ServiceLocator.Instance.GetService<MobileUI>().UpdateNumberTabletsLookingFinalScore();
    }
    /// <summary>
    /// It collects the values from the package and uses them to call the database update with the new data.
    ///It also updates the points of the equipment.
    /// </summary>
    /// <param name="_package">The package</param>
    public static void MatchData(ServerPackage _package)
    {
        string studentName = _package._matchData._studentName;
        string gameName = _package._matchData._gameName;
        int team = _package._matchData._team;
        int level = _package._matchData._gameLevel;
        int success = _package._matchData._averageSuccess;
        int errors = _package._matchData._averageErrors;
        int points = _package._matchData._averagePoints;
        float time = _package._matchData._averageGameTime;
        int idSession = ServiceLocator.Instance.GetService<ServerUtility>().gameObject.GetComponent<Android>().GetIDSession();

        ServiceLocator.Instance.GetService<ServerUtility>().gameObject.GetComponent<Android>().InsertMatch(idSession,studentName, gameName,team, success, errors, time, points, level);
        ServiceLocator.Instance.GetService<IGameManager>().SetTeamPoints(team, points);
        ServiceLocator.Instance.GetService<ServerUtility>().BroadcastTeamPoints();
    }
}
