using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CreditsButtons : MonoBehaviour
{
    public GameObject _creditPanel;
    int _showingValue;

    public GameObject ClickSound;

    public TMP_Text _panelText;
    public string _eachText;


    void Start()
    {
        _creditPanel.SetActive(false);
        _showingValue = 0;
       _panelText.text = _eachText;
    }

    void Update()
    {
     
    }
    public void MakeSound()
    {
        GetComponent<AudioSource>().Play();
    }

    public void ShowMessage()
    {
        if (_showingValue == 0)
        {
            _creditPanel.SetActive(true);
            _showingValue = 1;
        }
        else 
        {
            _creditPanel.SetActive(false);
            _showingValue = 0;
        }
    }
}
