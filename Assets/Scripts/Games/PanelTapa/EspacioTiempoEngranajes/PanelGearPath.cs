using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelGearPath : MonoBehaviour
{
    [Header("Grid Values")]
    private int _dim = 0;
    [Header("PathValues")]
    public GameObject _initPos;
    public GameObject _finishPos;
    public List<GameObject> _cellPath = new List<GameObject>();
    private int _middlePoints = 0;
    public List<GameObject> _correctPath = new List<GameObject>();
    [Header("GeneratePathValues")]
    private CellGear _currentCell = null;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    /// <summary>
    /// Generate the grid
    /// </summary>
    /// <param name="dim">Grid size</param>
    public void GenerateNewPaht(int dim)
    {
        _dim = dim;
        GenerateStartAndFinishPos();
        GenerateRandomPoints();
        GeneratePath();
        SetDirection();
        //AÑADIR CAMINOS FALSOS
        //RANDOMIZAR ROTACIONES
        ChangeNames();
    }
    #region GenerateStartAndFinishPos
    private void GenerateStartAndFinishPos()
    {
        if (_initPos != null)
        {
            _initPos.GetComponent<CellGear>().SetCellState(CellGear.CELL_STATE.EMPTY);
            _finishPos.GetComponent<CellGear>().SetCellState(CellGear.CELL_STATE.EMPTY);
        }
        _initPos = GetComponent<PanelGearGenerateGrid>().GetCell(new Vector2(Random.Range(0, _dim), 0));
        _finishPos = GetComponent<PanelGearGenerateGrid>().GetCell(new Vector2(Random.Range(0, _dim), _dim - 1));
        _initPos.GetComponent<CellGear>().SetCellState(CellGear.CELL_STATE.RECTO);
        _finishPos.GetComponent<CellGear>().SetCellState(CellGear.CELL_STATE.CUATRO);
        _initPos.GetComponent<CellGear>().SetIsInit(true);
        _finishPos.GetComponent<CellGear>().SetIsFinish(true);
        _cellPath.Add(_initPos);
    }
    #endregion
    #region GenerateRandomPoints
    private void GenerateRandomPoints()
    {
        SetMiddlePoints();
        for (int i = 0; i < _middlePoints; i++)
        {
            Vector2 newPos = Vector2.zero;
            do
            {
                newPos = new Vector2(Random.Range(0, _dim - 1), Random.Range(0, _dim - 1));
            } while (!CheckIfIsEmpty(newPos));
            GameObject newCell = GetComponent<PanelGearGenerateGrid>().GetCell(newPos);
            _cellPath.Add(newCell);
            newCell.GetComponent<CellGear>().SetCellState(CellGear.CELL_STATE.RECTO);
        }
        _cellPath.Add(_finishPos);
    }
    private bool CheckIfIsEmpty(Vector2 pos)
    {
        bool result = false;
        if (GetComponent<PanelGearGenerateGrid>().GetCell(pos).GetComponent<CellGear>().GetCellState() == CellGear.CELL_STATE.EMPTY)
        {
            result = true;
        }
        return result;
    }
    private void SetMiddlePoints()
    {
        switch (_dim)
        {
            case 3:
                _middlePoints = 1;
                break;
            case 4:
                _middlePoints = 2;
                break;
            case 5:
                _middlePoints = 4;
                break;
            case 6:
                _middlePoints = 5;
                break;
            case 7:
                _middlePoints = 5;
                break;
            case 8:
                _middlePoints = 6;
                break;
            case 9:
                _middlePoints = 8;
                break;
            case 10:
                _middlePoints = 10;
                break;
            default:
                break;
        }
    }
    #endregion
    #region GeneratePath
    private void GeneratePath()
    {
        int nextPathValue = 1;
        _currentCell = _initPos.GetComponent<CellGear>();
        _correctPath.Add(_currentCell.gameObject);
        do
        {
            List<CellGear> adjacentCells = GetAdjacentCells(_currentCell);

            float min = 9000f;
            CellGear closeCell = null;

            foreach (CellGear currentGear in adjacentCells)
            {
                float distance = currentGear.GetDistance(_cellPath[nextPathValue].GetComponent<CellGear>().GetCellPos());
                if (distance < min)
                {
                    closeCell = currentGear;
                    min = distance;
                }
            }
            closeCell.SetCellState(CellGear.CELL_STATE.RECTO);
            _currentCell = closeCell;
            _correctPath.Add(_currentCell.gameObject);
            if (_cellPath.Contains(_currentCell.gameObject) && _currentCell != _cellPath[_cellPath.Count - 1].GetComponent<CellGear>() && nextPathValue < _cellPath.Count - 1)
            {
                nextPathValue++;
            }

        } while (_currentCell != _cellPath[_cellPath.Count - 1].GetComponent<CellGear>());
    }
    private List<CellGear> GetAdjacentCells(CellGear cell)
    {
        List<CellGear> adjacentCells = new List<CellGear>();
        Vector2 upCell = new Vector2(cell.GetCellPos().x - 1, cell.GetCellPos().y);
        Vector2 downCell = new Vector2(cell.GetCellPos().x + 1, cell.GetCellPos().y);
        Vector2 leftCell = new Vector2(cell.GetCellPos().x, cell.GetCellPos().y - 1);
        Vector2 rightCell = new Vector2(cell.GetCellPos().x, cell.GetCellPos().y + 1);
        if (ExistsInGrid(upCell))
        {
            adjacentCells.Add(GetComponent<PanelGearGenerateGrid>().GetCell(upCell).GetComponent<CellGear>());
        }
        if (ExistsInGrid(downCell))
        {
            adjacentCells.Add(GetComponent<PanelGearGenerateGrid>().GetCell(downCell).GetComponent<CellGear>());
        }
        if (ExistsInGrid(leftCell))
        {
            adjacentCells.Add(GetComponent<PanelGearGenerateGrid>().GetCell(leftCell).GetComponent<CellGear>());
        }
        if (ExistsInGrid(rightCell))
        {
            adjacentCells.Add(GetComponent<PanelGearGenerateGrid>().GetCell(rightCell).GetComponent<CellGear>());
        }
        return adjacentCells;
    }
    private bool ExistsInGrid(Vector2 pos)
    {
        bool result = false;

        if (pos.x >= 0 && pos.x <= _dim - 1 && pos.y >= 0 && pos.y <= _dim - 1)
        {
            result = true;
        }

        return result;
    }
    #endregion
    #region SetDirections
    private void SetDirection()
    {
        for (int i = 0; i < _correctPath.Count; i++)
        {
            if (_correctPath[i].GetComponent<CellGear>().GetIsInit())
            {
                _correctPath[i].GetComponent<CellGear>().SetNewState(_correctPath[i + 1].GetComponent<CellGear>().GetCellPos(), true);
            }
            else if (_correctPath[i].GetComponent<CellGear>().GetIsFinish())
            {
                _correctPath[i].GetComponent<CellGear>().SetNewState(_correctPath[i - 1].GetComponent<CellGear>().GetCellPos(), false);
            }
            else
            {
                _correctPath[i].GetComponent<CellGear>().SetNewState(_correctPath[i - 1].GetComponent<CellGear>().GetCellPos(), _correctPath[i + 1].GetComponent<CellCable>().GetCellPos());
            }
        }
    }
    #endregion
    #region ChangeNames
    private void ChangeNames()
    {
        for (int i = 0; i < _correctPath.Count; i++)
        {
            if (i == 0)
            {
                _correctPath[i].name = _correctPath[i].name + "_INIT";
            }
            else if (i == _correctPath.Count - 1)
            {
                _correctPath[i].name = _correctPath[i].name + "_FINISH";
            }
            else
            {
                _correctPath[i].name = _correctPath[i].name + "_MIDDLE_" + i;
            }
        }
    }
    #endregion
    public void CheckPath()
    {
        _correctPath[0].GetComponent<GearCheck>().CheckConection(this);
    }
    public void FinishCheck(bool result)
    {
        UnCheckPath();
        if (result)
        {
            print("SI!!!");
        }
        else
        {
            print("NO!!!");
        }
    }
    private void UnCheckPath()
    {
        foreach (GameObject currentCable in _cellPath)
        {
            currentCable.GetComponent<GearCheck>().SetCheked(false);
        }
    }
}
