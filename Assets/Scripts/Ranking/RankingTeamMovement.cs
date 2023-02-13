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
    public LineRenderer _line;
    public int _team = -1;
    public bool _finalScene = false;

    // Update is called once per frame
    void Update()
    {
        if (_inMovement)
        {
            _time += Time.deltaTime / _moveTime;
            transform.position = Vector2.Lerp(_startPos, _newTargetPos, _time);

            SetLineRenderer();
            if (transform.position.y == _newTargetPos.y)
            {
                _inMovement = false;
            }
        }
    }
    /// <summary>
    /// Set's que line renderer to the variable <see cref="_line"/>
    /// </summary>
    private void SetLineRenderer()
    {
        if (!_line && _finalScene)
        {
            switch (_team)
            {
                case 1:
                    _line = GameObject.Find("Team1Line").GetComponent<LineRenderer>();
                    break;
                case 2:
                    _line = GameObject.Find("Time2Line").GetComponent<LineRenderer>();
                    break;
                case 3:
                    _line = GameObject.Find("Time3Line").GetComponent<LineRenderer>();
                    break;
                case 4:
                    _line = GameObject.Find("Time4Line").GetComponent<LineRenderer>();
                    break;
                case 5:
                    _line = GameObject.Find("Time5Line").GetComponent<LineRenderer>();
                    break;
                case 6:
                    _line = GameObject.Find("Time6Line").GetComponent<LineRenderer>();
                    break;
                default:
                    break;
            }
            _line.SetPosition(0, new Vector3(transform.position.x, -2.8f, 5));
            _line.SetPosition(1, new Vector3(transform.position.x, transform.position.y - 0.2f, 5));
        }

    }
    public void InitMove(Vector2 newPos)
    {
        _time = 0;
        _startPos = transform.position;
        _newTargetPos = newPos;
        _inMovement = true;
    }
    public void SetBasePos(Vector2 basePos)
    {
    }
}
