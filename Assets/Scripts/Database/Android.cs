using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;
//References
using System;
using System.Data;
using System.IO;
using UnityEngine.UI;
using TMPro;

public class Android : MonoBehaviour
{
    [Header("Database")]
    public DataService _dataService;
    public Text _tName;
    public TMP_InputField _tInputNamePro;
    public InputField _tInputName;
    public Text _tId;

    string _DatabaseName = "Mininautas.s3db";


    [Header("ReaderStudent")]
    public GameObject _buttonPrefab;
    public Transform _location;

    #region Buttons
    /// <summary>
    /// Connect to the database and add value to <see cref="_dataService"/>
    /// </summary>
    public void ConnectToDataBase()
    {
        _dataService = new DataService(_DatabaseName);
    }
    /// <summary>
    /// Starts the process to close the database connection
    /// </summary>
    public void CloseDataBase()
    {
        _dataService.CloaseDatabase();
    }
    /// <summary>
     /// Only the first time connect to the database and add value to <see cref="_dataService"/>
     /// </summary>
    public void NewConnectToDataBase()
    {
        _dataService = new DataService(_DatabaseName);
        _dataService.CreateDB();
        ReadClassData();

    }
    /// <summary>
    /// Starts the process to create a new Classroom
    /// </summary>
    public void InsertClassButton()
    {
        ConnectToDataBase();
        if (!string.IsNullOrEmpty(_tInputNamePro.text))
        {
            while (_tInputNamePro.text.StartsWith(" ") || _tInputNamePro.text.StartsWith("\t"))
            {
                _tInputNamePro.text = _tInputNamePro.text.Remove(0, 1);
            }
            while (_tInputNamePro.text.EndsWith(" ") || _tInputNamePro.text.EndsWith("\t"))
            {
                _tInputNamePro.text = _tInputNamePro.text.Remove(_tInputNamePro.text.Length - 1, 1);
            }
            while (_tInputNamePro.text.Contains("  "))
            {
                _tInputNamePro.text = _tInputNamePro.text.Replace("  ", " ");
            }
            //Inser new classroom
            ClassroomDB newClassroom = _dataService.InsertClass(_tInputNamePro.text);

            if (newClassroom != null)
            {
                UpdateClassPanel();
            }
        }
        CloseDataBase();
    }
    /// <summary>
    /// Starts the process to delete and create the table in classPanel
    /// </summary>
    private void UpdateClassPanel()
    {
        ConnectToDataBase();
        DestroyClassesPanel();
        IEnumerable<ClassroomDB> classroomTable = _dataService.GetAllClassrooms();

        foreach (ClassroomDB classroom in classroomTable)
        {
            GameObject newButton = Instantiate(ServiceLocator.Instance.GetService<UIManager>()._classButton, ServiceLocator.Instance.GetService<UIManager>()._classPanel.transform);
            newButton.GetComponentInChildren<TextMeshProUGUI>().text = classroom.name;
        }
        CloseDataBase();
    }
    /// <summary>
    /// Destroy the ClassesPanel's elements
    /// </summary>
    private void DestroyClassesPanel()
    {
        //Destroy all buttons
        foreach (Transform child in ServiceLocator.Instance.GetService<UIManager>()._classPanel.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
    /// <summary>
    /// Starts the process to Delete a Classroom
    /// </summary>
    public void DeleteClassButton()
    {
        ConnectToDataBase();
        _dataService.DeleteClass(_tInputNamePro.text);
        CloseDataBase();
        UpdateClassPanel();
    }
    /// <summary>
    /// Calls <see cref="UpdateClassPanel"/>
    /// </summary>
    public void ReadClassData()
    {
        UpdateClassPanel();
    }

    /// <summary>
    /// Checks the student are rigth writte and calls <see cref="DataService.InsertStudent(string)"/>
    /// </summary>
    public void InsertStudentButton()
    {
        ConnectToDataBase();
        if (!string.IsNullOrEmpty(_tInputNamePro.text))
        {
            while (_tInputNamePro.text.StartsWith(" ") || _tInputNamePro.text.StartsWith("\t"))
            {
                _tInputNamePro.text = _tInputNamePro.text.Remove(0, 1);
            }
            while (_tInputNamePro.text.EndsWith(" ") || _tInputNamePro.text.EndsWith("\t"))
            {
                _tInputNamePro.text = _tInputNamePro.text.Remove(_tInputNamePro.text.Length - 1, 1);
            }
            while (_tInputNamePro.text.Contains("  "))
            {
                _tInputNamePro.text = _tInputNamePro.text.Replace("  ", " ");
            }
            _dataService.InsertStudent(_tInputNamePro.text.ToUpper());
            ReaderStudent(_buttonPrefab, _location);
        }
        CloseDataBase();
    }
    /// <summary>
    /// Start's the process to delete the selected student
    /// </summary>
    public void DeleteStudentButton()
    {
        ConnectToDataBase();
        _dataService.DeleteStudent(_tInputNamePro.text.ToUpper());
        CloseDataBase();
        ReaderStudent(_buttonPrefab, _location);
    }
    public void UpdateStudentButton()
    {
        print("Is neccesary?");
        //UpdateStudent(_tId.text, _tName.text);
    }
    /// <summary>
    /// Button Reader Student
    /// </summary>
    public void ReadStudentsData()
    {
        ReaderStudent(_buttonPrefab, _location);
    }

    #endregion
    #region Table Student
    /// <summary>
    /// Starts the process to create a student
    /// </summary>
    /// <param name="name"></param>
    private void InsertStudent(string name)
    {
        ConnectToDataBase();
        _dataService.InsertStudent(name);
        ReaderStudent(_buttonPrefab, _location);
        CloseDataBase();
    }

    /// <summary>
    /// Starts the process to delete a student
    /// </summary>
    /// <param name="name">Student's name</param>
    private void DeleteStudent(string name)
    {
        ConnectToDataBase();
        _dataService.DeleteStudent(name);

        ReaderStudent(_buttonPrefab, _location);
        CloseDataBase();
    }
    /// <summary>
    /// Starts the process to destroy and create new student panel
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="location"></param>
    public void ReaderStudent(GameObject prefab, Transform location)
    {
        ConnectToDataBase();
        //Get Classroom Data
        string className = ServiceLocator.Instance.GetService<UIManager>()._classNamedb;
        ClassroomDB currentClassroom = _dataService.GetClass(className);

        //Get StudentData
        IEnumerable<StudentDB> students = _dataService.GetStudent(currentClassroom.idClassroom);

        //Destroy buttons
        DestroyChild(location);

        foreach (StudentDB studentDB in students)
        {
            GameObject newButton = Instantiate(prefab, location);

            newButton.GetComponentInChildren<TextMeshProUGUI>().text = studentDB.name;
            newButton.GetComponentInChildren<StudentButton>()._student = new Student();
            newButton.GetComponentInChildren<StudentButton>()._student._id = studentDB.idStudent;
            newButton.GetComponentInChildren<StudentButton>()._student._name = studentDB.name;
            newButton.GetComponentInChildren<StudentButton>()._student._idClass = studentDB.idClassroom;
        }
        CloseDataBase();
    }
    /// <summary>
    /// Destroy the child's parent
    /// </summary>
    /// <param name="location">The parent</param>
    private void DestroyChild(Transform location)
    {
        //Destroy all buttons
        foreach (Transform child in location)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
    /// <summary>
    /// I don't know
    /// </summary>
    /// <param name="idClassroom"></param>
    private void SearchStudent(string idClassroom)
    {
        ConnectToDataBase();
        print("Use?!?!");
        IEnumerable<StudentDB> students = _dataService.GetStudent(int.Parse(idClassroom));
        CloseDataBase();
    }
    #endregion
    #region Table Session
    /// <summary>
    /// Starts the process to create a session
    /// </summary>
    public void InsertSession()
    {
        ConnectToDataBase();
        SessionDB newSession = null;
        newSession = (SessionDB)_dataService.InsertSession();

        if(newSession != null)
        {
            ReaderDate();
        }
        CloseDataBase();
    }
    /// <summary>
    /// Only call's <see cref="DataService.GetAllSessions"/>
    /// </summary>
    private void ReaderDate()
    {
        ConnectToDataBase();
        IEnumerable<SessionDB> sessions = null;
        sessions = _dataService.GetAllSessions();
        CloseDataBase();
    }
    /// <summary>
    /// Returns the last session's ID
    /// </summary>
    /// <returns><see cref="SessionDB.idSession"/></returns>
    public int GetIDSession()
    {
        ConnectToDataBase();
        int id = -1;
        SessionDB session = _dataService.GetSessionOrderBy();
        if(session != null)
        {
            id = session.idSession;
        }
        CloseDataBase();
        return id;
    }
    #endregion
    #region Table Match
    /// <summary>Search and returns the difficulty</summary>
    /// <param name="nameStudent">Name of the student</param>
    /// <param name="nameGame">Name of the game</param>
    ///<returns>The number of difficulty, if there is not record returns 0.</returns>
    public int[] GetDifficulty(string nameStudent, string nameGame)
    {
        ConnectToDataBase();
        int idGame = _dataService.GetGame(nameGame).idGame;
        int idStudent = _dataService.GetStudent(nameStudent).idStudent;

        MatchDB currentMatch = _dataService.GetMatchDifficultyData(idStudent, idGame);
        int[] data = new int[2];

        if (currentMatch != null)
        {
            data[0] = currentMatch.level;
            data[1] = currentMatch.averagePoints;
        }
        CloseDataBase();
        return data;
    }
    /// <summary>
    /// Starts the process to create a new Match
    /// </summary>
    /// <param name="idSession">Session's ID</param>
    /// <param name="nameStudent">Student's ID</param>
    /// <param name="nameGame">Game's ID</param>
    /// <param name="team">Number's team</param>
    /// <param name="success">Average of success in the match</param>
    /// <param name="errors">Average of errors in the match</param>
    /// <param name="gameTime">Time to complet the game</param>
    /// <param name="points">Average of points int the match</param>
    /// <param name="level">Level's game</param>
    public void InsertMatch(int idSession, string nameStudent, string nameGame, int team, int success, int errors, float gameTime, int points, int level)
    {
        ConnectToDataBase();
        int idGame = _dataService.GetGame(nameGame).idGame;
        int idStudent = _dataService.GetStudent(nameStudent).idStudent;

        MatchDB newMatch = _dataService.InsertMatch(idStudent, idSession, idGame, team, level, success, errors, points, gameTime);

        if(newMatch != null)
        {
            EDebug.Log("Insert Done: InsertMatch");
        }
        else
        {
            EDebug.Log("Ocurrión un error");
        }
        CloseDataBase();
    }
    #endregion
}