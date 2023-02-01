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
    private CellCable _currentCell = null;
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
            _initPos.GetComponent<CellCable>().SetCellState(CellCable.CELL_STATE.EMPTY);
            _finishPos.GetComponent<CellCable>().SetCellState(CellCable.CELL_STATE.EMPTY);
        }
        _initPos = GetComponent<PanelCablesGenerateGrid>().GetCell(new Vector2(Random.Range(0, _dim), 0));
        _finishPos = GetComponent<PanelCablesGenerateGrid>().GetCell(new Vector2(Random.Range(0, _dim), _dim - 1));
        _initPos.GetComponent<CellCable>().SetCellState(CellCable.CELL_STATE.RECTO);
        _finishPos.GetComponent<CellCable>().SetCellState(CellCable.CELL_STATE.CUATRO);
        _initPos.GetComponent<CellCable>().SetIsInit(true);
        _finishPos.GetComponent<CellCable>().SetIsFinish(true);
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
            GameObject newCell = GetComponent<PanelCablesGenerateGrid>().GetCell(newPos);
            _cellPath.Add(newCell);
            newCell.GetComponent<CellCable>().SetCellState(CellCable.CELL_STATE.RECTO);
        }
        _cellPath.Add(_finishPos);
    }
    private bool CheckIfIsEmpty(Vector2 pos)
    {
        bool result = false;
        if (GetComponent<PanelCablesGenerateGrid>().GetCell(pos).GetComponent<CellCable>().GetCellState() == CellCable.CELL_STATE.EMPTY)
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
        _currentCell = _initPos.GetComponent<CellCable>();
        _correctPath.Add(_currentCell.gameObject);
        do
        {
            List<CellCable> adjacentCells = GetAdjacentCells(_currentCell);

            float min = 9000f;
            CellCable closeCell = null;

            foreach (CellCable currentCable in adjacentCells)
            {
                float distance = currentCable.GetDistance(_cellPath[nextPathValue].GetComponent<CellCable>().GetCellPos());
                if (distance < min)
                {
                    closeCell = currentCable;
                    min = distance;
                }
            }
            closeCell.SetCellState(CellCable.CELL_STATE.RECTO);
            _currentCell = closeCell;
            _correctPath.Add(_currentCell.gameObject);
            if (_cellPath.Contains(_currentCell.gameObject) && _currentCell != _cellPath[_cellPath.Count - 1].GetComponent<CellCable>() && nextPathValue < _cellPath.Count - 1)
            {
                nextPathValue++;
            }

        } while (_currentCell != _cellPath[_cellPath.Count - 1].GetComponent<CellCable>());
    }
    private List<CellCable> GetAdjacentCells(CellCable cell)
    {
        List<CellCable> adjacentCells = new List<CellCable>();
        Vector2 upCell = new Vector2(cell.GetCellPos().x - 1, cell.GetCellPos().y);
        Vector2 downCell = new Vector2(cell.GetCellPos().x + 1, cell.GetCellPos().y);
        Vector2 leftCell = new Vector2(cell.GetCellPos().x, cell.GetCellPos().y - 1);
        Vector2 rightCell = new Vector2(cell.GetCellPos().x, cell.GetCellPos().y + 1);
        if (ExistsInGrid(upCell))
        {
            adjacentCells.Add(GetComponent<PanelCablesGenerateGrid>().GetCell(upCell).GetComponent<CellCable>());
        }
        if (ExistsInGrid(downCell))
        {
            adjacentCells.Add(GetComponent<PanelCablesGenerateGrid>().GetCell(downCell).GetComponent<CellCable>());
        }
        if (ExistsInGrid(leftCell))
        {
            adjacentCells.Add(GetComponent<PanelCablesGenerateGrid>().GetCell(leftCell).GetComponent<CellCable>());
        }
        if (ExistsInGrid(rightCell))
        {
            adjacentCells.Add(GetComponent<PanelCablesGenerateGrid>().GetCell(rightCell).GetComponent<CellCable>());
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
            if (_correctPath[i].GetComponent<CellCable>().GetIsInit())
            {
                _correctPath[i].GetComponent<CellCable>().SetNewState(_correctPath[i + 1].GetComponent<CellCable>().GetCellPos(), true);
            }
            else if (_correctPath[i].GetComponent<CellCable>().GetIsFinish())
            {
                _correctPath[i].GetComponent<CellCable>().SetNewState(_correctPath[i - 1].GetComponent<CellCable>().GetCellPos(), false);
            }
            else
            {
                _correctPath[i].GetComponent<CellCable>().SetNewState(_correctPath[i - 1].GetComponent<CellCable>().GetCellPos(), _correctPath[i + 1].GetComponent<CellCable>().GetCellPos());
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
        //_correctPath[0].GetComponent<CableCheck>().CheckConection(this);
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
            currentCable.GetComponent<CableCheck>().SetCheked(false);
        }
    }
}
