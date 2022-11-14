using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviour, INetworkManager
{
    [Header("Game Connection")]
    public string _ip;
    public string _port;
    public static Server server = new Server();

    Client client = new Client();
    public TextMeshProUGUI _idText;
    public TMP_InputField _IPText;
    public TMP_InputField _portText;

    [Header("Students to tablets")]
    public List<Tablet> _studentsToTablets = new List<Tablet>();
    public int _selectedTablet = -10;

    [Header("Minigame timer client")]
    public int _minigameMinutes;
    public int _minigameSeconds;

    [Header("Minigame difficulty server")]
    public int _minigameLevel = -1;

    public static NetworkManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    /// <summary>Forces an update when the client receives a package</summary>
    void Update()
    {
        if (Client._allPackages != null && Client._allPackages.Count > 0)
        {
            Client.DoUpdate();
        }
    }

    /// <summary>Starts a new server and saves the ip and port</summary>
    public void StartServer()
    {
        string[] connectionData = new string[2];

        connectionData = ServiceLocator.Instance.GetService<ServerUtility>().createServer();

        _ip = connectionData[0];
        _port = connectionData[1];

        ServiceLocator.Instance.GetService<IGameManager>().SetServerState(IGameManager.GAME_STATE_SERVER.connection);
    }

    /// <summary>Get the number of tablets that are connected</summary>
    public int GetConnectedTablets()
    {
        return ServiceLocator.Instance.GetService<ServerUtility>()._connectedTablets;
    }

    /// <summary>Get a specific Tablet</summary>
    /// <param name="i">Tablet index</param>
    public Tablet GetTablets(int i)
    {
        return ServiceLocator.Instance.GetService<ServerUtility>()._tablets[i];
    }

    /// <summary>Get if a new tablet is connected</summary>
    public bool GetUpdateConnectedTablets()
    {
        return ServiceLocator.Instance.GetService<ServerUtility>()._updateConnectedTablets;
    }

    /// <summary>Rewrite the variable</summary>
    /// <param name="state">The state of the variable</param>
    public void SetUpdateConnectedTablets(bool state)
    {
        ServiceLocator.Instance.GetService<ServerUtility>()._updateConnectedTablets = state;
    }

    /// <summary>Starts a new client connecting to a server with specific IP and port </summary>
    public void StartClient()
    {
        client.CreateClient(_IPText.text, _portText.text);
    }

    /// <summary>Desactivate connections when the app is closed</summary>
    //private void OnDisable()
    //{
    //    Client.OnDisable();
    //}

    /// <summary>Desactivate connections if she goes back before game connection</summary>
    public void ResetConnections()
    {
        Client.OnDisable();
        ServiceLocator.Instance.GetService<ServerUtility>().ResetConnections();
    }

    public int GetMinigameMinutes()
    {
        return _minigameMinutes;
    }

    public int GetMinigameSeconds()
    {
        return _minigameSeconds;
    }

    public int GetMinigameLevel()
    {
        return _minigameLevel;
    }

    #region AddStudent

    /// <summary>Add or remove childrens on selected tablet</summary>
    /// <param name="student">student data</param>
    /// <param name="add">Add or remove student</param>
    public void AddRemoveChildrenToTablet(Student student, bool add)
    {
        if (add)
        {
            bool selected = false;
            //This causes a bug with buttons, because here is not added if already exists but the button thinks that it is
            foreach (Tablet tablet in _studentsToTablets)
            {
                if (tablet._students.Contains(student))
                {
                    selected = true;
                }
            }
            if (!selected)
            {
                _studentsToTablets[_selectedTablet - 1]._students.Add(student);
            }
        }
        else
        {
            _studentsToTablets[_selectedTablet - 1]._students.Remove(student);
        }

    }
    /// <summary>Check if every tablet has at least one student added</summary>
    public bool CheckIfTabletsHasStudents()
    {
        bool atLeastOneStudent = true;
        foreach (Tablet tablet in _studentsToTablets)
        {
            if (tablet._students.Count == 0)
            {
                atLeastOneStudent = false;
            }
        }
        return atLeastOneStudent;
    }
    #endregion

    #region CallingStudents

    /// <summary>It's called when every student has been called to the rocket</summary>
    public void SendEndCalling()
    {
        client.EndCallingStudents();
    }
    /// <summary>It's called when a student and game has been selected to play</summary>
    public void SendStudentGame()
    {
        client.StudentGameSelection(ServiceLocator.Instance.GetService<IGameManager>().GetCurrentStudentName(), ServiceLocator.Instance.GetService<IGameManager>().GetCurrentGameName());
    }
    #endregion

    #region FinalScore
    public void SendViewingFinalScore()
    {
        client.ViewFinalScore();
    }
    #endregion

    #region Scores
    public void SendMatchData()
    {
        client.StudentScore();
    }
    #endregion


    public void SetIp(string value)
    {
        _ip = value;
    }
    public string GetIp()
    {
        return _ip;
    }
    public void SetPort(string value)
    {
        _port = value;
    }
    public string GetPort()
    {
        return _port;
    }
    public void SetMinigamesMinutes(int minutes)
    {
        _minigameMinutes = minutes;
    }
    public int GetMinigamesMinutes()
    {
        return _minigameMinutes;
    }
    public void SetMinigamesSeconds(int seconds)
    {
        _minigameSeconds = seconds;
    }
    public int GetMinigamesSeconds()
    {
        return _minigameSeconds;
    }
    public void SetMinigamesLevel(int level)
    {
        _minigameLevel = level;
    }
    public int GetMinigamesLevel()
    {
        return _minigameLevel;
    }
    public void SetSelectedTablet(int selectedTablet)
    {
        _selectedTablet = selectedTablet;
    }
    public int GetSelectedTablet()
    {
        return _selectedTablet;
    }
    public void SetStudentToTablet(Tablet tablet)
    {
        _studentsToTablets.Add(tablet);
    }
    public List<Tablet> GetStudentsToTablets()
    {
        return _studentsToTablets;
    }

    public void RandomizeStudents()
    {
        //Utilizar shuffle para barajar los alumnos y luego añadirlos :)

        GameObject[] PresentStudents = GameObject.FindGameObjectsWithTag("StudentButton");
        List<GameObject> studentsList = new List<GameObject>(PresentStudents);


        System.Random random = new System.Random();
        int n = studentsList.Count;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            GameObject value = studentsList[k];
            studentsList[k] = studentsList[n];
            studentsList[n] = value;
        }

        int currentTablet = 0;
        foreach (GameObject currentStudent in PresentStudents)
        {
            _selectedTablet = currentTablet;
            AddRemoveChildrenToTablet(currentStudent.GetComponent<StudentButton>()._student, true);
            currentTablet++;
            if(currentTablet >= ServiceLocator.Instance.GetService<ServerUtility>()._tablets.Length)
            {
                currentTablet = 0;
            }
        }

    }
    //[Header("Game Connection")]
    //public string _ip;
    //public string _port;
    //public static Server server = new Server();

    //Client client = new Client();
    //public TextMeshProUGUI _idText;
    //public TMP_InputField _IPText;
    //public TMP_InputField _portText;

    //[Header("Students to tablets")]
    //public List<Tablet> _studentsToTablets = new List<Tablet>();
    //public int _selectedTablet = -10;

    //[Header("Minigame timer client")]
    //public int _minigameMinutes;
    //public int _minigameSeconds;

    //[Header("Minigame difficulty server")]
    //public int _minigameLevel = -1;

    //public static NetworkManager Instance;
}
