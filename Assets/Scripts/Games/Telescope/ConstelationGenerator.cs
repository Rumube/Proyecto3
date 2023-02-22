using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstelationGenerator : MonoBehaviour
{
    private LineRenderer _line;
    List<Vector2> _constelationPositions = new List<Vector2>();
    List<GameObject> _playerStarList = new List<GameObject>();
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
    /// Update the last position of the constelation
    /// </summary>
    /// <param name="newPos">Position</param>
    public void UpdateLastPosition(Vector2 newPos)
    {
        if(_constelationPositions.Count > 1)
        {
            Vector2 posFormat = Camera.main.ScreenToWorldPoint(newPos);
            _constelationPositions[_constelationPositions.Count - 1] = posFormat;
        }
    }
    /// <summary>
    /// Add new position to the list
    /// </summary>
    /// <param name="newPos">Position</param>
    public void AddNewPosition(Vector2 newPos)
    {
        if (_playerStarList.Count == 1)
        {
            Vector2 posFormat = Camera.main.ScreenToWorldPoint(newPos);
            _constelationPositions.Add(posFormat);
            _constelationPositions.Add(posFormat);
        }
        else
        {
            Vector2 posFormat = Camera.main.ScreenToWorldPoint(newPos);
            _constelationPositions.Add(posFormat);
        }
    
    }
    /// <summary>
    /// Draw the constelation
    /// </summary>
    private void DrawConstelation()
    {
        _line.positionCount = _constelationPositions.Count;
        if(_constelationPositions.Count > 0)
        {
            for (int i = 0; i < _constelationPositions.Count; i++)
            {
                _line.SetPosition(i, _constelationPositions[i]);
            }
        }
    }
    /// <summary>
    /// Clean the constelation arry and redraw
    /// </summary>
    public void ClearConstelation()
    {
        _constelationPositions.Clear();
        _playerStarList.Clear();
        GameObject[] starsInScene = GameObject.FindGameObjectsWithTag("Star");
        for (int i = 0; i < starsInScene.Length; i++)
        {
            starsInScene[i].GetComponent<Star>().SetIsConnected(false);
        }
    }
    /// <summary>
    /// Check's if the player list of stars is correct, and add the last star pressed
    /// </summary>
    /// <param name="star">The star pressed</param>
    public void CheckIfIsCorrect(GameObject star)
    {
        _playerStarList.Add(star);
        AddNewPosition(star.transform.position);
        List<GameObject> gameStarList = new List<GameObject>(GetComponent<GenerateStarsTelescopeSeries>()._starList);

        if(_playerStarList.Count == gameStarList.Count)
        {
            bool isCorrect = true;
            for (int i = 0; i < _playerStarList.Count; i++)
            {
                if(_playerStarList[i] != gameStarList[i])
                {
                    isCorrect = false;
                }
            }
            if (isCorrect)
            {
                _success++;
                ServiceLocator.Instance.GetService<IPositive>().GenerateFeedback(Vector3.zero);
                ServiceLocator.Instance.GetService<ICalculatePoints>().Puntuation(_success, _errors);
                _success = 0;
                _errors = 0;
                StartCoroutine(GetComponent<GenerateStarsTelescopeSeries>().GenerateNewOrde());
               
            }
            else
            {
                _errors++;
                ServiceLocator.Instance.GetService<IError>().GenerateError();
                ClearConstelation();
            }
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
}
