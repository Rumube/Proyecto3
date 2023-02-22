using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public GameObject _eachScreen;
    int _showingValue;
    public GameObject ClickSound;

    void Start()
    {
        _eachScreen.SetActive(false);
        _showingValue = 0;
    }
    /// <summary>
    /// Play a clip
    /// </summary>
    public void MakeSound()
    {
        GetComponent<AudioSource>().Play();
    }
    /// <summary>
    /// Show/Hide windows
    /// </summary>
    public void ShowScreen()
    {
        if (_showingValue == 0)
        {
            _eachScreen.SetActive(true);
            _showingValue = 1;
        }
        else
        {
            _eachScreen.SetActive(false);
            _showingValue = 0;
        }
    }
    /// <summary>
    /// Returns to previous windows
    /// </summary>
    public void BackScreen()
    {
        _eachScreen.SetActive(false);
        _showingValue = 0;
    }
}
