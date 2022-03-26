using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabletUI : UI
{
    public InputField _ipServer;
    public InputField _portServer;
    // Start is called before the first frame update
    void Start()
    {
        //Get the last info saved
        if (PlayerPrefs.HasKey("IPServer") && PlayerPrefs.HasKey("PortServer"))
        {
            _ipServer.text = PlayerPrefs.GetString("IPServer");
            _portServer.text = PlayerPrefs.GetString("PortServer");
        }       
    }
    /// <summary>
    /// Save the values places in the input in order to have direct access to the connection if
    /// server port and ip doesn't change
    /// </summary>
    public void SaveIPPortInfo()
    {
        if(_ipServer.text != PlayerPrefs.GetString("IPServer"))
        {
            PlayerPrefs.SetString("IPServer", _ipServer.text);
        }
        if (_portServer.text != PlayerPrefs.GetString("PortServer"))
        {
            PlayerPrefs.SetString("PortServer", _portServer.text);
        }      
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
