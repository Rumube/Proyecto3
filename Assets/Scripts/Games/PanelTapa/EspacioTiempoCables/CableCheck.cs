using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CableCheck : MonoBehaviour
{
    private CellCable _cableData = null;
    private bool _checked = false;
    public int _count = 0;
    private bool _overFlow = false;
    // Start is called before the first frame update
    void Start()
    {
        _cableData = GetComponent<CellCable>();
    }
    /// <summary>
    /// Check if the connections made by the wires reach the end of the cable run
    /// </summary>
    /// <param name="path"><see cref="PanelCablesCamino"/></param>
    /// <param name="preCell">Previous cell</param>
    public void CheckConection(PanelCablesCamino path, CellCable preCell = null)
    {
        bool finish = false;
        _count++;
        if(_count >= _cableData._conections.Count && _overFlow)
        {
            return;
        }else if(_count > _cableData._conections.Count)
        {
            _count = 1;
            _overFlow = true;
        }

        if (!_cableData.GetIsFinish())
        {
            if (_cableData._conections[_count - 1].GetComponent<CellCable>() != preCell)
            {
                _cableData._conections[_count - 1].GetComponent<CableCheck>().CheckConection(path, _cableData);
            }
            else if(!_cableData.GetIsInit())
            {
                CheckConection(path, preCell);
            }
        }
        else
        {//FINAL
            finish = true;
        }
        if (finish)
        {
            path.setCorrect(true);
        }
    }
    /// <summary>
    /// Returns if the cell has cheked
    /// </summary>
    /// <returns></returns>
    public bool GetChecked()
    {
        return _checked;
    }
    /// <summary>
    /// Set the value of <see cref="_checked"/>
    /// </summary>
    /// <param name="value">The new value</param>
    public void SetCheked(bool value)
    {
        _checked = value;
    }
    /// <summary>
    /// Reset the values of <see cref="_count"/> and <see cref="_overFlow"/>
    /// </summary>
    public void ResetCount()
    {
        _count = 0;
        _overFlow = false;
    }
}
