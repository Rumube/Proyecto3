using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generate_CoverPanel : MonoBehaviour
{
    [Header("Table Configuration")]
    public int _column;

    public int _gapX;

    public int _offsetX;
    public float scale;

    public List<GameObject> _referentList;

    public int _level;

    [SerializeField]
    Cover_Panel_Series_Difficulty _coverPanelDifficulty;
    Cover_Panel_Series_Difficulty.dataDiffilcuty _currentDataDifficulty;
    // Start is called before the first frame update
    void Start()
    {
        _coverPanelDifficulty = GetComponent<Cover_Panel_Series_Difficulty>();
        _currentDataDifficulty = _coverPanelDifficulty.GenerateDataDifficulty(_level);
        GenerateGears();
    }

    // Update is called once per frame
    void Update()
    {

    }
    void GenerateGears()
    {

        int random = Random.Range(0, 2);
        int decrease = _column;
        for (int i = 0; i < _column; i++)
        {
            GameObject newGear;
            newGear = Instantiate(_currentDataDifficulty.possibleGeometry[1], new Vector3((i + _offsetX) * _gapX, 0, 0), Quaternion.identity);
            float scale = i * 0.25f;
            newGear.transform.localScale = new Vector3(newGear.transform.localScale.x * scale, newGear.transform.localScale.y * scale, newGear.transform.localScale.y * scale);
            if (random == 1)//increase
            {

            }
            else//decrease
            {
                //float scale = decrease * 0.25f;
            }

            _referentList.Add(newGear);

        }
        if (random == 1)//increase
        {
            for (int i = _column; i == 0; i--)
            {
                _referentList[i].transform.position = new Vector3((i + _offsetX) * _gapX, 0, 0);

            }
            Debug.Log("decrease");
        }
       
    }
}

    


