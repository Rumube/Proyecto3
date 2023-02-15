using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingTeamMovement : MonoBehaviour
{
    private Vector2 _newTargetPos;
    private Vector2 _startPos;
    private bool _inMovement = false;
    public float _moveTime;
    private float _time;
    public int _team = -1;
    public bool _finalScene = false;
    [SerializeField] private Image _image = null;
    [SerializeField] private Transform _zeroPos;
    [SerializeField] private Transform _lastPos;

    // Update is called once per frame
    void Update()
    {
        RankingMovement();
    }
    /// <summary>
    /// Manage the ship movement using a Lerp betwen two positions
    /// </summary>
    private void RankingMovement()
    {
        if (_inMovement)
        {
            _time += Time.deltaTime / _moveTime;
            transform.position = Vector2.Lerp(_startPos, _newTargetPos, _time);
            if (transform.position.y == _newTargetPos.y)
            {
                _inMovement = false;
            }
            TrailManager();
        }
    }
    /// <summary>
    /// Manage the ships trail
    /// Only works when _imagen exits, don't work in game scenes
    /// </summary>
    private void TrailManager()
    {
        if (_image != null)
        {
            _image.fillAmount = (transform.position.y / _lastPos.position.y);
        }
    }
    /// <summary>
    /// Starts the ship movement process
    /// </summary>
    /// <param name="newPos">New pos to move</param>
    public void InitMove(Vector2 newPos)
    {
        _time = 0;
        _startPos = transform.position;
        _newTargetPos = newPos;
        _inMovement = true;
    }
    /// <summary>
    /// Initializes the ship's values
    /// </summary>
    /// <param name="zeroPos">Lower potion</param>
    /// <param name="lastPos">Highest position</param>
    /// <param name="image">Image referring to the ship's trail</param>
    public void InitValues(Transform zeroPos, Transform lastPos, Image image)
    {
        _zeroPos = zeroPos;
        _lastPos = lastPos;
        _image = image;
    }
}
