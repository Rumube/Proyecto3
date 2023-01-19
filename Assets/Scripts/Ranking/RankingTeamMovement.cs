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

    // Update is called once per frame
    void Update()
    {
        if (_inMovement)
        {
            _time += Time.deltaTime / _moveTime;
            transform.position = Vector2.Lerp(_startPos, _newTargetPos, _time);

            _line.SetPosition(0, Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, 0, 0)));
            _line.SetPosition(1, Camera.main.WorldToScreenPoint(transform.position));


            if (transform.position.y == _newTargetPos.y)
            {
                _inMovement = false;
            }
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
