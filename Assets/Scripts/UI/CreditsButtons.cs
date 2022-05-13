using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CreditsButtons : MonoBehaviour
{
    public GameObject _creditPanel;
    public string _eachText;
    public TMP_Text _panelText;
    int _showingValue;

    void Start()
    {
        _creditPanel.SetActive(false);
        _showingValue = 0;
       _panelText.text = _eachText;
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
