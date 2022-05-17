using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingClient : MonoBehaviour
{
    public float maxPoints = 0;
    public float minPoints = 99999;

    [Header("Transform References")]
    public List<Transform> _rocketsTransforms;
    public List<Transform> _rankingPositions;

    private TestingRankings _testingRankings;
    // Start is called before the first frame update
    void Start()
    {
        _testingRankings = GetComponent<TestingRankings>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CalculateRocketsPosition()
    {
        int teamNumbers = 0;
        foreach (KeyValuePair<int, float> team in _testingRankings._teamPoints)
        {
            float positionYRelative = (team.Value - minPoints) / (maxPoints - minPoints);

            positionYRelative = (Mathf.Round(positionYRelative * 10.0f) * 0.1f) * 10;

            int positionY = (int)positionYRelative;

            Vector2 newPos = new Vector2(_rocketsTransforms[team.Key].transform.position.x, _rankingPositions[positionY].transform.position.y);
            _rocketsTransforms[team.Key].gameObject.GetComponent<RankingTeamMovement>().InitMove(newPos);
            teamNumbers++;
        }
    }

    public IEnumerator CreateGrid()
    {
        yield return new WaitForSeconds(5f);
        foreach (Transform team in _rocketsTransforms)
        {
            team.gameObject.SetActive(false);
        }

        int numTeam = 0;
        foreach (KeyValuePair<int, float> team in _testingRankings._teamPoints)
        {
            _rocketsTransforms[numTeam].gameObject.SetActive(true);
            _rocketsTransforms[numTeam].gameObject.GetComponent<RectTransform>().position = _rankingPositions[numTeam].gameObject.GetComponent<RectTransform>().position;

            numTeam++;
        }
    }
}
