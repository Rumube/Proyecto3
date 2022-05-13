using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditsButtons : MonoBehaviour
{
    public GameObject _creditText;
    int _showingValue;

    // Start is called before the first frame update
    void Start()
    {
        _creditText.SetActive(false);
        _showingValue = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowMessage()
    {
        if (_showingValue == 0)
        {
            _creditText.SetActive(true);
            _showingValue = 1;
        }
        else 
        {
            _creditText.SetActive(false);
            _showingValue = 0;
        }
    }

}
