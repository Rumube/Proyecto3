using SQLite4Unity3d;
using UnityEngine;
#if !UNITY_EDITOR
using System.Collections;
using System.IO;
#endif
using System.Collections.Generic;

public class DataService
{
    private SQLiteConnection _connection;

    public DataService(string DatabaseName)
    {

#if UNITY_EDITOR
        var dbPath = string.Format(@"Assets/StreamingAssets/{0}", DatabaseName);
#else
        // check if file exists in Application.persistentDataPath
        var filepath = string.Format("{0}/{1}", Application.persistentDataPath, DatabaseName);

        if (!File.Exists(filepath))
        {
            Debug.Log("Database not in Persistent path");
            // if it doesn't ->
            // open StreamingAssets directory and load the db ->

#if UNITY_ANDROID
            var loadDb = new WWW("jar:file://" + Application.dataPath + "!/assets/" + DatabaseName);  // this is the path to your StreamingAssets in android
            while (!loadDb.isDone) { }  // CAREFUL here, for safety reasons you shouldn't let this while loop unattended, place a timer and error check
            // then save to Application.persistentDataPath
            File.WriteAllBytes(filepath, loadDb.bytes);
#elif UNITY_IOS
                 var loadDb = Application.dataPath + "/Raw/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);
#elif UNITY_WP8
                var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);

#elif UNITY_WINRT
		var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
		// then save to Application.persistentDataPath
		File.Copy(loadDb, filepath);
		
#elif UNITY_STANDALONE_OSX
		var loadDb = Application.dataPath + "/Resources/Data/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
		// then save to Application.persistentDataPath
		File.Copy(loadDb, filepath);
#else
	var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
	// then save to Application.persistentDataPath
	File.Copy(loadDb, filepath);

#endif

            Debug.Log("Database written");
        }

