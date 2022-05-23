using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingTeamMovement : MonoBehaviour
{
    private Vector2 _newTargetPos;
    private Vector2 _startPos;
    private bool _inMovement = false;
    public float _moveTime;
    private float _time;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_inMovement)
        {
            _time += Time.deltaTime / _moveTime;
            transform.position = Vector2.Lerp(_startPos, _newTargetPos, _time);
            if (transform.position.y == _newTargetPos.y)
                _inMovement = false;
        }

    }

    public void InitMove(Vector2 newPos)
    {
        _time = 0;
        _startPos = transform.position;
        _newTargetPos = newPos;
        _inMovement = true;
    }
}
