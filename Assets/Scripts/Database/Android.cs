using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;
//References
using Mono.Data.Sqlite;
using System;
using System.Data;
using System.IO;
using UnityEngine.UI;
using TMPro;

public class Android : MonoBehaviour
{
    [Header("Envia a la base")]
    public Text _tName;
    public TMP_InputField _tInputNamePro;
    public InputField _tInputName;
    public Text _tId;

   //[Header("Recibe de la base")]
   // public Text _stateText, _infoText;
   // public int _Class;

    private string _conn, _sqlQuery;
    IDbConnection _dbconn;
    IDbCommand _dbcmd;
    private IDataReader _reader;

    string _DatabaseName = "Employers.s3db";


    [Header("ReaderStudent")]
    public GameObject _buttonPrefab;
    public Transform _location;

    int level;

    // Start is called before the first frame update
    void Start()
    {
        
        
    #if UNITY_EDITOR

        string filepath = Application.dataPath + "/Plugins/" + _DatabaseName;

        //open db connection
        _conn = "URI=file:" + filepath;

        EDebug.Log("Stablishing connection to: " + _conn);
        _dbconn = new SqliteConnection(_conn);
        _dbconn.Open();

#elif UNITY_ANDROID

        string filepath = Application.persistentDataPath + "/" + _DatabaseName;
            if (!File.Exists(filepath))
            {
                // If not found on android will create Tables and database

                EDebug.LogWarning("File \"" + filepath + "\" does not exist. Attempting to create from \"" +
                                 Application.dataPath + "!/assets/Employers");



                // UNITY_ANDROID
                WWW loadDB = new WWW("jar:file://" + Application.dataPath + "!/assets/Employers.s3db");
                while (!loadDB.isDone) { }
                // then save to Application.persistentDataPath
                File.WriteAllBytes(filepath, loadDB.bytes);
            }

#endif
        //Application database Path android

        _conn = "URI=file:" + filepath;

        EDebug.Log("Stablishing connection to: " + _conn);
        _dbconn = new SqliteConnection(_conn);
        _dbconn.Open();

#if UNITY_ANDROID

        string query;
        query = "create table if not exists Classroom (idClassroom INTEGER PRIMARY KEY   AUTOINCREMENT, Name varchar(100), UNIQUE(Name));"+

        " create table if not exists Student(idStudent INTEGER PRIMARY KEY   AUTOINCREMENT, Name varchar(100), idClassroom INTEGER, UNIQUE(Name), foreign key(idClassroom) references Classroom(idClassroom));"+

        " create table if not exists Session(idSession INTEGER PRIMARY KEY   AUTOINCREMENT, DateSession Date);"+
        
        "create table if not exists Game(idGame INTEGER PRIMARY KEY   AUTOINCREMENT, Name varchar(100), UNIQUE(Name));"+

        "create table if not exists Match("+
         "idMatch INTEGER PRIMARY KEY   AUTOINCREMENT, "+
         "idStudent INTEGER, idSession INTEGER, idGame INTEGER,"+
         "team integer, "+"level integer,"+
         "foreign key(idStudent) references Student(idStudent), "+
         "foreign key(idSession) references Session(idSession), "+
         "foreign key(idGame) references Game(idGame)); "
         ;
        try
        {
            _dbcmd = _dbconn.CreateCommand(); // create empty command
            _dbcmd.CommandText = query; // fill the command
            _reader = _dbcmd.ExecuteReader(); // execute command which returns a reader
        }
        catch (Exception e)
        {

            EDebug.Log(e);

        }
       

#endif
        //EDebug.Log (SearchMatchInfo("Lara", "JUEGO1"));
       
    }
    #region Buttons


