using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellCable : MonoBehaviour
{
    [Header("Referenes")]
    public List<Sprite> _spriteList = new List<Sprite>();
    private Image _sprite;
    private Vector2 _cellPos = Vector2.zero;
    private int _pass = 0;
    public enum CELL_STATE
    {
        EMPTY,
        RECTO,
        GIRO,
        TRES,
        CUATRO,
        TRESERR,
        CUATROERR
    }
    public enum ROTATION
    {
        D0,
        D90,
        D180,
        D270
    }
    public CELL_STATE _cellState = CELL_STATE.EMPTY;
    public ROTATION _cellRotation = ROTATION.D0;
    private bool _isInit = false;
    private bool _isFinish = false;

    // Start is called before the first frame update
    void Start()
    {
        _sprite = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        SetSprite();
        SetRotation();
    }
    /// <summary>
    /// Chenge the sprite using <see cref="_cellState"/> value
    /// </summary>
    private void SetSprite()
    {
        switch (_cellState)
        {
            case CELL_STATE.EMPTY:
                _sprite.sprite = _spriteList[0];
                break;
            case CELL_STATE.RECTO:
                _sprite.sprite = _spriteList[1];
                break;
            case CELL_STATE.GIRO:
                _sprite.sprite = _spriteList[2];
                break;
            case CELL_STATE.TRES:
                _sprite.sprite = _spriteList[3];
                break;
            case CELL_STATE.CUATRO:
                _sprite.sprite = _spriteList[4];
                break;
            case CELL_STATE.TRESERR:
                _sprite.sprite = _spriteList[5];
                break;
            case CELL_STATE.CUATROERR:
                _sprite.sprite = _spriteList[6];
                break;
            default:
                _sprite.sprite = _spriteList[0];
                break;
        }
    }
    private void SetRotation()
    {
        switch (_cellRotation)
        {
            case ROTATION.D0:
                transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case ROTATION.D90:
                transform.rotation = Quaternion.Euler(0, 0, 90);
                break;
            case ROTATION.D180:
                transform.rotation = Quaternion.Euler(0, 0, 180);
                break;
            case ROTATION.D270:
                transform.rotation = Quaternion.Euler(0, 0, 270);
                break;
            default:
                break;
        }
    }
    public void SetNewState(Vector2 pos)
    {
        _pass++;
        //int isUp = 0;//0 = Misma Altura || 1 = Altura +1 || -1 = Altura -1 
        //int isRight = 0;//0 = Misma Columna|| 1 = Columna -1 || -1 = Columna +1 

        float posX = _cellPos.x - pos.x;
        float posY = _cellPos.y - pos.y;

        switch (posX)
        {
            case 1:
                _cellState = CELL_STATE.GIRO;
                _cellRotation = ROTATION.D90;
                break;
            case -1:
                _cellState = CELL_STATE.GIRO;
                _cellRotation = ROTATION.D180;
                break;
            default:
                break;
        }
        switch (posY)
        {
            case 1:
            case -1:
                SetCellState(CELL_STATE.RECTO);
                _cellRotation = ROTATION.D90;
                break;
        }
    }

    public void SetNewState(Vector2 prePos, Vector2 nextPos)
    {
        _pass++;
        //int isUp = 0;//0 = Misma Altura || 1 = Altura +1 || -1 = Altura -1 
        //int isRight = 0;//0 = Misma Columna|| 1 = Columna -1 || -1 = Columna +1 

        float prePosX = _cellPos.x - prePos.x;
        float prePosY = _cellPos.y - prePos.y;

        float nextPosx = _cellPos.x - nextPos.x;
        float nextPosy = _cellPos.y - nextPos.y;
        if (_pass == 1)
        {
            if (prePos.x != nextPos.x && prePos.y != nextPos.y)
            {
                _cellState = CELL_STATE.GIRO;
            }
            else
            {
                _cellState = CELL_STATE.RECTO;
            }
        }
        else
        {
            _cellState = CELL_STATE.TRES;
        }

    }
    public void RotateCell()
    {
        int pos = (int)_cellRotation;
        pos++;
        if (pos > Enum.GetValues(typeof(ROTATION)).Length)
        {
            pos = 0;
        }
        _cellRotation = (ROTATION)pos;
    }

    #region GETS
    public CELL_STATE GetCellState()
    {
        return _cellState;
    }
    public ROTATION GetRotation()
    {
        return _cellRotation;
    }
    public Vector2 GetCellPos()
    {
        return _cellPos;
    }
    public float GetDistance(Vector2 pos)
    {
        return Mathf.Abs(Vector2.Distance(pos, _cellPos));
    }
    public bool GetIsInit()
    {
        return _isInit;
    }
    public bool GetIsFinish()
    {
        return _isFinish;
    }
    #endregion
    #region SETS
    public void SetCellState(CELL_STATE state)
    {
        _cellState = state;
    }
    public void SetCellPos(Vector2 pos)
    {
        _cellPos = pos;
    }
    public void SetIsInit(bool isInit)
    {
        _isInit = isInit;
    }
    public void SetIsFinish(bool isFinish)
    {
        _isFinish = isFinish;
    }
    #endregion
}
