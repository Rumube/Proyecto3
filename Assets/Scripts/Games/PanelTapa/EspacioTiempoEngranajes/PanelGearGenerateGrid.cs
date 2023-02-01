using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelGearGenerateGrid : MonoBehaviour
{
    [Header("Properties")]
    [Range(4,10)]
    [Tooltip("Grid dimensions 4 = 4x4")]
    public int _dim = 0;
    [Header("References")]
    public GameObject _gridParent;
    public GameObject _cell;
    private GameObject[,] _cellArr;
    [Header("Sizes")]
    private GridLayoutGroup _layoutGroup; //Component
    // Start is called before the first frame update
    void Start()
    {
        SetSize();
        Restart();
    }
    /// <summary>
    /// Init the size values
    /// </summary>
    private void SetSize()
    {
        _layoutGroup = _gridParent.GetComponent<GridLayoutGroup>();
        _cellArr = new GameObject[_dim, _dim];

    }
    /// <summary>
    /// Generate a grid of <see cref="_dim"/> rows and columns
    /// </summary>
    public void GenerateGrid()
    {
        _cellArr = new GameObject[_dim, _dim];
        GridLayoutGroup glg = _gridParent.GetComponent<GridLayoutGroup>();
        glg.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        glg.constraintCount = _dim;

        for (int i = 0; i < _dim; i++)
        {
            for (int j = 0; j < _dim; j++)
            {
                GameObject newCell = Instantiate(_cell, _gridParent.transform);
                newCell.name = "Cell_" + i + "x" + j;
                newCell.GetComponent<CellCable>().SetSize(_dim);
                _cellArr[i, j] = newCell;
                newCell.GetComponent<CellCable>().SetCellPos(new Vector2(i, j));
            }
        }
        ReSizeCells();
    }
    /// <summary>
    /// Change the cells size using the space
    /// </summary>
    private void ReSizeCells()
    {
        Vector2 screenSize = new Vector2(_gridParent.GetComponent<RectTransform>().rect.width, _gridParent.GetComponent<RectTransform>().rect.height); // Current screen size
        float yValue = screenSize.y / _dim;
        _layoutGroup.cellSize = new Vector2(yValue, yValue);
    }
    public void Restart()
    {
        DeleteCells();
        GenerateGrid();
        GetComponent<PanelCablesCamino>().GenerateNewPaht(_dim);
    }
    private void DeleteCells()
    {
        for (int i = 0; i < _dim; i++)
        {
            for (int j = 0; j < _dim; j++)
            {
                Destroy(_cellArr[i, j]);
            }
        }
    }
    /// <summary>
    /// Returns the cell in the position given
    /// </summary>
    /// <param name="pos">The position</param>
    /// <returns>The Cell</returns>
    public GameObject GetCell(Vector2 pos)
    {
        return _cellArr[(int)pos.x,(int)pos.y];
    }
}
