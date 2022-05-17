using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class RankingServer : MonoBehaviour
{
    public TextMeshProUGUI _minPoints;
    public TextMeshProUGUI _maxPoints;

    public float maxPoints = 0;
    public float minPoints = 99999;

    public float _currentMaxPoints = 0;
    public float _currentMinPoints = 0;

    public float _lastMaxPoints = 0;
    public float _lastMinPoints = 0;

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

    private bool _rankingStarted = false;
    // Start is called before the first frame update
    void Start()
    {
        _minPoints.text = 0.ToString();
        _maxPoints.text = 0.ToString();

        //_differenceBetweenLowerUpper = _upperLevel.position.y - _lowerLevel.position.y;
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


        //CHANGE VISUAL POINTS
        if (_currentMaxPoints < maxPoints && _rankingStarted)
        {
            _currentMaxPoints++;
            _maxPoints.text = _currentMaxPoints.ToString();
        }
        else
        {
            _lastMaxPoints = maxPoints;
        }
        if (_currentMinPoints < minPoints && _rankingStarted)
        {
            _currentMinPoints++;
            _minPoints.text = _currentMinPoints.ToString();
        }
        else
        {
            _lastMinPoints = minPoints;
        }

    }

    /// <summary>
    /// Creates the ranking grid
    /// </summary>
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

        foreach (KeyValuePair<int, float> team in _testingRankings._teamPoints)
        {
            print(_gridPositions[currentTeam, 0]);
            _rocketsTransforms[team.Key].transform.position = _gridPositions[currentTeam, 0].transform.position;
            currentTeam++;
        }
        StartCoroutine(setGrid());
    }

    /// <summary>
    /// Update the teams points
    /// </summary>
    void UpdateRankingPoints()
    {

        if (!_rankingStarted)
            _rankingStarted = true;

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

    /// <summary>
    /// Calculates the position of the rockets
    /// </summary>
    void CalculateRocketsPosition()
    {
        int teamNumbers = 0;
        foreach(KeyValuePair<int, float> team in _testingRankings._teamPoints)
        {
            float positionYRelative = (team.Value - minPoints) / (maxPoints - minPoints);

            positionYRelative = (Mathf.Round(positionYRelative * 10.0f) * 0.1f) * 10;

            int positionY = (int)positionYRelative;

            Vector2 newPos = new Vector2(_rocketsTransforms[team.Key].transform.position.x, _gridPositions[teamNumbers, positionY].transform.position.y);
            _rocketsTransforms[team.Key].gameObject.GetComponent<RankingTeamMovement>().InitMove(newPos);
            teamNumbers++;
        }
    }

    /// <summary>
    /// Need waits to can create the grid
    /// </summary>
    IEnumerator setGrid()
    {
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < _numTotalTeams; i++)
        {
            _rocketsTransforms[i].transform.position = _gridPositions[i, 0].GetComponent<RectTransform>().position;
        }
    }
}
