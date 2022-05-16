using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingRankings : MonoBehaviour
{
    public Dictionary<int, float> _teamPoints = new Dictionary<int, float>();
    // Start is called before the first frame update
    void Start()
    {
        _teamPoints.Add(0, 34f);
        _teamPoints.Add(1, 70f);
        _teamPoints.Add(2, 130f);
        _teamPoints.Add(3, 40f);
        _teamPoints.Add(4, 100f);
        //_teamPoints.Add(5, 80);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
