using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CreditsButtons : MonoBehaviour
{
    public GameObject _creditPanel;
    //public Animator _panelAnim;
    public string _eachText;
    public TMP_Text _panelText;
    int _showingValue;

    // Start is called before the first frame update
    void Start()
    {
        _creditPanel.SetActive(false);
        _showingValue = 0;
       _panelText.text = _eachText;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowMessage()
    {
        if (_showingValue == 0)
        {
            _creditPanel.SetActive(true);
            _showingValue = 1;
            //_panelAnim.Play("Appear");
        }
        else 
        {
            _creditPanel.SetActive(false);
            _showingValue = 0;
            //_panelAnim.Play("Disappear");
        }
    }

}
