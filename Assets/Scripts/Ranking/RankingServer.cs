using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class RankingServer : MonoBehaviour
{
    public TextMeshProUGUI _minPoints;
    public TextMeshProUGUI _maxPoints;
    float maxPoints = 0;
    float minPoints = 99999;

    public bool _newDataFromClient;
    public TestingRankings _testingRankings;

    public Transform _lowerLevel;
    public Transform _upperLevel;
    public Transform _rankingPanel;
    public float _differenceBetweenLowerUpper;

    public List<Transform> _rocketsTransforms; 

    private float _maxPointsInt = 0;
    private float _minPointsInt = 99999;
    // Start is called before the first frame update
    void Start()
    {
        _minPoints.text = 0.ToString();
        _maxPoints.text = 0.ToString();

        _differenceBetweenLowerUpper = _upperLevel.position.y - _lowerLevel.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (_newDataFromClient)
        {
            _newDataFromClient = false;
            UpdateRankingPoints();
        }
    }
    void UpdateRankingPoints()
    {

        if (float.TryParse(_maxPoints.text,out maxPoints))
        {
            if (float.TryParse(_minPoints.text, out minPoints))
            {
                foreach (KeyValuePair<int, float> team in _testingRankings._teamPoints)
                {
                    int key = team.Key;
                    float value = team.Value;

                    if (value < _minPointsInt)
                    {
                        minPoints = value;
                        _minPointsInt = value;
                    }
                    if (value > _maxPointsInt)
                    {
                        maxPoints = value;
                        _maxPointsInt = value;
                    }
                }
            }
        }
        _minPoints.text = minPoints.ToString();
        _maxPoints.text = maxPoints.ToString();

        CalculateRocketsPosition();
    }

    void CalculateRocketsPosition()
    {
        foreach(KeyValuePair<int, float> team in _testingRankings._teamPoints)
        {
            float positionYRelative = (team.Value - minPoints) / (maxPoints - minPoints);

            float positionY = ((_lowerLevel.position.y + positionYRelative) - _lowerLevel.position.y) / (_upperLevel.position.y - _lowerLevel.position.y);

            _rocketsTransforms[team.Key].transform.position = new Vector3(_rocketsTransforms[team.Key].transform.position.x, _rocketsTransforms[team.Key].transform.position.y + positionY, _rocketsTransforms[team.Key].transform.position.z);
        }
        
    }
}
