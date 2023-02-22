using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGear : MonoBehaviour
{
   public float _scaleIncrease;
   public float _scaleDecrease;

    public int _level;
    [SerializeField]
    Cover_Panel_Series_Difficulty _coverPanelDifficulty;
    Cover_Panel_Series_Difficulty.dataDiffilcuty _currentDataDifficulty;
    // Start is called before the first frame update
    void Start()
    {
        //_coverPanelDifficulty = GetComponent<Cover_Panel_Series_Difficulty>();
        _currentDataDifficulty = _coverPanelDifficulty.GenerateDataDifficulty(_level);
        GameObject newGear;
       newGear= Instantiate(_currentDataDifficulty.possibleGeometry[1], new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
        newGear.transform.localScale = new Vector3(newGear.transform.localScale.x * _scaleIncrease, newGear.transform.localScale.y *_scaleIncrease, newGear.transform.localScale.y *_scaleIncrease);
    }
}