        var dbPath = filepath;
#endif
        _connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
        Debug.Log("Final PATH: " + dbPath);

    }
    /// <summary>
    /// Close the database connection
    /// </summary>
    public void CloaseDatabase()
    {
        _connection.Close();
    }
    /// <summary>
    /// Create the database if not exist
    /// </summary>
    public void CreateDB()
    {
        _connection.CreateTable<StudentDB>();
        _connection.CreateTable<ClassroomDB>();
        _connection.CreateTable<SessionDB>();
        _connection.CreateTable<GameDB>();
        _connection.CreateTable<MatchDB>();
    }
    #region Classroom
    /// <summary>
    /// Create a Classroom in thedatabase
    /// </summary>
    /// <param name="name">The Classroom's name</param>
    /// <returns>The created Classroom</returns>
    public ClassroomDB InsertClass(string name)
    {
        ClassroomDB newClassroom = new ClassroomDB
        {
            name = name.ToUpper()
        };
        try
        {
            _connection.Insert(newClassroom);
            ServiceLocator.Instance.GetService<MobileUI>().PopupAddClass();
            return newClassroom;
        }
        catch (System.Exception)
        {
            ServiceLocator.Instance.GetService<MobileUI>().AddingTwoClassesWithSameName();
            return null;
        }
    }
    /// <summary>
    /// Delete classroom using the name
    /// </summary>
    /// <param name="name">The classroom's name</param>
    public void DeleteClass(string name)
    {
        _connection.Delete(GetClass(name.ToLower()));
    }
    /// <summary>
    /// Returns the classroom using the name
    /// </summary>
    /// <param name="name">The classroom's name</param>
    /// <returns><see cref="IEnumerable{ClassroomDB}"/></returns>
    public ClassroomDB GetClass(string name)
    {
        ClassroomDB value = null;
        IEnumerable<ClassroomDB> classrooms = _connection.Table<ClassroomDB>().Where(x => x.name == name.ToUpper());
        foreach (ClassroomDB classroom in classrooms)
        {
            value = classroom;
        }
        return value;
    }
    /// <summary>
    /// Returns the classroom using the id
    /// </summary>
    /// <param name="id">The classroom's id</param>
    /// <returns><see cref="IEnumerable{ClassroomDB}"/></returns>
    public IEnumerable<ClassroomDB> GetClass(int id)
    {
        return _connection.Table<ClassroomDB>().Where(x => x.idClassroom == id);
    }
    /// <summary>
    /// Get all Classroom table
    /// </summary>
    /// <returns>All Classrooms rows <see cref="IEnumerable{ClassroomDB}"/></returns>
    public IEnumerable<ClassroomDB> GetAllClassrooms()
    {
        return _connection.Table<ClassroomDB>();
    }
    #endregion
    #region Student
    /// <summary>
    /// Create a Student in thedatabase
    /// </summary>
    /// <param name="name">The Student's name</param>
    /// <returns>The created Student</returns>
    public StudentDB InsertStudent(string name)
    {
        //GET CURRENT CLASSROOM
        ClassroomDB currentClassroom  = GetClass(ServiceLocator.Instance.GetService<UIManager>()._classNamedb);
        int idClassroom = currentClassroom.idClassroom;

        StudentDB newStudent = new StudentDB
        {
            name = name.ToUpper(),
            idClassroom = idClassroom
        };
        try
        {
            _connection.Insert(newStudent);
            ServiceLocator.Instance.GetService<MobileUI>().PopupAddStudent();
            return newStudent;
        }
        catch (System.Exception)
        {
            ServiceLocator.Instance.GetService<MobileUI>().AddingTwoStudentsWithSameName();
            return null;
        }
    }
    /// <summary>
    /// Get all Student table
    /// </summary>
    /// <returns>All Classrooms rows <see cref="IEnumerable{StudentDB}"/></returns>
    public IEnumerable<StudentDB> GetAllStudent()
    {
        return _connection.Table<StudentDB>();
    }
    /// <summary>
    /// Returns the student using the name
    /// </summary>
    /// <param name="name">The student's name</param>
    /// <returns><see cref="StudentDB"/></returns>
    public StudentDB GetStudent(string name)
    {
        StudentDB result = null;

        IEnumerable<StudentDB> students = _connection.Table<StudentDB>().Where(x => x.name == name.ToUpper());

        foreach (StudentDB student in students)
        {
            result = student;
        }
        return result;
    }
    /// <summary>
    /// Returns the student using the idClassroom
    /// </summary>
    /// <param name="idClassroom">The student's idClassroom</param>
    /// <returns><see cref="IEnumerable{StudentDB}"/></returns>
    public IEnumerable<StudentDB> GetStudent(int idClassroom)
    {
        return _connection.Table<StudentDB>().Where(x => x.idClassroom == idClassroom);
    }
    /// <summary>
    /// Returns the student using the idStudent, needs the bool
    /// </summary>
    /// <param name="idStudent">The student's id</param>
    /// <param name="isId">Needs the bool true or false</param>
    /// <returns></returns>
    public StudentDB GetStudent(int idStudent, bool isId)
    {
        StudentDB result = null;

        IEnumerable<StudentDB> students = _connection.Table<StudentDB>().Where(x => x.idStudent == idStudent);

        foreach (StudentDB student in students)
        {
            result = student;
        }
        return result;
    }
    /// <summary>
    /// Get All students from the same classroom
    /// </summary>
    /// <param name="idClassroom">id's classroom</param>
    public IEnumerable<StudentDB> SeachStudent(string idClassroom)
    {
        return _connection.Table<StudentDB>().Where(x => x.idClassroom == int.Parse(idClassroom));
    }
    /// <summary>
    /// Delete student using the name
    /// </summary>
    /// <param name="name">The student's name</param>
    public void DeleteStudent(string name)
    {
        _connection.Delete(GetStudent(name.ToUpper()));
    }
    /// <summary>
    /// Delete student using the idStudent
    /// </summary>
    /// <param name="idStudent">The student's id</param>
    public void DeleteStudent(int idStudent)
    {
        _connection.Delete(GetStudent(idStudent, true));
    }
    #endregion
    #region Session
    /// <summary>
    /// Create a Session in the database
    /// </summary>
    /// <returns>The session created</returns>
    public SessionDB InsertSession()
    {
        string dateNow = System.DateTime.Now.ToString();
        SessionDB newSession = new SessionDB
        {
            dateSession = dateNow
        };

        try
        {
            _connection.Insert(newSession);
            return newSession;
        }
        catch (System.Exception)
        {
            return null;
        }
    }
    /// <summary>
    /// Returns all the sessions
    /// </summary>
    /// <returns>All Sessions rows <see cref="IEnumerable{SessionDB}"/></returns>
    public IEnumerable<SessionDB> GetAllSessions()
    {
        return _connection.Table<SessionDB>();
    }
    /// <summary>
    /// Returns the last <see cref="SessionDB"/> order by the idSession
    /// </summary>
    /// <returns><see cref="SessionDB"/></returns>
    public SessionDB GetSessionOrderBy()
    {
        SessionDB result = null;
        IEnumerable<SessionDB> sessions = _connection.Query<SessionDB>("SELECT idSession FROM SessionDB ORDER BY idSession DESC LIMIT 1");
        foreach (SessionDB session in sessions)
        {
            result = session;
        }
        return result;
    }
    #endregion
    #region Game
    /// <summary>
    /// Returns the game info using the <see cref="GameDB.name"/>
    /// </summary>
    /// <param name="nameGame"></param>
    /// <returns><see cref="GameDB"/></returns>
    public GameDB GetGame(string nameGame)
    {
        GameDB result = null;
        IEnumerable<GameDB> games = _connection.Query<GameDB>("SELECT * FROM GameDB where name = ?", nameGame);
        foreach (GameDB game in games)
        {
            result = game;
        }
        return result;
    }
    #endregion
    #region Match
    /// <summary>
    /// Inserts a new Match in the table
    /// </summary>
    /// <param name="idStudent">Student's ID</param>
    /// <param name="idSession">Session's ID</param>
    /// <param name="idGame">Game's ID</param>
    /// <param name="team">Number's team</param>
    /// <param name="level">Level's game</param>
    /// <param name="averageSuccess">Average of success in the match</param>
    /// <param name="averageErrors">Average of errors in the match</param>
    /// <param name="averagePoints">Average of points int the match</param>
    /// <param name="averageTime">Time to complet the game</param>
    /// <returns>The created match <see cref="MatchDB"/></returns>
    public MatchDB InsertMatch(int idStudent, int idSession, int idGame, int team, int level, int averageSuccess, int averageErrors, int averagePoints, float averageTime)
    {
        MatchDB newMatch = new MatchDB
        {
            idStudent = idStudent,
            idSession = idSession,
            idGame = idGame,
            team = team,
            level = level,
            averageSuccess = averageSuccess,
            averageErrors = averageErrors,
            averagePoints = averagePoints,
            averageTime = averageTime
        };

        try
        {
            _connection.Insert(newMatch);
            return newMatch;
        }
        catch (System.Exception)
        {
            return null;
        }
    }
    /// <summary>
    /// Returns the last <see cref="MatchDB"/> using the <see cref="MatchDB.idStudent"/> and the <see cref="MatchDB.idGame"/>
    /// </summary>
    /// <param name="idStudent">The student's id</param>
    /// <param name="idGame">The game's id</param>
    /// <returns><see cref="MatchDB"/></returns>
    public MatchDB GetMatchDifficultyData(int idStudent, int idGame)
    {
        MatchDB result = null;
        IEnumerable<MatchDB> matches = _connection.Query<MatchDB>("SELECT * FROM MatchDB where idStudent = ? AND idGame = ? LIMIT 1", idStudent, idGame);
        foreach (MatchDB match in matches)
        {
            result = match;
        }
        return result;
    }
    #endregion
}
