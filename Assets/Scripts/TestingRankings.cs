using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingRankings : MonoBehaviour
{

    public List<float> _puntos;

    public Dictionary<int, float> _teamPoints = new Dictionary<int, float>();

    private void Start()
    {
        int equipo = 0;
        foreach (float punto in _puntos)
        {
            _teamPoints.Add(equipo, punto);
            equipo++;
        }
        StartCoroutine(GetComponent<RankingClient>().CreateGrid());
    }

    // Update is called once per frame
    void Update()
    {

            //_teamPoints.Clear();
            //int equipo = 0;
            //foreach (float punto in _puntos)
            //{
            //    _teamPoints.Add(equipo, punto);
            //    equipo++;
            //}
    }
}
