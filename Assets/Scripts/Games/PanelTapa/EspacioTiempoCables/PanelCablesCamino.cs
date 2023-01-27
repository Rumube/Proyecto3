using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelCablesCamino : MonoBehaviour
{
    [Header("Grid Values")]
    private int _dim = 0;
    [Header("PathValues")]
    public GameObject _initPos;
    public GameObject _finishPos;
    public List<GameObject> _cellPath = new List<GameObject>();
    public int _middlePoints = 0;
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
        //GENERAR INIT AND FINISH
        GenerateStartAndFinishPos();
        //GENERAR PUNTOS INTERMEDIOS
        GenerateRandomPoints();
        //GENERAR CAMINO
        GeneratePath();
        //SETEAR DIRECIONES
        //SETEAR ROTATCION
        //AÑADIR CAMINOS FALSOS
        //RANDOMIZAR ROTACIONES
        //CAMBIAR NOMBRES
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
        _cellPath.Add(_initPos);
    }
    #endregion
    #region GenerateRandomPoints
    private void GenerateRandomPoints()
    {
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
    #endregion
    #region GeneratePath
    private void GeneratePath()
    {
        int nextPathValue = 1;
        _currentCell = _initPos.GetComponent<CellCable>();
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
            if (_cellPath.Contains(_currentCell.gameObject) && _currentCell != _cellPath[_cellPath.Count - 1].GetComponent<CellCable>())
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
    #region ChangeNames
    private void ChangeNames()
    {
        for (int i = 0; i < _cellPath.Count; i++)
        {
            if (i == 0)
            {
                _cellPath[i].name = _cellPath[i].name + "_INIT";
            }
            else if (i == _cellPath.Count - 1)
            {
                _cellPath[i].name = _cellPath[i].name + "_FINISH";
            }
            else
            {
                _cellPath[i].name = _cellPath[i].name + "_MIDDLE";
            }
        }
    }
    #endregion
}
