using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FinalMinigameRanking : MonoBehaviour
{
    public Text _scorePanel;
    public GameObject _panel;
    public TextMeshProUGUI _nameTxt;
    public TextMeshProUGUI _muyBien;
    public TextMeshProUGUI _points;
    public List<GameObject> _starList;
    private int _totalPoints;
    private float _currentPoints;
    private int _starPoints;
    public int _totalStars;
    private float _addPoints;
    private float _nextStar;
    private int _currentStar = 0;
    private bool _newStar = true;
    private bool _isWait = true;

    // Update is called once per frame
    void Update()
    {
        if (!_isWait)
        {
            _currentPoints += _addPoints;
            if (_currentPoints >= _nextStar && _newStar)
                ActiveStar();

            if (_currentPoints >= _totalPoints)
            {
                _currentPoints = _totalPoints;
            }
            _points.text = (Mathf.Round(_currentPoints * 10.0f) * 0.1f).ToString();
        }
    }
    /// <summary>
    /// Displays a panel with the number of points scored and a star rating
    /// </summary>
    public void UpdateFinalPanel()
    {
        _panel.SetActive(true);
        _nameTxt.text = ServiceLocator.Instance.GetService<IGameManager>().GetCurrentStudentName();
        _totalPoints = (int)ServiceLocator.Instance.GetService<ICalculatePoints>().GetAverage().averageSuccess;
        _starPoints = (int)ServiceLocator.Instance.GetService<ICalculatePoints>().GetAverage().averageSuccess;
        _addPoints = Time.deltaTime * _totalPoints * 0.5f;
        if (_starPoints < 33)
        {
            _muyBien.text = "La próxima vez mejor";
            _totalStars = 1;
        }
        else if (_starPoints > 66)
        {
            _muyBien.text = "¡Qué fantástico!";
            _totalStars = 3;
        }

        else
        {
            _muyBien.text = "¡Lo has hecho muy bien!";
            _totalStars = 2;
        }
        StartCoroutine(WaitToStart());
    }
    /// <summary>
    /// Activate the following stars
    /// </summary>
    private void ActiveStar()
    {
        _starList[_currentStar].SetActive(true);
        _newStar = false;

        if (_currentStar < _totalStars - 1)
        {
            _newStar = true;
            _nextStar = (_totalPoints / _totalStars) + _currentPoints;
            _currentStar++;
        }
    }
    /// <summary>
    /// Waiting period
    /// </summary>
    IEnumerator WaitToStart()
    {
        yield return new WaitForSeconds(1.5f);
        _isWait = false;
        ActiveStar();
    }
}