    public void InsertClassButton()
    {
        if (!string.IsNullOrEmpty(_tInputNamePro.text))
        {
            while(_tInputNamePro.text.StartsWith(" ") || _tInputNamePro.text.StartsWith("\t"))
            {
                _tInputNamePro.text = _tInputNamePro.text.Remove(0,1);
            }
            while(_tInputNamePro.text.EndsWith(" ") || _tInputNamePro.text.EndsWith("\t"))
            {
                _tInputNamePro.text = _tInputNamePro.text.Remove(_tInputNamePro.text.Length-1, 1);
            }
            while(_tInputNamePro.text.Contains("  "))
            {
                _tInputNamePro.text = _tInputNamePro.text.Replace("  "," ");
            }

            InsertClass(_tInputNamePro.text.ToUpper());
        }
            
    }
    public void prueba()
    {
        Debug.Log(GetDifficulty("pepe", "JUEGO1"));
    }
    public void DeleteClassButton()
    {
        DeleteClass(_tInputNamePro.text.ToUpper());
    }
    public void UpdateClassButton()
    {
        UpdateClass(_tId.text,_tName.text.ToUpper());
    }
    public void ReadClassData()
    {
        ReaderClass();
    }
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

            InsertStudent(_tInputNamePro.text.ToUpper());
        }
    }
    public void DeleteStudentButton()
    {
        DeleteStudent(_tInputNamePro.text.ToUpper());
    }
    public void UpdateStudentButton()
    {
        UpdateStudent(_tId.text, _tName.text);
    }
    public void ReadStudentsData()
    {
        ReaderStudent(_buttonPrefab, _location);
    }

    #endregion

    #region Table Classroom
    /** 
    * @desc inserta una clase en la base de datos

    * @param string name - el nombre de la clase 

    */
    private void InsertClass(string name)
    {
        using (_dbconn = new SqliteConnection(_conn))
        {
            _dbconn.Open(); //Open connection to the database.
            _dbcmd = _dbconn.CreateCommand();
            _sqlQuery = string.Format("insert into Classroom (Name) values (\"{0}\")", name);// table name
            _dbcmd.CommandText = _sqlQuery;

            try
            {
                _dbcmd.ExecuteScalar();
                ServiceLocator.Instance.GetService<MobileUI>().PopupAddClass();
            }
            catch (Exception ex)
            {
                ServiceLocator.Instance.GetService<MobileUI>().AddingTwoClassesWithSameName();
            }
            _dbconn.Close();
        }
        //_infoText.text = "";

        ReaderClass();
    }
    /** 

    * @desc Borra una clase

    * @param string deleteById - El id de la clase que va a ser elimimada 

    */
    private void DeleteClass(string name)
    {
        using (_dbconn = new SqliteConnection(_conn))
        {

            _dbconn.Open(); //Open connection to the database.

            EDebug.Log("NAme: "+name);
            IDbCommand dbcmd2 = _dbconn.CreateCommand();
            string deleteById = "SELECT idClassroom FROM Classroom where Name = \"" + name  + "\"";
            EDebug.Log("sql: " + deleteById);
            dbcmd2.CommandText = deleteById;
            IDataReader reader2 = dbcmd2.ExecuteReader();
            EDebug.Log("Reader: "+reader2.Read());
            EDebug.Log("Reader: " + reader2.GetValue(0));


            IDbCommand dbcmd = _dbconn.CreateCommand();
            string sqlQuery = "DELETE FROM Classroom where idClassroom = " + reader2.GetValue(0);// table name
            EDebug.Log("sql1: " + sqlQuery);
            dbcmd.CommandText = sqlQuery;
            IDataReader reader = dbcmd.ExecuteReader();
            EDebug.Log("Reader1: " + reader);

            dbcmd.Dispose();
            dbcmd = null;

            dbcmd2.Dispose();
            dbcmd2 = null;
            _dbconn.Close();
            //_stateText.text = deleteById + " Delete  Done ";

        }
        //_infoText.text = "";
        ReaderClass();

    }
    /** 

    * @desc Cambia el nombre de una clase de la base

    * @param string updateId - El id de la clase que va a ser modificada 
    * @param string updateName- El nombre nuevo de la clase
    */
    private void UpdateClass(string updateId, string updateName)
    {
        using (_dbconn = new SqliteConnection(_conn))
        {
            _dbconn.Open(); //Open connection to the database.
            _dbcmd = _dbconn.CreateCommand();
            _sqlQuery = string.Format("UPDATE Classroom set Name = @name where idClassroom = @id ");

            SqliteParameter P_update_name = new SqliteParameter("@name", updateName);
           
            SqliteParameter P_update_id = new SqliteParameter("@id", updateId);

            _dbcmd.Parameters.Add(P_update_name);
           
            _dbcmd.Parameters.Add(P_update_id);

            _dbcmd.CommandText = _sqlQuery;
            _dbcmd.ExecuteScalar();
            _dbconn.Close();
            //Search_function(t_id_class.text);
            ReaderClass();
        }
    }

    /** 

    * @desc Lee toda la tabla Classroom de la base
    */
    private void ReaderClass()
    {
        int idreaders;
        string Namereaders;
        using (_dbconn = new SqliteConnection(_conn))
        {
            _dbconn.Open(); //Open connection to the database.
            IDbCommand dbcmd = _dbconn.CreateCommand();

            string sqlQuery = "SELECT  idClassroom, Name " + "FROM Classroom";// table name

            dbcmd.CommandText = sqlQuery;
            IDataReader reader = dbcmd.ExecuteReader();

            //Destroy all buttons
            foreach (Transform child in ServiceLocator.Instance.GetService<UIManager>()._classPanel.transform)
            {
                GameObject.Destroy(child.gameObject);
            }

            while (reader.Read())
            {
                idreaders = reader.GetInt32(0);

                Namereaders = reader.GetString(1);

               // _infoText.text += idreaders + Namereaders + " " + "\n";
                EDebug.Log("Value=" + idreaders + " name =" + Namereaders);
                GameObject newButton = Instantiate(ServiceLocator.Instance.GetService<UIManager>()._classButton, ServiceLocator.Instance.GetService<UIManager>()._classPanel.transform);
                newButton.GetComponentInChildren<TextMeshProUGUI>().text = Namereaders;
            }
            reader.Close();
            reader = null;
            dbcmd.Dispose();
            dbcmd = null;
            _dbconn.Close();
        }
    }
    #endregion
    #region Table Student
    /** 

* @desc Inserta un estudiante en la base de datos

* @param string Name - El nombre del estudiante que va a ser a�adido
*/
    private void InsertStudent(string name)
    {
        using (_dbconn = new SqliteConnection(_conn))
        {
            _dbconn.Open(); //Open connection to the database.

            Debug.Log("NAme: " + name);
            IDbCommand dbcmd2 = _dbconn.CreateCommand();
            string deleteById = "SELECT idClassroom FROM Classroom where Name = \"" + ServiceLocator.Instance.GetService<UIManager>()._classNamedb + "\"";
            EDebug.Log("sql: " + deleteById);
            dbcmd2.CommandText = deleteById;
            IDataReader reader2 = dbcmd2.ExecuteReader();
            EDebug.Log("Reader: " + reader2.Read());
            EDebug.Log("Reader: " + reader2.GetValue(0));


            _dbcmd = _dbconn.CreateCommand();
            _sqlQuery = string.Format("insert into Student (Name, idClassroom) values (\"{0}\",\"{1}\")", name, reader2.GetValue(0));// table name
            _dbcmd.CommandText = _sqlQuery;
            try
            {
                _dbcmd.ExecuteScalar();
                ServiceLocator.Instance.GetService<MobileUI>().PopupAddStudent();
            }
            catch (Exception ex)
            {
                ServiceLocator.Instance.GetService<MobileUI>().AddingTwoStudentsWithSameName();
            }

            reader2.Close();
            reader2 = null;
            dbcmd2.Dispose();
            dbcmd2 = null;

            _dbconn.Close();
        }
        //_infoText.text = "";
        EDebug.Log("Insert Done  ");

        ReaderStudent(_buttonPrefab, _location);
    }
    /** 

* @desc Borra un estudiante de la base

* @param string deleteById - El id del estudiante que va a ser elimimado

*/
    private void DeleteStudent(string name)
    {
        using (_dbconn = new SqliteConnection(_conn))
        {

            _dbconn.Open(); //Open connection to the database.

            Debug.Log("NAme: " + name);
            IDbCommand dbcmd2 = _dbconn.CreateCommand();
            string deleteById = "SELECT idStudent FROM Student where Name = \"" + name + "\"";
            EDebug.Log("sql: " + deleteById);
            dbcmd2.CommandText = deleteById;
            IDataReader reader2 = dbcmd2.ExecuteReader();
            EDebug.Log("Reader: " + reader2.Read());
            EDebug.Log("Reader: " + reader2.GetValue(0));

            IDbCommand dbcmd = _dbconn.CreateCommand();
            string sqlQuery = "DELETE FROM Student where idStudent = " + reader2.GetValue(0);// table name
            dbcmd.CommandText = sqlQuery;
            IDataReader reader = dbcmd.ExecuteReader();

            reader.Close();
            reader = null;
            reader2.Close();
            reader2 = null;
            dbcmd.Dispose();
            dbcmd = null;
            _dbconn.Close();
           // _stateText.text = deleteById + " Delete  Done ";

        }
        ReaderStudent(_buttonPrefab, _location);

    }
    /** 

    * @desc Cambia el nombre de un estudiante de la base

    * @param string updateId - El id del estudiante que va a ser modificado 
    * @param string updateName- El nombre nuevo del estudiante

    */
    private void UpdateStudent(string updateId, string updateName)
    {
        using (_dbconn = new SqliteConnection(_conn))
        {
            _dbconn.Open(); //Open connection to the database.
            _dbcmd = _dbconn.CreateCommand();
            _sqlQuery = string.Format("UPDATE Student set Name = @name where idStudent = @id ");

            SqliteParameter P_update_name = new SqliteParameter("@name", updateName);

            SqliteParameter P_update_id = new SqliteParameter("@id", updateId);

            _dbcmd.Parameters.Add(P_update_name);

            _dbcmd.Parameters.Add(P_update_id);

            _dbcmd.CommandText = _sqlQuery;
            _dbcmd.ExecuteScalar();
            _dbconn.Close();
            //Search_function(t_id_class.text);
            //ReaderStudent();
        }
    }
    /** 

    * @desc Lee toda la tabla Student de la base
    */
    public void ReaderStudent(GameObject prefab, Transform location)
    {
        int id_Student_readers, id_Classroom_readers;
        string Namereaders;
        string className = ServiceLocator.Instance.GetService<UIManager>()._classNamedb;
        using (_dbconn = new SqliteConnection(_conn))
        {
            _dbconn.Open(); //Open connection to the database.

            //Destroy all buttons
            foreach (Transform child in location)
            {
                GameObject.Destroy(child.gameObject);
            }

            EDebug.Log("NAme: " + name);
            IDbCommand dbcmd2 = _dbconn.CreateCommand();
            string deleteById = "SELECT idClassroom FROM Classroom where Name = \"" + className + "\"";
            EDebug.Log("sql: " + deleteById);
            dbcmd2.CommandText = deleteById;
            IDataReader reader2 = dbcmd2.ExecuteReader();
            EDebug.Log("Reader: " + reader2.Read());
            EDebug.Log("Reader: " + reader2.GetValue(0));


            IDbCommand dbcmd = _dbconn.CreateCommand();
            string sqlQuery = "SELECT idStudent, Name, idClassroom " + "FROM Student WHERE idClassroom = " + reader2.GetValue(0);// table name
            dbcmd.CommandText = sqlQuery;
            IDataReader reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                id_Student_readers = reader.GetInt32(0);
                Namereaders = reader.GetString(1); ;
                id_Classroom_readers = reader.GetInt32(2);
              //  _infoText.text += id_Student_readers + Namereaders + id_Classroom_readers + " " + "\n";

                EDebug.Log("Value=" + id_Student_readers + " name =" + Namereaders + " Clase =" + id_Classroom_readers);
                GameObject newButton = Instantiate(prefab, location);

                newButton.GetComponentInChildren<TextMeshProUGUI>().text = Namereaders;
                newButton.GetComponentInChildren<StudentButton>()._student = new Student();
                newButton.GetComponentInChildren<StudentButton>()._student._id = id_Student_readers;
                newButton.GetComponentInChildren<StudentButton>()._student._name = Namereaders;
                newButton.GetComponentInChildren<StudentButton>()._student._idClass = id_Classroom_readers;
            }
            reader.Close();
            reader = null;
            reader2.Close();
            reader2 = null;
            dbcmd.Dispose();
            dbcmd = null;
            dbcmd2.Dispose();
            dbcmd2 = null;
            _dbconn.Close();

        }
    }
    /** 

    * @desc Busca a todos los estudiantes de una misma clase

    * @param string searchByClass - El id de la clase de los alumnos

    */
    private void SearchStudent(string searchByClass)
    {
        using (_dbconn = new SqliteConnection(_conn))
        {
            string Name_readers_Search;
            _dbconn.Open(); //Open connection to the database.
            IDbCommand dbcmd = _dbconn.CreateCommand();
            string sqlQuery = "SELECT Name " + "FROM Student where idClassroom =" + searchByClass;// table name
            dbcmd.CommandText = sqlQuery;
            IDataReader reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                //  string id = reader.GetString(0);
                Name_readers_Search = reader.GetString(0);
               
               // _stateText.text += Name_readers_Search + " - " +  "\n";

                EDebug.Log(" name =" + Name_readers_Search );

            }
            reader.Close();
            reader = null;
            dbcmd.Dispose();
            dbcmd = null;
            _dbconn.Close();

        }

    }
    #endregion
    #region Table Session
    /** 

    * @desc A�ade la fecha y hora actuales a la base
    */
    public void InsertSession()
    {
        using (_dbconn = new SqliteConnection(_conn))
        {
            _dbconn.Open(); //Open connection to the database.
            _dbcmd = _dbconn.CreateCommand();
            _sqlQuery = string.Format("insert into Session (DateSession) values (DateTime('now','localtime'))");// table name
           
            _dbcmd.CommandText = _sqlQuery;
            _dbcmd.ExecuteScalar();
            _dbconn.Close();
        }
       // _infoText.text = "";
        EDebug.Log("Insert Done  ");

        ReaderDate();
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
    public int GetDifficulty(string nameStudent, string nameGame)
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
            string difficulty = "SELECT level FROM Match where idStudent = \"" + reader2.GetValue(0) + "\""/*+ " AND idGame = \"" + reader.GetValue(0) + "\""*/;
            dbcmd3.CommandText = difficulty;
            IDataReader reader3 = dbcmd3.ExecuteReader();
            //string json= reader3.GetString(0);
            //int level = reader3.GetInt32(0);
            //EDebug.Log("Reader: Level" + reader3.GetValue(0));
            while (reader3.Read())
            {
                level = reader3.GetInt32(0);
                EDebug.Log("Reader: Level int" + level);
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
            return level;
            //return json;
        }
        // _infoText.text = "";
    }
    /** 

    * @desc Registra una partida a la base

    * @param int idStudent - El id del estudiante
    * @param int idGSession- El id de la sesi�n
* @param int idGame- El id del juego al que ha jugado
    * @param int team- El equipo del estudiante
* @param string matchInfo- Informacion de la partida

    */
    public void InsertMatch(string nameStudent, string nameGame, int team, int success, int errors, float gameTime, int points, int level)
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

            int idSession = GetIDSession();
            _dbcmd = _dbconn.CreateCommand();
            _sqlQuery = string.Format("insert into Match (idStudent, idSession, idGame, team, level, averageSuccess, averageErrors, averagePoints, averageTime) values (\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\",\"{7}\",\"{8}\")", idStudent, idSession, idGame, team, level, success, errors, points, gameTime);// table name
            _dbcmd.CommandText = _sqlQuery;
            _dbcmd.ExecuteScalar();
            _dbconn.Close();
        }
       // _infoText.text = "";
        EDebug.Log("Insert Done: InsertMatch");

    }
    #endregion


    ////Search 
    //public void Search_button()
    //{
    //    data_staff.text = "";
    //    Search_function(t_id.text);

    //}

    ////Found to Update 
    //public void F_to_update_button()
    //{
    //    data_staff.text = "";
    //    F_to_update_function(t_id.text);

    //}
    ////Update
    //public void Update_button()
    //{
    //    update_function(t_id.text, t_name.text, t_Address.text);

    //}

    ////Search on Database by ID
    //private void Search_function(string Search_by_id)
    //{
    //    using (dbconn = new SqliteConnection(conn))
    //    {
    //        string Name_readers_Search, Address_readers_Search;
    //        dbconn.Open(); //Open connection to the database.
    //        IDbCommand dbcmd = dbconn.CreateCommand();
    //        string sqlQuery = "SELECT name,address " + "FROM Staff where id =" + Search_by_id;// table name
    //        dbcmd.CommandText = sqlQuery;
    //        IDataReader reader = dbcmd.ExecuteReader();
    //        while (reader.Read())
    //        {
    //            //  string id = reader.GetString(0);
    //            Name_readers_Search = reader.GetString(0);
    //            Address_readers_Search = reader.GetString(1);
    //            data_staff.text += Name_readers_Search + " - " + Address_readers_Search + "\n";

    //            EDebug.Log(" name =" + Name_readers_Search + "Address=" + Address_readers_Search);

    //        }
    //        reader.Close();
    //        reader = null;
    //        dbcmd.Dispose();
    //        dbcmd = null;
    //        dbconn.Close();


    //    }

    //}


    ////Search on Database by ID
    //private void F_to_update_function(string Search_by_id)
    //{
    //    using (dbconn = new SqliteConnection(conn))
    //    {
    //        string Name_readers_Search, Address_readers_Search;
    //        dbconn.Open(); //Open connection to the database.
    //        IDbCommand dbcmd = dbconn.CreateCommand();
    //        string sqlQuery = "SELECT name,address " + "FROM Staff where id =" + Search_by_id;// table name
    //        dbcmd.CommandText = sqlQuery;
    //        IDataReader reader = dbcmd.ExecuteReader();
    //        while (reader.Read())
    //        {

    //            Name_readers_Search = reader.GetString(0);
    //            Address_readers_Search = reader.GetString(1);
    //            t_name.text = Name_readers_Search;
    //            t_Address.text = Address_readers_Search;

    //        }
    //        reader.Close();
    //        reader = null;
    //        dbcmd.Dispose();
    //        dbcmd = null;
    //        dbconn.Close();


    //    }

    //}
    ////Update on  Database 
    //private void update_function(string update_id, string update_name, string update_address)
    //{
    //    using (dbconn = new SqliteConnection(conn))
    //    {
    //        dbconn.Open(); //Open connection to the database.
    //        dbcmd = dbconn.CreateCommand();
    //        sqlQuery = string.Format("UPDATE Staff set name = @name ,address = @address where ID = @id ");

    //        SqliteParameter P_update_name = new SqliteParameter("@name", update_name);
    //        SqliteParameter P_update_address = new SqliteParameter("@address", update_address);
    //        SqliteParameter P_update_id = new SqliteParameter("@id", update_id);

    //        dbcmd.Parameters.Add(P_update_name);
    //        dbcmd.Parameters.Add(P_update_address);
    //        dbcmd.Parameters.Add(P_update_id);

    //        dbcmd.CommandText = sqlQuery;
    //        dbcmd.ExecuteScalar();
    //        dbconn.Close();
    //        Search_function(t_id.text);
    //    }

    //    // SceneManager.LoadScene("home");
    //}


}