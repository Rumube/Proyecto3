using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellGear : MonoBehaviour
{
    [Header("Referenes")]
    public List<Sprite> _spriteList = new List<Sprite>();
    public List<GameObject> _gearList = new List<GameObject>();
    private Vector2 _cellPos = Vector2.zero;
    private int _pass = 0;
    public List<GameObject> _conections = new List<GameObject>();
    public enum CELL_STATE
    {
        EMPTY,
        CIRCLE,
        TRIANGLE,
        SQUARE,

    }

    public CELL_STATE _cellState = CELL_STATE.EMPTY;


    // Update is called once per frame
    void Update()
    {
        SetSprite();

    }
    /// <summary>
    /// Chenge the sprite using <see cref="_cellState"/> value
    /// </summary>
    private void SetSprite()
    {
        foreach (GameObject currentCable in _gearList)
        {
            currentCable.SetActive(false);
        }

        switch (_cellState)
        {
            case CELL_STATE.EMPTY:
                _gearList[0].SetActive(true);
                break;
            case CELL_STATE.TRIANGLE:
                _gearList[1].SetActive(true);
                break;
            case CELL_STATE.CIRCLE:
                _gearList[2].SetActive(true);
                break;
            case CELL_STATE.SQUARE:
                _gearList[3].SetActive(true);
                break;
            default:
                break;
        }
    }

    public void SetNewState(Vector2 pos, bool isInit)
    {
        _pass++;
        float posX = _cellPos.x - pos.x;
        float posY = _cellPos.y - pos.y;
     
    }
    public void SetSize(int dim)
    {
        foreach (GameObject currentGear in _gearList)
        {
            switch (dim)
            {
                case 3:
                    currentGear.transform.localScale = new Vector2(60, 60);
                    break;
                case 4:
                    currentGear.transform.localScale = new Vector2(65, 65);
                    break;
                case 5:
                    currentGear.transform.localScale = new Vector2(50, 50);
                    break;
                case 6:
                    currentGear.transform.localScale = new Vector2(45, 45);
                    break;
                case 7:
                    currentGear.transform.localScale = new Vector2(40, 40);
                    break;
                case 8:
                    currentGear.transform.localScale = new Vector2(35, 35);
                    break;
                case 9:
                    currentGear.transform.localScale = new Vector2(30, 30);
                    break;
                case 10:
                    currentGear.transform.localScale = new Vector2(25, 25);
                    break;
                default:
                    break;
            }
        }

    }
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
                _cellState = CELL_STATE.TRIANGLE;//PREGUNTAR A RUBEN SI CON ESTO PUEDO HACER QUE LOS DE ALREDEDOR COMPLETEN LA FIGURA
            }
            else
            {
                _cellState = CELL_STATE.SQUARE;
            }
        }
        else
        {
            _cellState = CELL_STATE.CIRCLE;
        }

    }


    public void ClearConexions()
    {
        _conections.Clear();
    }
    #region GETS
    public CELL_STATE GetCellState()
    {
        return _cellState;
    }

    public Vector2 GetCellPos()
    {
        return _cellPos;
    }
    public float GetDistance(Vector2 pos)
    {
        return Mathf.Abs(Vector2.Distance(pos, _cellPos));
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

    public void SetCollision(GameObject collision)
    {
        if (!_conections.Contains(collision))
        {
            _conections.Add(collision);
        }
    }
    #endregion
}
