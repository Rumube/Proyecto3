using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviour
{

    [Header("Game Connection")]
    public string _ip;
    public string _port;
    public static Server server = new Server();

    Client client = new Client();
    public Text _idText;
    //Testing 
    public InputField _IPTest;
    public InputField _portTest;

    [Header("Students to tablets")]
    public List<Tablet> _studentsToTablets = new List<Tablet>();
    public int _selectedTablet = -10;

    [Header("Minigame timer client")]
    public int _minigameMinutes;
    public int _minigameSeconds;

    [Header("Minigame difficulty server")]
    public int _minigameLevel = -1;

    void Update()
    {

        //Not finished
        if (Client._allPackages != null && Client._allPackages.Count > 0)
        {
            Client.DoUpdate();
            //_idText.text = ((TabletUI.TEAMCOLOR)Client._tablet._id).ToString();
        }

        //if (Server._allPackages != null && Server._allPackages.Count > 0)
        //{
        //    server.DoUpdate();
        //}

            //server.DoUpdate();
        
    }

    /// <summary>Starts a new server and provide the ip and port's device</summary>
    public void StartServer()
    {
        string[] connectionData = new string[2];

        //server = new Server();
        EDebug.Log(1 + " " + server.State);
        connectionData = ServiceLocator.Instance.GetService<ServerUtility>().createServer();
        EDebug.Log(3 + " " + server.State);

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
        //client = new Client();
        client.CreateClient(_IPTest.text, _portTest.text);
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
    public void AddingStudentsToTablets()
    {
        // EDebug.Log(server.Context);
        //server.AddingStudents();
        // EDebug.Log(server.Context);
        //Server._sendAddStudents = true;
    }

    public void SendEndCalling()
    {
        client.EndCallingStudents();
    }

    public void SendStudentGame()
    {
        client.StudentGameSelection(ServiceLocator.Instance.GetService<GameManager>()._currentstudentName, ServiceLocator.Instance.GetService<GameManager>()._currentgameName);
    }

    #endregion
}
