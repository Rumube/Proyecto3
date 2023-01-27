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
    #endregion
}
