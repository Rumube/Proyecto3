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
public class Android : MonoBehaviour
{
    [Header("Envia a la base")]
    public Text _tName;
    public Text _tId;

   [Header("Recibe de la base")]
    public Text _stateText, _infoText;
    public int _Class;

    private string _conn, _sqlQuery;
    IDbConnection _dbconn;
    IDbCommand _dbcmd;
    private IDataReader _reader;

    string _DatabaseName = "Employers.s3db";
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
        query = "create table if not exists Classroom (idClassroom INTEGER PRIMARY KEY   AUTOINCREMENT, Name varchar(100));"+

        " create table if not exists Student(idStudent INTEGER PRIMARY KEY   AUTOINCREMENT, Name varchar(100), idClassroom INTEGER, foreign key(idClassroom) references Classroom(idClassroom));"+

        " create table if not exists Session(idSession INTEGER PRIMARY KEY   AUTOINCREMENT, DateSession Date);"+
        
        "create table if not exists Game(idGame INTEGER PRIMARY KEY   AUTOINCREMENT, Name varchar(100), UNIQUE(Name));"+

        "create table if not exists Match("+
         "idMatch INTEGER PRIMARY KEY   AUTOINCREMENT, "+
         "idStudent INTEGER, idSession INTEGER, idLevelConf INTEGER,"+
         "team integer, "+"matchInfo varchar(200),"+
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
        query = "CREATE TABLE Kids (ID INTEGER PRIMARY KEY   AUTOINCREMENT, Name varchar(100), Points integer, Tablet integer)";
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
        //query = "CREATE TABLE Classroom (idClassroom INTEGER PRIMARY KEY   AUTOINCREMENT, Name varchar(100))";
        //try
        //{
        //    dbcmd = dbconn.CreateCommand(); // create empty command
        //    dbcmd.CommandText = query; // fill the command
        //    reader = dbcmd.ExecuteReader(); // execute command which returns a reader
        //}
        //catch (Exception e)
        //{

        //    EDebug.Log(e);

        //}
        //query = "CREATE TABLE Student (idClassroom INTEGER PRIMARY KEY   AUTOINCREMENT, Name varchar(100),foreign key(id_Classroom) references Classroom(idClassroom))";
        //try
        //{
        //    dbcmd = dbconn.CreateCommand(); // create empty command
        //    dbcmd.CommandText = query; // fill the command
        //    reader = dbcmd.ExecuteReader(); // execute command which returns a reader
        //}
        //catch (Exception e)
        //{

        //    EDebug.Log(e);

        //}
        //query = "CREATE TABLE Session (idSession INTEGER PRIMARY KEY   AUTOINCREMENT, Name varchar(100),foreign key(id_Classroom) references Classroom(idClassroom))";
        //try
        //{
        //    dbcmd = dbconn.CreateCommand(); // create empty command
        //    dbcmd.CommandText = query; // fill the command
        //    reader = dbcmd.ExecuteReader(); // execute command which returns a reader
        //}
        //catch (Exception e)
        //{

        //    EDebug.Log(e);

        //}
        //query = "CREATE TABLE Match (idMatch INTEGER PRIMARY KEY   AUTOINCREMENT, Name varchar(100), Points integer, Tablet integer)";
        //try
        //{
        //    dbcmd = dbconn.CreateCommand(); // create empty command
        //    dbcmd.CommandText = query; // fill the command
        //    reader = dbcmd.ExecuteReader(); // execute command which returns a reader
        //}
        //catch (Exception e)
        //{

        //    EDebug.Log(e);

