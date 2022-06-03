using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gears_Series_Manager : MonoBehaviour
{
    public int _increase;
    public int _level;
    public GameObject guide;
    [SerializeField]
    Cover_Panel_Series_Difficulty _coverPanelDifficulty;
    Cover_Panel_Series_Difficulty.dataDiffilcuty _currentDataDifficulty;
    void Start()
    {
        _coverPanelDifficulty = GetComponent<Cover_Panel_Series_Difficulty>();
        _currentDataDifficulty = _coverPanelDifficulty.GenerateDataDifficulty(_level);
        guide.SetActive(_currentDataDifficulty.guide);
        int _increase = Random.Range(0, 2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
