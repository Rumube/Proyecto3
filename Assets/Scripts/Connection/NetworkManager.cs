using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviour
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

        ServiceLocator.Instance.GetService<GameManager>()._gameStateServer = GameManager.GAME_STATE_SERVER.connection;
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
    private void OnDisable()
    {
        Client.OnDisable();
    }

    /// <summary>Desactivate connections if she goes back before game connection</summary>
    public void ResetConnections()
    {
        Client.OnDisable();
        ServiceLocator.Instance.GetService<ServerUtility>().ResetConnections();
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
        client.StudentGameSelection(ServiceLocator.Instance.GetService<GameManager>()._currentstudentName, ServiceLocator.Instance.GetService<GameManager>()._currentgameName);
    }
    #endregion

}
