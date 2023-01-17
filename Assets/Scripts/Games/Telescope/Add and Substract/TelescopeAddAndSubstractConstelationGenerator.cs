using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelescopeAddAndSubstractConstelationGenerator : MonoBehaviour
{
    public LineRenderer _line;


    public List<GameObject> _playerStarList = new List<GameObject>();
    int _success = 0;
    int _errors = 0;
    public Vector2 point;
    public float radius;
    
    // Start is called before the first frame update
    void Start()
    {
        _line = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        DrawConstelation();
    }


    /// <summary>
    /// Draw the constelation
    /// </summary>
    private void DrawConstelation()
    {
        _line.positionCount = _playerStarList.Count;
        _line.startWidth = 5f;
        _line.endWidth = 5f;
        for (int i = 0; i < _playerStarList.Count; i++)
        {
            _line.SetPosition(i, _playerStarList[i].transform.position);
        }
    }


    /// <summary>
    /// Clean the constelation arry and redraw
    /// </summary>
    public void ClearConstelation()
    {
        _playerStarList.Clear();
        GameObject[] starsInScene = GameObject.FindGameObjectsWithTag("Star");
        for (int i = 0; i < starsInScene.Length; i++)
        {
            starsInScene[i].GetComponent<TelescopeAddAndSubstractStars>().SetStarConnected(false);
        }
    }

    /// <summary>
    /// Check's if the player list of stars is correct
    /// </summary>
    public void CheckIfIsCorrect()
    {
        bool correct = false;
        if (_playerStarList.Count == GetComponent<GenerateStarsTelescopeAddAndSubstract>().GetRandomStars())
        {
            correct = true;
        }
        FinishGame(correct);

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="correct"></param>
    private void FinishGame(bool correct)
    {
        if (correct)
        {
            _success++;
            ServiceLocator.Instance.GetService<IPositive>().GenerateFeedback(Vector3.zero);
            ServiceLocator.Instance.GetService<ICalculatePoints>().Puntuation(_success, _errors);
            _success = 0;
            _errors = 0;
            StartCoroutine(GetComponent<GenerateStarsTelescopeAddAndSubstract>().GenerateNewOrde());
        }
        else
        {
            _errors++;
            ServiceLocator.Instance.GetService<IError>().GenerateError();
            ClearConstelation();
            StartCoroutine(GetComponent<GenerateStarsTelescopeAddAndSubstract>().GenerateNewOrde());
        }
    }

    /// <summary>
    /// Returns the number of star selecteds for the player
    /// </summary>
    /// <returns></returns>
    public int GetStarsSelecteds()
    {
        return _playerStarList.Count;
    }
    public List<GameObject> GetplayerStarList()
    {
        return _playerStarList;
    }
}
