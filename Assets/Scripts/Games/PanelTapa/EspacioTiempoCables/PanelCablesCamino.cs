using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelCablesCamino : MonoBehaviour
{
    [Header("PathValues")]
    public GameObject _initPos;
    public GameObject _finishPos;
    public List<GameObject> _cellPath = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void GenerateNewPaht(int _dim)
    {
        if(_initPos != null)
        {
            _initPos.GetComponent<CellCable>().SetCellState(CellCable.CELL_STATE.EMPY);
            _finishPos.GetComponent<CellCable>().SetCellState(CellCable.CELL_STATE.EMPY);
        }
        _initPos = GetComponent<PanelCablesGenerateGrid>().GetCell(new Vector2 (Random.Range(0, _dim), 0));
        _finishPos = GetComponent<PanelCablesGenerateGrid>().GetCell(new Vector2(Random.Range(0, _dim), _dim-1));
        _initPos.GetComponent<CellCable>().SetCellState(CellCable.CELL_STATE.RECTO);
        _finishPos.GetComponent<CellCable>().SetCellState(CellCable.CELL_STATE.CUATRO);
    }
}
