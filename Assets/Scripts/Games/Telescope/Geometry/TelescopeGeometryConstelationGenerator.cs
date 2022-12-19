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
    [SerializeField]
    List<GameObject> prefabList = new List<GameObject>();

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
        prefabList.Clear();
        prefabList = GetComponent<GenerateStarsTelescopeGeometry>()._gameStarList;
        //prefabList.Add(prefabList[0]);
        bool correct = true;
        int upDirection = 0;//0 = null - 1 = up - 2 = down

        if (_playerStarList[0].GetComponent<TelescopeGeometryStars>().GetOrder() != _playerStarList[_playerStarList.Count - 1].GetComponent<TelescopeGeometryStars>().GetOrder())
        {
            correct = false;
        }
        else
        {
            for (int i = 0; i < _playerStarList.Count - 1; i++)
            {
                int orderValue = _playerStarList[i].GetComponent<TelescopeGeometryStars>().GetOrder();

                if (upDirection == 0)
                {
                    upDirection = setDirection(upDirection, orderValue, i);
                }

                int postUp = 0;
                int postUpPlayer = 0;
                int postDown = 0;
                int postDownPlayer = 0;

                if (orderValue == 1)
                {
                    if (upDirection == 1)
                    {
                        postUp = 1;
                        postUpPlayer = 2;
                    }
                    else
                    {
                        postDown = prefabList.Count - 2;
                        postDownPlayer = prefabList.Count - 2;
                    }
                }
                else if (orderValue == _playerStarList.Count - 1)
                {
                    if (upDirection == 1)
                    {
                        postUp = 0;
                        postUpPlayer = 0;
                    }
                    else
                    {
                        postDown = orderValue - 2;
                        postDownPlayer = orderValue - 2;
                    }
                }
                else
                {
                    if (upDirection == 1)
                    {
                        postUp = orderValue;
                        postUpPlayer = orderValue;
                    }
                    else
                    {
                        postDown = orderValue - 2;
                        postDownPlayer = orderValue - 1;
                    }
                }


                if (upDirection == 1)
                {
                    if (prefabList[postUp].GetComponent<TelescopeGeometryStars>().GetOrder() != _playerStarList[postUp].GetComponent<TelescopeGeometryStars>().GetOrder())
                    {
                        correct = false;
                    }
                }
                else
                {
                    if (prefabList[postDown].GetComponent<TelescopeGeometryStars>().GetOrder() != _playerStarList[postDownPlayer].GetComponent<TelescopeGeometryStars>().GetOrder())
                    {
                        correct = false;
                    }
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

    private int setDirection(int upDirection, int orderValue, int i)
    {
        int value = 0;
        if (orderValue < _playerStarList[1].GetComponent<TelescopeGeometryStars>().GetOrder() && orderValue != _playerStarList.Count - 1 && orderValue != 1)//ASCENDENTE != 1 != ultimo
        {
            value = 1;
        }
        else if (orderValue > _playerStarList[1].GetComponent<TelescopeGeometryStars>().GetOrder() && orderValue != 1 && orderValue != _playerStarList.Count - 1)//DESCENDENTE =! ULTIMO != 1
        {
            value = 2;
        }
        else if (orderValue == 1)
        {
            if (_playerStarList[1].GetComponent<TelescopeGeometryStars>().GetOrder() == 2)
            {
                value = 1;
            }
            else if (_playerStarList[_playerStarList.Count - 1].GetComponent<TelescopeGeometryStars>().GetOrder() == _playerStarList[0].GetComponent<TelescopeGeometryStars>().GetOrder())
            {
                value = 2;
            }
        }
        else if (orderValue == _playerStarList.Count - 1)
        {
            if (orderValue == _playerStarList[0].GetComponent<TelescopeGeometryStars>().GetOrder())
            {
                value = 1;
            }
            else if (orderValue == _playerStarList[orderValue - 1].GetComponent<TelescopeGeometryStars>().GetOrder())
            {
                value = 2;
            }
        }
        return value;
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
