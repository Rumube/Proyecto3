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

    private string _conn, _sqlQuery;
    IDbConnection _dbconn;
    IDbCommand _dbcmd;
    private IDataReader _reader;

    string _DatabaseName = "Mininautas.s3db";


    [Header("ReaderStudent")]
    public GameObject _buttonPrefab;
    public Transform _location;

    int level;
    int averagePoints;
    // Start is called before the first frame update
    #region Buttons
    /// <summary>
    /// Starts the process to create a new Classroom
    /// </summary>
    public void InsertClassButton()
    {
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
    }
    /// <summary>
    /// Starts the process to delete and create the table in classPanel
    /// </summary>
    private void UpdateClassPanel()
    {
        DestroyClassesPanel();
        IEnumerable<ClassroomDB> classroomTable = _dataService.GetAllClassrooms();

        foreach (ClassroomDB classroom in classroomTable)
        {
            GameObject newButton = Instantiate(ServiceLocator.Instance.GetService<UIManager>()._classButton, ServiceLocator.Instance.GetService<UIManager>()._classPanel.transform);
            newButton.GetComponentInChildren<TextMeshProUGUI>().text = classroom.name;
        }
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
        _dataService.DeleteClass(_tInputNamePro.text);
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
        }
    }
    public void DeleteStudentButton()
    {
        _dataService.DeleteStudent(_tInputNamePro.text.ToUpper());
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
        _dataService.InsertStudent(name);
        ReaderStudent(_buttonPrefab, _location);
    }

    /// <summary>
    /// Starts the process to delete a student
    /// </summary>
    /// <param name="name">Student's name</param>
    private void DeleteStudent(string name)
    {
        string className = ServiceLocator.Instance.GetService<UIManager>()._classNamedb;
        ClassroomDB currentClassroom = (ClassroomDB)_dataService.GetClass(className);

        _dataService.DeleteStudent(name);

        ReaderStudent(_buttonPrefab, _location);
    }
    /// <summary>
    /// Starts the process to destroy and create new student panel
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="location"></param>
    public void ReaderStudent(GameObject prefab, Transform location)
    {
        //Get Classroom Data
        string className = ServiceLocator.Instance.GetService<UIManager>()._classNamedb;
        ClassroomDB currentClassroom = (ClassroomDB)_dataService.GetClass(className);

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
        print("Use?!?!");
        IEnumerable<StudentDB> students = _dataService.GetStudent(int.Parse(idClassroom));
    }
    #endregion
    #region Table Session
    /// <summary>
    /// Starts the process to create a session
    /// </summary>
    public void InsertSession()
    {
        SessionDB newSession = null;
        newSession = (SessionDB)_dataService.InsertSession();

        if(newSession != null)
        {
            ReaderDate();
        }
    }
    /** 

    * @desc Lee la fecha de la tabla Session de la base
    */
    private void ReaderDate()
    {
        string date;
        using (_dbconn = new SqliteConnection(_conn))
        {
            _dbconn.Open(); //Open connection to the database.
            IDbCommand dbcmd = _dbconn.CreateCommand();
            string sqlQuery = "SELECT DateSession " + "FROM Session";// table name
            dbcmd.CommandText = sqlQuery;
            IDataReader reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                date = reader.GetString(0);

                // _infoText.text += date + " " + "\n";
                EDebug.Log("Value=" + date + " name =");
            }
            reader.Close();
            reader = null;
            dbcmd.Dispose();
            dbcmd = null;
            _dbconn.Close();
            //       dbconn = null;

        }
    }

    public int GetIDSession()
    {
        int id = -1;
        using (_dbconn = new SqliteConnection(_conn))
        {
            _dbconn.Open(); //Open connection to the database.
            IDbCommand dbcmd = _dbconn.CreateCommand();
            string sqlQuery = "SELECT idSession " + "FROM Session ORDER BY idSession DESC LIMIT 1";
            dbcmd.CommandText = sqlQuery;
            IDataReader reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                id = reader.GetInt32(0);

                // _infoText.text += date + " " + "\n";
                EDebug.Log("Value=" + id + " name =");
            }

            reader.Close();
            reader = null;

            dbcmd.Dispose();
            dbcmd = null;

            _dbconn.Close();
            //       dbconn = null;
        }
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

        using (_dbconn = new SqliteConnection(_conn))
        {
            _dbconn.Open(); //Open connection to the database.
            _dbcmd = _dbconn.CreateCommand();

            string idGame = "SELECT idGame FROM Game where Name = \"" + nameGame + "\"";
            _dbcmd.CommandText = idGame;
            IDataReader reader = _dbcmd.ExecuteReader();

            IDbCommand dbcmd2 = _dbconn.CreateCommand();
            string idStudent = "SELECT idStudent FROM Student where Name = \"" + nameStudent + "\"";
            dbcmd2.CommandText = idStudent;
            IDataReader reader2 = dbcmd2.ExecuteReader();
            EDebug.Log("Reader: GAME " + reader.GetValue(0));
            EDebug.Log("Reader: STUDENT " + reader2.GetValue(0));
            IDbCommand dbcmd3 = _dbconn.CreateCommand();
            string difficulty = "SELECT level,averagePoints FROM Match where idStudent = \"" + reader2.GetValue(0) + "\"" + " AND idGame = \"" + reader.GetValue(0) + "\"";
            dbcmd3.CommandText = difficulty;
            IDataReader reader3 = dbcmd3.ExecuteReader();
            while (reader3.Read())
            {
                level = reader3.GetInt32(0);
                averagePoints = reader3.GetInt32(1);
                EDebug.Log("Reader: Level int" + level + " averagePoints:" + averagePoints);
            }


            reader.Close();
            reader = null;
            reader2.Close();
            reader2 = null;
            reader3.Close();
            reader3 = null;
            _dbcmd.Dispose();
            _dbcmd = null;
            dbcmd2.Dispose();
            dbcmd2 = null;
            dbcmd3.Dispose();
            dbcmd3 = null;

            _dbconn.Close();

            int[] data = new int[2];
            data[0] = level;
            data[1] = averagePoints;

            return data;
            //return json;
        }
        // _infoText.text = "";
    }
    /** 

    * @desc Registra una partida a la base

    * @param int idStudent - El id del estudiante
    * @param int idGSession- El id de la sesión
* @param int idGame- El id del juego al que ha jugado
    * @param int team- El equipo del estudiante
* @param string matchInfo- Informacion de la partida

    */
    public void InsertMatch(int idSession, string nameStudent, string nameGame, int team, int success, int errors, float gameTime, int points, int level)
    {
        using (_dbconn = new SqliteConnection(_conn))
        {
            _dbconn.Open(); //Open connection to the database.

            _dbcmd = _dbconn.CreateCommand();
            string idGame = "SELECT idGame FROM Game where Name = \"" + nameGame + "\"";
            _dbcmd.CommandText = idGame;
            IDataReader reader = _dbcmd.ExecuteReader();

            IDbCommand dbcmd2 = _dbconn.CreateCommand();
            string idStudent = "SELECT idStudent FROM Student where Name = \"" + nameStudent + "\"";
            dbcmd2.CommandText = idStudent;
            IDataReader reader2 = dbcmd2.ExecuteReader();

            EDebug.Log("Reader: GAME " + reader.GetValue(0));
            EDebug.Log("Reader: STUDENT " + reader2.GetValue(0));

            IDbCommand _dbcmd3 = _dbconn.CreateCommand();
            _sqlQuery = string.Format("insert into Match (idStudent, idSession, idGame, team, level, averageSuccess, averageErrors, averagePoints, averageTime) values (\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\",\"{7}\",\"{8}\")", reader2.GetValue(0), idSession, reader.GetValue(0), team, level, success, errors, points, gameTime);// table name
            _dbcmd3.CommandText = _sqlQuery;
            _dbcmd3.ExecuteScalar();

            reader.Close();
            reader = null;
            reader2.Close();
            reader2 = null;
            _dbcmd.Dispose();
            _dbcmd = null;
            dbcmd2.Dispose();
            dbcmd2 = null;
            _dbcmd3.Dispose();
            _dbcmd3 = null;

            _dbconn.Close();
        }
        // _infoText.text = "";
        EDebug.Log("Insert Done: InsertMatch");

    }
    #endregion
}