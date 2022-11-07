using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RankingClientFinal : MonoBehaviour
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
    public bool _isTest;
    public bool _newDataFromClient;
    public bool _generateGrid;
    [SerializeField]
    public Dictionary<int, int> _testTeamPoints = new Dictionary<int, int>();

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
    private float _minPointsInt = 0;

    public GameObject[,] _gridPositions;
    private int _numTotalTeams;

    private bool _rankingStarted = false;
    // Start is called before the first frame update
    void Start()
    {
        _minPoints.text = 0.ToString("0000");
        _maxPoints.text = 0.ToString("0000");

        _differenceBetweenLowerUpper = _upperLevel.position.y - _lowerLevel.position.y;
        _testTeamPoints.Add(0, 0);
        _testTeamPoints.Add(1, 0);
        _testTeamPoints.Add(2, 0);
        _testTeamPoints.Add(3, 0);
        _testTeamPoints.Add(4, 0);
        _testTeamPoints.Add(5, 0);

        CreateGrid(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isTest)
        {
            if (_newDataFromClient)
            {
                _testTeamPoints.Clear();
                _testTeamPoints.Add(0, 150);
                _testTeamPoints.Add(1, 450);
                _testTeamPoints.Add(2, 50);
                _testTeamPoints.Add(3, 300);
                _testTeamPoints.Add(4, 250);
                _testTeamPoints.Add(5, 25);

                _newDataFromClient = false;
                UpdateRankingPoints(ServiceLocator.Instance.GetService<IGameManager>().GetTeamPoints());
            }
            if (_generateGrid)
            {
                _generateGrid = false;
                CreateGrid(true);
            }
        }
        else
        {
            if (_newDataFromClient)
            {
                _testTeamPoints.Clear();
                _testTeamPoints.Add(0, 150);
                _testTeamPoints.Add(1, 3450);
                _testTeamPoints.Add(2, 50);
                _testTeamPoints.Add(3, 300);
                _testTeamPoints.Add(4, 250);
                _testTeamPoints.Add(5, 25);
                _newDataFromClient = false;
                UpdateRankingPoints(_testTeamPoints);
            }
            if (_generateGrid)
            {
                _generateGrid = false;
                CreateGrid(true);
            }
        }

        _currentMaxPoints = Mathf.Lerp(_currentMaxPoints, _maxPointsInt, 5 * Time.deltaTime);
        _maxPoints.text = _currentMaxPoints.ToString("0000");

        _currentMinPoints = Mathf.Lerp(_currentMinPoints, _minPointsInt, 5 * Time.deltaTime);
        _minPoints.text = _currentMinPoints.ToString("0000");


    }

    /// <summary>
    /// Creates the ranking grid
    /// </summary>
    public void CreateGrid(bool finalScore = false)
    {
        Dictionary<int, int> teamsPoint = _testTeamPoints;

        foreach (Transform team in _rocketsTransforms)
        {
            team.gameObject.SetActive(false);
        }

        List<GameObject> teamParentList = new List<GameObject>();

        foreach (KeyValuePair<int, int> team in teamsPoint)
        {
            GameObject teamParent = new GameObject();
            teamParent.name = "Team_" + team.Key;
            teamParentList.Add(teamParent);
            _rocketsTransforms[team.Key].gameObject.SetActive(true);
            _rocketsTransforms[team.Key].gameObject.transform.position = teamParent.GetComponentInChildren<Transform>().position;
            teamParent.transform.SetParent(_rankingGridParent.transform);
            teamParent.AddComponent<VerticalLayoutGroup>();
            teamParent.GetComponent<VerticalLayoutGroup>().spacing = 80;
            teamParent.GetComponent<VerticalLayoutGroup>().reverseArrangement = true;
            teamParent.transform.localScale = new Vector3(1, 1, 1);
        }
        _gridPositions = new GameObject[teamsPoint.Count, 11];
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

        StartCoroutine(setGrid());
    }

    /// <summary>
    /// Update the teams points
    /// </summary>
    public void UpdateRankingPoints(Dictionary<int, int> teamPoints)
    {

        if (!_rankingStarted)
            _rankingStarted = true;

        if (float.TryParse(_maxPoints.text, out maxPoints))
        {
            if (float.TryParse(_minPoints.text, out minPoints))
            {
                _minPointsInt = 99999;
                _maxPointsInt = 0;
                foreach (KeyValuePair<int, int> team in teamPoints)
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
        CalculateRocketsPosition(teamPoints);
    }

    /// <summary>
    /// Calculates the position of the rockets
    /// </summary>
    void CalculateRocketsPosition(Dictionary<int, int> teamPoints)
    {
        int teamNumbers = 0;
        foreach (KeyValuePair<int, int> team in teamPoints)
        {
            if (minPoints != maxPoints)
            {
                float positionYRelative = (team.Value - minPoints) / (maxPoints - minPoints);

                positionYRelative = (Mathf.Round(positionYRelative * 10.0f) * 0.1f) * 10;

                int positionY = (int)positionYRelative;

                Vector2 newPos = new Vector2(_rocketsTransforms[team.Key].transform.position.x, _gridPositions[teamNumbers, positionY].transform.position.y);
                _rocketsTransforms[team.Key].gameObject.GetComponent<RankingTeamMovement>().InitMove(newPos);

            }
            else
            {
                Vector2 newPos = new Vector2(_rocketsTransforms[team.Key].transform.position.x, _gridPositions[teamNumbers, 0].transform.position.y);
                _rocketsTransforms[team.Key].gameObject.GetComponent<RankingTeamMovement>().InitMove(newPos);
            }
            teamNumbers++;
        }
    }

    /// <summary>
    /// Need waits to can create the grid
    /// </summary>
    IEnumerator setGrid()
    {
        yield return new WaitForSeconds(0.1f);

        foreach (KeyValuePair<int, int> team in _testTeamPoints)
        {
            print(_gridPositions[team.Key, 0]);
            _rocketsTransforms[team.Key].transform.position = _gridPositions[team.Key, 0].transform.position;
        }
        UpdateRankingPoints(_testTeamPoints);
    }

    #region FakeServerInClient
    public void CreateGridClient()
    {
        CreateGrid(true);
    }
    #endregion
}