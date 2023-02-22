using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingClient : MonoBehaviour
{
    public float maxPoints = 0;
    public float minPoints = 99999;

    [Header("Transform References")]
    public List<RectTransform> _rocketsTransforms;
    public List<RectTransform> _rankingPositions;

    [Header("Test")]
    public bool _update = false;
    public bool _gridCreated = false;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CreateGrid());
    }
    // Update is called once per frame
    void Update()
    {
        if(_update)
        {
            _update= false;
            UpdateRankingPoints();
        }
    }
    /// <summary>
    /// Start the proces to Update the ranking
    /// </summary>
    public void UpdateRankingPoints()
    {
        if (_gridCreated)
        {
            SetMinMax();
            CalculateRocketsPosition();
        }
    }
    /// <summary>
    /// Sets the new maximum and minimum values
    /// </summary>
    private void SetMinMax()
    {
        foreach (KeyValuePair<int, int> team in ServiceLocator.Instance.GetService<IGameManager>().GetTeamPoints())
        {
            float value = team.Value;

            if (value < minPoints)
            {
                minPoints = value;
            }
            if (value > maxPoints)
            {
                maxPoints = value;
            }
        }
    }
    /// <summary>
    /// Calculates the new relative positions of the spaceships in the rankings
    /// </summary>
    void CalculateRocketsPosition()
    {
        int teamNumbers = 0;
        foreach (KeyValuePair<int, int> team in ServiceLocator.Instance.GetService<IGameManager>().GetTeamPoints())
        {
            if (minPoints != maxPoints)
            {
                float positionYRelative = (team.Value - minPoints) / (maxPoints - minPoints);

                positionYRelative = (Mathf.Round(positionYRelative * 10.0f) * 0.1f) * 10;

                int positionY = (int)positionYRelative;
                Vector2 newPos = new Vector2(100, _rankingPositions[positionY].transform.position.y);
                _rocketsTransforms[team.Key].gameObject.GetComponent<RankingTeamMovement>().InitMove(newPos);
            }
            else
            {
                Vector2 newPos = new Vector2(150, _rankingPositions[team.Key].transform.position.y);
                _rocketsTransforms[team.Key].gameObject.GetComponent<RankingTeamMovement>().InitMove(newPos);
            }
            teamNumbers++;
        }
    }
    /// <summary>
    /// Generates a grid from the number of clients in the game
    /// </summary>
    public IEnumerator CreateGrid()
    {
        yield return new WaitForSeconds(1f);
        foreach (Transform team in _rocketsTransforms)
        {
            team.gameObject.SetActive(false);
        }

        int numTeam = 0;
        foreach (KeyValuePair<int, int> team in ServiceLocator.Instance.GetService<IGameManager>().GetTeamPoints())
        {
            _rocketsTransforms[numTeam].gameObject.SetActive(true);
            _rocketsTransforms[numTeam].position = new Vector2(0, _rankingPositions[numTeam].position.y);
            numTeam++;
        }
        _gridCreated = true;
        UpdateRankingPoints();
    }
}
