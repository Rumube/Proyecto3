using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviour
{

    [Header("Game Connection")]
    public string _ip;
    public string _port;
    Server server;

    Client client;
    public Text _idText;
    //Testing 
    public InputField _IPTest;
    public InputField _portTest;

    void Update()
    {
        //Not finished
        if (Client._allPackages != null && Client._allPackages.Count > 0)
        {
            Client.DoUpdate();
            _idText.text = Client._tablet._id.ToString();
        }
    }

    /// <summary>Starts a new server and provide the ip and port's device</summary>
    public void StartServer()
    {
        string[] connectionData = new string[2];

        server = new Server();
        connectionData = server.createServer();

        _ip = connectionData[0];
        _port = connectionData[1];

        ServiceLocator.Instance.GetService<GameManager>()._gameStateServer = GameManager.GAME_STATE_SERVER.connection;
    }

    /// <summary>Get the number of tablets that are connected</summary>
    public int GetConnectedTablets()
    {
        return Server._connectedTablets;
    }

    /// <summary>Get a specific Tablet</summary>
    /// <param name="i">Tablet index</param>
    public Tablet GetTablets(int i)
    {
        return Server._tablets[i];
    }

    /// <summary>Get if a new tablet is connected</summary>
    public bool GetUpdateConnectedTablets()
    {
        return Server._updateConnectedTablets;
    }

    /// <summary>Rewrite the variable</summary>
    /// <param name="state">The state of the variable</param>
    public void SetUpdateConnectedTablets(bool state)
    {
        Server._updateConnectedTablets = state;
    }

    /// <summary>Starts a new client connecting to a server with specific IP and port </summary>
    public void StartClient()
    {
        client = new Client();
        client.CreateClient(_IPTest.text, _portTest.text);
    }


    /// <summary>Desactivate connections when the app is closed</summary>
    private void OnDisable()
    {
        Server.OnDisable();
        Client.OnDisable();
    }

    /// <summary>Desactivate connections if she goes back before game connection</summary>
    public void ResetConnections()
    {
        Server.OnDisable();
        Client.OnDisable();
    }

}
