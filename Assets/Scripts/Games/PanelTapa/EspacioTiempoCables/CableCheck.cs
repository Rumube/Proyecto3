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
        bool finish = false;
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
                    currentCable.GetComponent<CableCheck>().CheckConection(path, _cableData);
                }
            }
        }
        else
        {
            finish = true;
        }
        if (finish)
        {
            path.FinishCheck(finish);
        }
    }
    public bool GetChecked()
    {
        return _checked;
    }
}
