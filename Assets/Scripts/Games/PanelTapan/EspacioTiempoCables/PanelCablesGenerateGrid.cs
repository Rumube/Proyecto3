using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelCablesGenerateGrid : MonoBehaviour
{
    [Header("Properties")]
    [Tooltip("Grid dimensions 4 = 4x4")]
    public int _dim = 0;
    [Header("References")]
    public GameObject _gridParent;
    public GameObject _cell;
    private List<GameObject> _cellList = new List<GameObject>();
    [Header("Sizes")]
    private GridLayoutGroup _layoutGroup; //Component
    // Start is called before the first frame update
    void Start()
    {
        SetSize();
        GenerateGrid();
    }
    private void SetSize()
    {
        _layoutGroup = _gridParent.GetComponent<GridLayoutGroup>();
    }
    // Update is called once per frame
    void Update()
    {

    }
    public void GenerateGrid()
    {
        GridLayoutGroup glg = _gridParent.GetComponent<GridLayoutGroup>();
        glg.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        glg.constraintCount = _dim;

        for (int i = 0; i < _dim; i++)
        {
            for (int j = 0; j < _dim; j++)
            {
                GameObject newCell = Instantiate(_cell, _gridParent.transform);
                _cellList.Add(newCell);
            }
        }
        ReSizeCells();
    }
    private void ReSizeCells()
    {
        Vector2 screenSize = new Vector2(_gridParent.GetComponent<RectTransform>().rect.width, _gridParent.GetComponent<RectTransform>().rect.height); // Current screen size
        float yValue = screenSize.y / _dim;
        _layoutGroup.cellSize = new Vector2(yValue, yValue);
    }
}
