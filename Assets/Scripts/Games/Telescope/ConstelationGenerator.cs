using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstelationGenerator : MonoBehaviour
{
    private LineRenderer _line;
    List<Vector2> _constelationPositions = new List<Vector2>();
    List<GameObject> _playerStarList = new List<GameObject>();

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
        Vector2 posFormat = Camera.main.ScreenToWorldPoint(newPos);
        _constelationPositions.Add(posFormat);
        _constelationPositions.Add(posFormat);
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
                ServiceLocator.Instance.GetService<IPositive>().GenerateFeedback(Vector3.zero);

                ClearConstelation();
                GetComponent<GenerateStarsTelescopeSeries>().GenerateNewOrde();
            }
            else
            {
                ServiceLocator.Instance.GetService<IError>().GenerateError();
                ClearConstelation();
            }
        }
    }
}
