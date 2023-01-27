using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellCable : MonoBehaviour
{
    /// <summary>
    /// PREFABS SIZES:
    /// 3 = 90 | 1;
    /// 4 = 75 | 2;
    /// 5 = 50 | 4;
    /// 6 = 45 | 5
    /// 7 = 40 | 5
    /// 8 = 35 | 6
    /// 9 = 30 | 8
    /// 10 = 25 | 10;
    /// </summary>
    [Header("Referenes")]
    public List<Sprite> _spriteList = new List<Sprite>();
    public List<GameObject> _cableList = new List<GameObject>();
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
        CUATROERR,
        INICIORECTO,
        FINRECTO,
        INICIOGIROBAJO,
        INICIOGIROARRI,
        FINGIROBAJO,
        FINGIROARRI
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
        foreach (GameObject currentCable in _cableList)
        {
            currentCable.SetActive(false);
        }

        switch (_cellState)
        {
            case CELL_STATE.EMPTY:
                _cableList[0].SetActive(true);
                break;
            case CELL_STATE.RECTO:
                _cableList[1].SetActive(true);
                break;
            case CELL_STATE.GIRO:
                _cableList[2].SetActive(true);
                break;
            case CELL_STATE.TRES:
                _cableList[3].SetActive(true);
                break;
            case CELL_STATE.CUATRO:
                _cableList[4].SetActive(true);
                break;
            case CELL_STATE.TRESERR:
                _cableList[5].SetActive(true);
                break;
            case CELL_STATE.CUATROERR:
                _cableList[6].SetActive(true);
                break;
            case CELL_STATE.INICIORECTO:
                _cableList[7].SetActive(true);
                break;
            case CELL_STATE.FINRECTO:
                _cableList[8].SetActive(true);
                break;
            case CELL_STATE.INICIOGIROBAJO:
                _cableList[9].SetActive(true);
                break;
            case CELL_STATE.INICIOGIROARRI:
                _cableList[10].SetActive(true);
                break;
            case CELL_STATE.FINGIROBAJO:
                _cableList[11].SetActive(true);
                break;
            case CELL_STATE.FINGIROARRI:
                _cableList[12].SetActive(true);
                break;
            default:
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
    public void SetNewState(Vector2 pos, bool isInit)
    {
        _pass++;
        //int isUp = 0;//0 = Misma Altura || 1 = Altura +1 || -1 = Altura -1 
        //int isRight = 0;//0 = Misma Columna|| 1 = Columna -1 || -1 = Columna +1 

        float posX = _cellPos.x - pos.x;
        float posY = _cellPos.y - pos.y;
        GetComponent<Button>().enabled = false;
        switch (posX)
        {
            case 1:
                if (isInit)
                {
                    _cellState = CELL_STATE.INICIOGIROARRI;
                }
                else
                {
                    _cellState = CELL_STATE.FINGIROARRI;
                }
                break;
            case -1:
                if (!isInit)
                {
                    _cellState = CELL_STATE.INICIOGIROBAJO;
                }
                else
                {
                    _cellState = CELL_STATE.FINGIROBAJO;
                }
                break;
            default:
                break;
        }
        switch (posY)
        {
            case 1:
            case -1:
                if (isInit)
                {
                    _cellState = CELL_STATE.INICIORECTO;
                }
                else
                {
                    _cellState = CELL_STATE.FINRECTO;
                }
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
        if (pos > Enum.GetValues(typeof(ROTATION)).Length - 1)
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
