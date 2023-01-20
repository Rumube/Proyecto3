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

    private Dictionary<int, int> _teamPoints = new Dictionary<int, int>();
    private List<GameObject> _teamParents = new List<GameObject>();

    [Header("Test")]
    public bool _test = false;
    public bool _newDataFromClient;
    public bool _generateGrid;
    public List<int> _points = new List<int>();

    [Header("References")]
    public Transform _lowerLevel;
    public Transform _upperLevel;
    public float _differenceBetweenLowerUpper;
    public GameObject _rankingGridParent;

    [Header("Transform References")]
    public List<Transform> _rocketsTransforms;
    public List<Transform> _rankingPositions;

    private float _maxPointsInt = 0;
    private float _minPointsInt = 99999;

    public GameObject[,] _gridPositions;

    private bool _rankingStarted = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (_newDataFromClient)
        {
            _newDataFromClient = false;
            UpdateRankingPoints();
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
        UpdatePoints();

        foreach (Transform team in _rocketsTransforms)
        {
            team.gameObject.SetActive(false);
        }

        List<GameObject> teamParentList = new List<GameObject>();

        foreach (KeyValuePair<int, int> team in _teamPoints)
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
            _teamParents.Add(teamParent);
        }
        _gridPositions = new GameObject[_teamPoints.Count, 11];
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

                if(j == 0)
                {
                    _rocketsTransforms[i].gameObject.GetComponent<RankingTeamMovement>().SetBasePos(_gridPositions[i, j].transform.position);
                }

            }
        }

        StartCoroutine(setGrid());
    }
    /// <summary>
    /// Set the init variables and starts the process
    /// </summary>
    public void StartRanking()
    {
        _minPoints.text = 0.ToString();
        _maxPoints.text = 0.ToString();

        _differenceBetweenLowerUpper = _upperLevel.position.y - _lowerLevel.position.y;
        CreateGrid();
    }

    /// <summary>
    /// Update the teams points
    /// </summary>
    public void UpdateRankingPoints()
    {
        UpdatePoints();
        if (!_rankingStarted)
            _rankingStarted = true;

        if (float.TryParse(_maxPoints.text, out maxPoints))
        {
            if (float.TryParse(_minPoints.text, out minPoints))
            {
                _minPointsInt = 99999;
                _maxPointsInt = 0;
                foreach (KeyValuePair<int, int> team in _teamPoints)
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

        CalculateRocketsPosition(_teamPoints);
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
                //_teamParents[team.Key].GetComponent<Image>().fillAmount = positionYRelative/10;
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

        foreach (KeyValuePair<int, int> team in _teamPoints)
        {
            _rocketsTransforms[team.Key].transform.position = _gridPositions[team.Key, 0].transform.position;
        }
        UpdateRankingPoints();
    }
    /// <summary>
    /// If <see cref="_test"/> is TRUE Update <see cref="_teamPoints"/> whit the <see cref="_points"/> values
    /// Else <see cref="_test"/> is FALSE Update <see cref="_teamPoints"/> whit the <see cref="IGameManager.GetTeamPoints()"/> values
    /// </summary>
    public void UpdatePoints()
    {
        if (_test)
        {
            for (int i = 0; i < _points.Count; i++)
            {
                _teamPoints[i] = _points[i];
            }
        }
        else
        {
            _teamPoints = ServiceLocator.Instance.GetService<IGameManager>().GetTeamPoints();
        }
    }

    #region FakeServerInClient
    public void CreateGridClient()
    {
        CreateGrid();
    }
    #endregion
}
