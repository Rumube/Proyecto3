using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingTeamMovement : MonoBehaviour
{
    private Vector2 _newTargetPos;
    private Vector2 _startPos;
    [SerializeField] private bool _inMovement = false;
    [SerializeField] private float _finishMovementDistance = 0;
    public float _moveTime;
    private float _time;
    public bool _finalScene = false;
    private Transform _lastPos;
    private Image _image = null;
    private float _lastTrailPos;


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
            if (Mathf.Abs(transform.position.y - _newTargetPos.y) <= _finishMovementDistance)
            {
                _inMovement = false;
            }
        }
        TrailManager();
    }
    /// <summary>
    /// Manage the ships trail
    /// Only works when _imagen exits, don't work in game scenes
    /// </summary>
    private void TrailManager()
    {
        if (_image != null)
        {
            int fill = (int)((transform.position.y / _lastPos.position.y) * 10);
            print("Nave: " + name + " / Pos: " + fill);

            _image.fillAmount = Mathf.Lerp(_lastTrailPos, transform.position.y / _lastPos.position.y, _time);
            _lastTrailPos = (transform.position.y / _lastPos.position.y) - 0.1f;

            if (!_inMovement)
            {
                switch (fill)
                {
                    case 0:
                        _image.fillAmount = 0.1f;
                        _lastTrailPos = 0.1f;
                        break;
                    case 1:
                        _image.fillAmount = 0.1f;
                        _lastTrailPos = 0.1f;
                        break;
                    case 2:
                        _image.fillAmount = 0.1f;
                        _lastTrailPos = 0.1f;
                        break;
                    case 3:
                        _image.fillAmount = 0.25f;
                        _lastTrailPos = 0.25f;
                        break;
                    case 4:
                        _image.fillAmount = 0.4f;
                        _lastTrailPos = 0.4f;
                        break;
                    case 5:
                        _image.fillAmount = 0.5f;
                        _lastTrailPos = 0.5f;
                        break;
                    case 6:
                        _image.fillAmount = 0.6f;
                        _lastTrailPos = 0.6f;
                        break;
                    case 7:
                        _image.fillAmount = 0.7f;
                        _lastTrailPos = 0.7f;
                        break;
                    case 8:
                        _image.fillAmount = 0.8f;
                        _lastTrailPos = 0.8f;
                        break;
                    case 9:
                        _image.fillAmount = 0.95f;
                        _lastTrailPos = 0.95f;
                        break;
                    case 10:
                        _image.fillAmount = 1f;
                        _lastTrailPos = 1f;
                        break;
                    default:
                        break;
                }
            }
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
    /// <param name="image">Image referring to the ship's trail</param>
    public void InitValues(Image image, Transform lastPos)
    {
        _lastPos = lastPos;
        _image = image;
        _lastTrailPos = 0;
    }
}
