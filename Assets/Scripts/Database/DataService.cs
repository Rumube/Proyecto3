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

    public void CreateDB()
    {
        _connection.CreateTable<StudentDB>();
        _connection.CreateTable<ClassroomDB>();
        _connection.CreateTable<SessionDB>();
        _connection.CreateTable<GameDB>();
        _connection.CreateTable<MatchDB>();

        _connection.InsertAll(new[]{
            new StudentDB{
                name = "Tom",
                idClassroom = 1,
            },
            new StudentDB{
                name = "Juan",
                idClassroom = 1,
            },
            new StudentDB{
                name = "Marta",
                idClassroom = 1,
            },
            new StudentDB{
                name = "Lola",
                idClassroom = 1,
            }
        });
    }
    #region examples
    public IEnumerable<StudentDB> GetPersons()
    {
        return _connection.Table<StudentDB>();
    }

    public IEnumerable<StudentDB> GetPersonsNamedRoberto()
    {
        return _connection.Table<StudentDB>().Where(x => x.name == "Roberto");
    }

    public StudentDB GetJohnny()
    {
        return _connection.Table<StudentDB>().Where(x => x.name == "Johnny").FirstOrDefault();
    }

    public StudentDB CreatePerson()
    {
        var p = new StudentDB
        {
            name = "Olivia",
            idClassroom = 10,
        };
        _connection.Insert(p);
        return p;
    }
    #endregion
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
        _connection.Delete(GetClass(name.ToUpper()));
    }
    /// <summary>
    /// Returns the classroom using the name
    /// </summary>
    /// <param name="name">The classroom's name</param>
    /// <returns><see cref="IEnumerable{ClassroomDB}"/></returns>
    public IEnumerable<ClassroomDB> GetClass(string name)
    {
        return _connection.Table<ClassroomDB>().Where(x => x.name == name.ToUpper());
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
        ClassroomDB currentClassroom  = (ClassroomDB)GetClass(ServiceLocator.Instance.GetService<UIManager>()._classNamedb);
        int idClassroom = currentClassroom.idClassroom;

        StudentDB newStudent = new StudentDB
        {
            name = name.ToUpper(),
            idClassroom = idClassroom
        };
        try
        {
            _connection.Insert(newStudent);
            ServiceLocator.Instance.GetService<MobileUI>().PopupAddClass();
            return newStudent;
        }
        catch (System.Exception)
        {
            ServiceLocator.Instance.GetService<MobileUI>().AddingTwoClassesWithSameName();
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
    /// <returns><see cref="IEnumerable{StudentDB}"/></returns>
    public IEnumerable<StudentDB> GetStudent(string name)
    {
        return _connection.Table<StudentDB>().Where(x => x.name == name.ToUpper());
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
    public IEnumerable<StudentDB> GetStudent(int idStudent, bool isId)
    {
        return _connection.Table<StudentDB>().Where(x => x.idStudent == idStudent);
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
            ServiceLocator.Instance.GetService<MobileUI>().PopupAddClass();
            return newSession;
        }
        catch (System.Exception)
        {
            ServiceLocator.Instance.GetService<MobileUI>().AddingTwoClassesWithSameName();
            return null;
        }
    }
    #endregion
}
