using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CableCheck : MonoBehaviour
{
    private CellCable _cableData = null;
    private bool _checked = false;
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
        if (!_checked)
        {
            if (!_cableData.GetIsFinish())
            {
                if (preCell != null)
                {
                    foreach (GameObject currentCable in _cableData._conections)
                    {
                        if (currentCable != preCell || preCell.GetComponent<CableCheck>().GetChecked())
                        {
                            _checked = true;
                            currentCable.GetComponent<CableCheck>().CheckConection(path, _cableData);
                        }
                    }
                }
                else
                {
                    foreach (GameObject currentCable in _cableData._conections)
                    {
                        _checked = true;
                        currentCable.GetComponent<CableCheck>().CheckConection(path, _cableData);
                    }
                }
            }
            else
            {
                finish = true;
            }
        }
        path.FinishCheck(finish);
    }
    public bool GetChecked()
    {
        return _checked;
    }
    public void SetCheked(bool value)
    {
        _checked = value;
    }
}
