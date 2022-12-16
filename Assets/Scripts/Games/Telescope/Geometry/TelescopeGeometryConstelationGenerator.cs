using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelescopeGeometryConstelationGenerator : MonoBehaviour
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
        if (_constelationPositions.Count > 1)
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
        if (_constelationPositions.Count > 0)
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
            starsInScene[i].GetComponent<TelescopeGeometryStars>().SetIsConnected(false);
        }
    }


    /// <summary>
    /// Add the last star pressed
    /// </summary>
    /// <param name="star"></param>
    public void AddStars(GameObject star)
    {
        _playerStarList.Add(star);
        AddNewPosition(star.transform.position);

    }
    /// <summary>
    /// Check's if the player list of stars is correct
    /// </summary>
    public void CheckIfIsCorrect()
    {
        bool correct = true;

        if (_playerStarList[0].GetComponent<TelescopeGeometryStars>().GetOrder() != _playerStarList[_playerStarList.Count - 1].GetComponent<TelescopeGeometryStars>().GetOrder())
        {
            correct = false;
        }
        else
        {
            for (int i = 0; i < _playerStarList.Count; i++)
            {
                int orderValue = _playerStarList[i].GetComponent<TelescopeGeometryStars>().GetOrder();

                int orderUp = 0;
                int orderDown = 0;

                int postUp = 0;
                int postDown = 0;

                if (orderValue == 1)
                {
                    orderUp = 2;
                    postUp = 1;

                    orderDown = _playerStarList.Count - 1;
                    postDown = _playerStarList.Count - 2;
                }
                else if (orderValue == _playerStarList.Count - 1)
                {
                    orderUp = 1;
                    postUp = 0;

                    orderDown = orderValue - 1;
                    postDown = orderValue - 2;
                }
                else
                {
                    orderUp = orderValue + 1;
                    postUp = i + 1;

                    orderDown = orderValue - 1;
                    postDown = i - 1;
                }

                if (_playerStarList[postUp].GetComponent<TelescopeGeometryStars>().GetOrder() != orderUp ||
                    _playerStarList[postDown].GetComponent<TelescopeGeometryStars>().GetOrder() != orderDown)
                {
                    correct = false;
                }
            }
        }

        if (correct)
        {
            _success++;
            ServiceLocator.Instance.GetService<IPositive>().GenerateFeedback(Vector3.zero);
            ServiceLocator.Instance.GetService<ICalculatePoints>().Puntuation(_success, _errors);
            _success = 0;
            _errors = 0;
            StartCoroutine(GetComponent<GenerateStarsTelescopeGeometry>().GenerateNewOrde());
        }
        else
        {
            _errors++;
            ServiceLocator.Instance.GetService<IError>().GenerateError();
            ClearConstelation();
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
