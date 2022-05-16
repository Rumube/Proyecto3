using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class RankingServer : MonoBehaviour
{
    public TextMeshProUGUI _minPoints;
    public TextMeshProUGUI _maxPoints;
    float maxPoints = 0;
    float minPoints = 99999;

    [Header("Test")]
    public bool _newDataFromClient;
    public bool _generateGrid;
    public TestingRankings _testingRankings;

    [Header("References")]
    public Transform _lowerLevel;
    public Transform _upperLevel;
    public Transform _rankingPanel;
    public float _differenceBetweenLowerUpper;
    public GameObject _rankingGridParent;

    [Header("Transform References")]
    public List<Transform> _rocketsTransforms;
    public List<Transform> _rankingPositions;

    private float _maxPointsInt = 0;
    private float _minPointsInt = 99999;

    public GameObject[,] _gridPositions;
    private int _numTotalTeams;
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
        if (_generateGrid)
        {
            _generateGrid = false;
            CreateGrid();
        }
    }

    public void CreateGrid()
    {

        foreach (Transform team in _rocketsTransforms)
        {
            team.gameObject.SetActive(false);
        }

        List<GameObject> teamParentList = new List<GameObject>();
        int numTeam = 1;

        foreach (KeyValuePair<int, float> team in _testingRankings._teamPoints)
        {
            GameObject teamParent = new GameObject();
            teamParent.name = "Team_" + numTeam;
            teamParentList.Add(teamParent);
            _rocketsTransforms[numTeam - 1].gameObject.SetActive(true);
            _rocketsTransforms[numTeam - 1].gameObject.transform.position = teamParent.GetComponentInChildren<Transform>().position;
            numTeam++;
            teamParent.transform.SetParent(_rankingGridParent.transform);
            teamParent.AddComponent<VerticalLayoutGroup>();
            teamParent.GetComponent<VerticalLayoutGroup>().spacing = 80;
            teamParent.GetComponent<VerticalLayoutGroup>().reverseArrangement = true;
            teamParent.transform.localScale = new Vector3(1, 1, 1);
        }
        _numTotalTeams = numTeam - 1;
        _gridPositions = new GameObject[numTeam-1, 11];   
        for (int i = 0; i < teamParentList.Count; i++)
        {
            for (int j = 0; j < 11; j++)
            {
                GameObject cellInGrid = new GameObject();
                cellInGrid.name = "Team" + i + "_Pos" + j;
                cellInGrid.transform.SetParent(teamParentList[i].transform);
                cellInGrid.AddComponent<RectTransform>();
                _gridPositions[i, j] = cellInGrid;
                print(_gridPositions[i, j].GetComponent<RectTransform>().position);
            }
        }
        int currentTeam = 0;
        //foreach (GameObject currentTeamParent in teamParentList)
        //{
        //    currentTeamParent.transform.position = _gridPositions[currentTeam, 0].transform.position;
        //    currentTeam++;
        //}

        foreach (KeyValuePair<int, float> team in _testingRankings._teamPoints)
        {
            print(_gridPositions[currentTeam, 0]);
            _rocketsTransforms[team.Key].transform.position = _gridPositions[currentTeam, 0].transform.position;
            currentTeam++;
        }
        StartCoroutine(setGrid());
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

            positionYRelative = (Mathf.Round(positionYRelative * 10.0f) * 0.1f) * 10;

            int positionY = (int)positionYRelative;
            print("Pos: " + positionY);

            _rocketsTransforms[team.Key].transform.position = new Vector2(_rocketsTransforms[team.Key].transform.position.x, _rankingPositions[positionY].position.y);

            //float positionY = ((_lowerLevel.position.y + positionYRelative) - _lowerLevel.position.y) / (_upperLevel.position.y - _lowerLevel.position.y);

            //_rocketsTransforms[team.Key].transform.position = new Vector3(_rocketsTransforms[team.Key].transform.position.x, _rocketsTransforms[team.Key].transform.position.y + positionY, _rocketsTransforms[team.Key].transform.position.z);
        }
        
    }

    IEnumerator setGrid()
    {
        yield return new WaitForSeconds(0.5f);
        print("Count: " + _numTotalTeams);
        for (int i = 0; i < _numTotalTeams; i++)
        {
            print("Pos: " + i);
            _rocketsTransforms[i].transform.position = _gridPositions[i, 0].GetComponent<RectTransform>().position;
        }
    }
}
