using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearCheck : MonoBehaviour
{
    private CellGear _gearData = null;
    private bool _checked = false;
    // Start is called before the first frame update
    void Start()
    {
        _gearData = GetComponent<CellGear>();
    }

    public void CheckConection(PanelGearPath path, CellGear preCell = null)
    {
        //TODO: INTENTAR GUARDAR DONDE HAY MÁS CONEXIONES PARA VOLVER MÁS TARDE SI NO HAY CAMINOS
        //PROBAR ALGORITMO A*
        bool finish = false;
        if (!_checked)
        {
            if (!_gearData.GetIsFinish())
            {
                if (preCell != null)
                {
                    foreach (GameObject currentGear in _gearData._conections)
                    {
                        if (currentGear != preCell || preCell.GetComponent<GearCheck>().GetChecked())
                        {
                            _checked = true;
                            currentGear.GetComponent<GearCheck>().CheckConection(path, _gearData);
                        }
                    }
                }
                else
                {
                    foreach (GameObject currentGear in _gearData._conections)
                    {
                        _checked = true;
                        currentGear.GetComponent<GearCheck>().CheckConection(path, _gearData);
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
