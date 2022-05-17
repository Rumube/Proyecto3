using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingRankings : MonoBehaviour
{

    public List<float> _puntos;
    public bool _testPoints;

    public Dictionary<int, float> _teamPoints = new Dictionary<int, float>();

    // Update is called once per frame
    void Update()
    {
        if (_testPoints)
        {
            _teamPoints.Clear();
            _testPoints = false;
            int equipo = 0;
            foreach (float punto in _puntos)
            {
                _teamPoints.Add(equipo, punto);
                equipo++;
            }
        }
    }
}
