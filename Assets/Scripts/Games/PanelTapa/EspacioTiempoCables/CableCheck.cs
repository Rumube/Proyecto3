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

    public void CheckConection(PanelCablesCamino path, CellCable preCell = null)
    {
        //TODO: INTENTAR GUARDAR DONDE HAY MÁS CONEXIONES PARA VOLVER MÁS TARDE SI NO HAY CAMINOS
        //PROBAR ALGORITMO A*
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
            //foreach (GameObject currentCable in _cableData._conections)
            //{
            //    if (currentCable.GetComponent<CellCable>() != preCell && _count < 2)
            //    {
            //        currentCable.GetComponent<CableCheck>().CheckConection(path, _cableData);
            //    }
            //}
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
    public bool GetChecked()
    {
        return _checked;
    }
    public void SetCheked(bool value)
    {
        _checked = value;
    }
    public void ResetCount()
    {
        _count = 0;
        _overFlow = false;
    }
}
