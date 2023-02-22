using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellCable : MonoBehaviour
{
    [Header("Referenes")]
    public List<Sprite> _spriteList = new List<Sprite>();
    public List<GameObject> _cableList = new List<GameObject>();
    private Vector2 _cellPos = Vector2.zero;
    private int _pass = 0;
    public List<GameObject> _conections = new List<GameObject>();
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

    [Header("Pathfinding")]
    public int _gCost = 0;
    public int _hCost = 0;
    public int fCost = 0;
    public CellCable _cameFromCell;

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
    /// <summary>
    /// Chenge the gameObject rotation using <see cref="_cellRotation"/>
    /// </summary>
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
    /// <summary>
    /// Changes cable status according to possible connections
    /// </summary>
    /// <param name="pos">Current possition</param>
    /// <param name="isInit">Is true if it is an initial position</param>
    public void SetNewState(Vector2 pos, bool isInit)
    {
        _pass++;
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
                if (isInit)
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
    /// <summary>
    /// Changes the size of the gameObject according to the grid size
    /// </summary>
    /// <param name="dim">Grid size</param>
    public void SetSize(int dim)
    {
        foreach (GameObject currentCable in _cableList)
        {
            switch (dim)
            {
                case 3:
                    currentCable.transform.localScale = new Vector2(60, 60);
                    break;
                case 4:
                    currentCable.transform.localScale = new Vector2(65, 65);
                    break;
                case 5:
                    currentCable.transform.localScale = new Vector2(50, 50);
                    break;
                case 6:
                    currentCable.transform.localScale = new Vector2(45, 45);
                    break;
                case 7:
                    currentCable.transform.localScale = new Vector2(40, 40);
                    break;
                case 8:
                    currentCable.transform.localScale = new Vector2(35, 35);
                    break;
                case 9:
                    currentCable.transform.localScale = new Vector2(30, 30);
                    break;
                case 10:
                    currentCable.transform.localScale = new Vector2(25, 25);
                    break;
                default:
                    break;
            }
        }
    }
    /// <summary>
    /// Changes cable status according to possible connections
    /// </summary>
    /// <param name="prePos">Previous position</param>
    /// <param name="nextPos">Next position</param>
    public void SetNewState(Vector2 prePos, Vector2 nextPos)
    {
        _pass++;
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
        SetRandomRotation();
    }
    /// <summary>
    /// Set a random position to init the game
    /// </summary>
    private void SetRandomRotation()
    {
        _cellRotation = (ROTATION)UnityEngine.Random.Range(0, 3);
    }
    /// <summary>
    /// Rotate the cell (No longer in use)
    /// </summary>
    public void RotateCell()
    {
        foreach (GameObject currentCable in _conections)
        {
            currentCable.GetComponent<CellCable>().ClearConexions();
        }
        ClearConexions();

        int pos = (int)_cellRotation;
        pos++;
        if (pos > Enum.GetValues(typeof(ROTATION)).Length - 1)
        {
            pos = 0;
        }
        _cellRotation = (ROTATION)pos;
        foreach (GameObject currentCable in _cableList)
        {
            currentCable.SetActive(false);
        }
    }
    /// <summary>
    /// Clear all the conexion
    /// </summary>
    public void ClearConexions()
    {
        _conections.Clear();
    }
    #region GETS
    /// <summary>
    /// Returns the state
    /// </summary>
    /// <returns><see cref="_cellState"/></returns>
    public CELL_STATE GetCellState()
    {
        return _cellState;
    }
    /// <summary>
    /// Returns the rotation
    /// </summary>
    /// <returns><see cref="_cellRotation"/></returns>
    public ROTATION GetRotation()
    {
        return _cellRotation;
    }
    /// <summary>
    /// Returns the position
    /// </summary>
    /// <returns><see cref="_cellPos"/></returns>
    public Vector2 GetCellPos()
    {
        return _cellPos;
    }
    /// <summary>
    /// Returns the distance between the pos given and the <see cref="_cellPos"/>
    /// </summary>
    /// <param name="pos">Position to get distance</param>
    /// <returns>Distance</returns>
    public float GetDistance(Vector2 pos)
    {
        return Mathf.Abs(Vector2.Distance(pos, _cellPos));
    }
    /// <summary>
    /// Returns if is a init position
    /// </summary>
    /// <returns><see cref="_isInit"/></returns>
    public bool GetIsInit()
    {
        return _isInit;
    }
    /// <summary>
    /// Returns if is a finish position
    /// </summary>
    /// <returns><see cref="_isFinish"/></returns>
    public bool GetIsFinish()
    {
        return _isFinish;
    }
    #endregion
    #region SETS
    /// <summary>
    /// Set a new <see cref="CELL_STATE"/>
    /// </summary>
    /// <param name="state">new state</param>
    public void SetCellState(CELL_STATE state)
    {
        _cellState = state;
    }
    /// <summary>
    /// Set a new pos
    /// </summary>
    /// <param name="pos">new pos</param>
    public void SetCellPos(Vector2 pos)
    {
        _cellPos = pos;
    }
    /// <summary>
    /// Set if is init position to <see cref="_isInit"/>
    /// </summary>
    /// <param name="isInit">True if is init postion</param>
    public void SetIsInit(bool isInit)
    {
        _isInit = isInit;
    }
    /// <summary>
    /// Set if is finish position to <see cref="_isFinish"/>
    /// </summary>
    /// <param name="isFinish">True if is finish postion</param>
    public void SetIsFinish(bool isFinish)
    {
        _isFinish = isFinish;
    }
    /// <summary>
    /// Set a new collision and add the collision to <see cref="_conections"/>
    /// </summary>
    /// <param name="collision">new collision</param>
    public void SetCollision(GameObject collision)
    {
        if (!_conections.Contains(collision))
        {
            _conections.Add(collision);
        }
    }
    #endregion
}