        //}
#endif
  
    }
    #region Buttons
    

    public void InsertClassButton()
    {
        InsertClass(_tName.text);
    }
    public void DeleteClassButton()
    {
        DeleteClass(_tId.text);
    }
    public void UpdateClassButton()
    {
        UpdateClass(_tId.text,_tName.text);
    }
    public void InsertStudentButton()
    {
        InsertStudent(_tName.text);
    }
    public void DeleteStudentButton()
    {
        DeleteStudent(_tId.text);
    }
    public void UpdateStudentButton()
    {
        UpdateStudent(_tId.text, _tName.text);
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
            _dbcmd.ExecuteScalar();
            _dbconn.Close();
        }
        _infoText.text = "";

        ReaderClass();
    }
    /** 

    * @desc Borra una clase

    * @param string deleteById - El id de la clase que va a ser elimimada 

    */
    private void DeleteClass(string deleteById)
    {
        using (_dbconn = new SqliteConnection(_conn))
        {

            _dbconn.Open(); //Open connection to the database.
            IDbCommand dbcmd = _dbconn.CreateCommand();
            string sqlQuery = "DELETE FROM Classroom where idClassroom = " + deleteById;// table name
            dbcmd.CommandText = sqlQuery;
            IDataReader reader = dbcmd.ExecuteReader();

            dbcmd.Dispose();
            dbcmd = null;
            _dbconn.Close();
            _stateText.text = deleteById + " Delete  Done ";

        }
        _infoText.text = "";
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

            while (reader.Read())
            {
                idreaders = reader.GetInt32(0);

                Namereaders = reader.GetString(1);

                _infoText.text += idreaders + Namereaders + " " + "\n";
                EDebug.Log("Value=" + idreaders + " name =" + Namereaders);
                
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
            _dbcmd = _dbconn.CreateCommand();
            _sqlQuery = string.Format("insert into Student (Name, idClassroom) values (\"{0}\",\"{1}\")", name,_Class );// table name
            _dbcmd.CommandText = _sqlQuery;
            _dbcmd.ExecuteScalar();
            _dbconn.Close();
        }
        _infoText.text = "";
        EDebug.Log("Insert Done  ");
        
        ReaderStudent();
    }
    /** 

* @desc Borra un estudiante de la base

* @param string deleteById - El id del estudiante que va a ser elimimado

*/
    private void DeleteStudent(string deleteById)
    {
        using (_dbconn = new SqliteConnection(_conn))
        {

            _dbconn.Open(); //Open connection to the database.
            IDbCommand dbcmd = _dbconn.CreateCommand();
            string sqlQuery = "DELETE FROM Student where idStudent = " + deleteById;// table name
            dbcmd.CommandText = sqlQuery;
            IDataReader reader = dbcmd.ExecuteReader();


            dbcmd.Dispose();
            dbcmd = null;
            _dbconn.Close();
            _stateText.text = deleteById + " Delete  Done ";

        }
        ReaderStudent();

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
            ReaderStudent();
        }
    }
    /** 

    * @desc Lee toda la tabla Student de la base
    */
    private void ReaderStudent()
    {
        int id_Student_readers, id_Classroom_readers;
        string Namereaders;
        using (_dbconn = new SqliteConnection(_conn))
        {
            _dbconn.Open(); //Open connection to the database.
            IDbCommand dbcmd = _dbconn.CreateCommand();
            string sqlQuery = "SELECT idStudent, Name, idClassroom " + "FROM Student";// table name
            dbcmd.CommandText = sqlQuery;
            IDataReader reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                id_Student_readers = reader.GetInt32(0);
                Namereaders = reader.GetString(1); ;
                id_Classroom_readers = reader.GetInt32(2);
                _infoText.text += id_Student_readers + Namereaders + id_Classroom_readers + " " + "\n";
                EDebug.Log("Value=" + id_Student_readers + " name =" + Namereaders + " Clasa =" + id_Classroom_readers);
            }
            reader.Close();
            reader = null;
            dbcmd.Dispose();
            dbcmd = null;
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
               
                _stateText.text += Name_readers_Search + " - " +  "\n";

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
        _infoText.text = "";
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

                _infoText.text += date + " " + "\n";
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
    #endregion
    #region Table Match
    /** 

    * @desc Registra una partida a la base

    * @param int idStudent - El id del estudiante
    * @param int idGSession- El id de la sesi�n
* @param int idGame- El id del juego al que ha jugado
    * @param int team- El equipo del estudiante
* @param string matchInfo- Informacion de la partida

    */
    private void InsertMatch(int idStudent, int idSession, int idGame, int team, string matchInfo)
    {
        using (_dbconn = new SqliteConnection(_conn))
        {
            _dbconn.Open(); //Open connection to the database.
            _dbcmd = _dbconn.CreateCommand();
            _sqlQuery = string.Format("insert into Match (idStudent, idSession, idLevelConf, team, matchInfo ) values (\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\")", idStudent, idSession, idGame, team, matchInfo);// table name
            _dbcmd.CommandText = _sqlQuery;
            _dbcmd.ExecuteScalar();
            _dbconn.Close();
        }
        _infoText.text = "";
        EDebug.Log("Insert Done  ");

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