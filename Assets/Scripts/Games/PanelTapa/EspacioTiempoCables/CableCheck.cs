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
        
        if (!_cableData.GetIsFinish())
        {
            foreach (GameObject currentCable in _cableData._conections)
            {
                print("P: " + _cableData.name);
                 //|| preCell.GetComponent<CableCheck>().GetChecked()
                if (currentCable != preCell)
                {
                    currentCable.GetComponent<CableCheck>().CheckConection(path, _cableData);
                }
            }
            //if (preCell != null)
            //{//OTROS

            //}
            //else
            //{//INICIO
            //    foreach (GameObject currentCable in _cableData._conections)
            //    {
            //        currentCable.GetComponent<CableCheck>().CheckConection(path, _cableData);
            //    }
            //}
        }
        else
        {//FINAL
            finish = true;
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
