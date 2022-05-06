using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ButtonsManager : MonoBehaviour
{

    [Header("Configuration")]
    public int _finalNumber;
    public Text _text;

    // Start is called before the first frame update
    void Start()
    {
        _finalNumber = 0;
        _text.text = "" + _finalNumber;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Buttons   

    public void AddNumber(Text currentTextAdd)
    {
        _finalNumber = (int.Parse(currentTextAdd.text)+1);
        currentTextAdd.text = "" + _finalNumber;
    }

    public void SubNumber(Text currentTextSub)
    {
        _finalNumber = (int.Parse(currentTextSub.text)-1);
        if (_finalNumber <= 0)
        {
            _finalNumber = 0;
        }
        currentTextSub.text = "" + _finalNumber;
    }
    #endregion Buttons


}
