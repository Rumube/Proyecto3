using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CableCheck : MonoBehaviour
{
    private CellCable _cableData = null;
    // Start is called before the first frame update
    void Start()
    {
        _cableData = GetComponent<CellCable>();
    }

    public bool CheckConection(CellCable preCell = null)
    {
        bool finish = false;
        if (!_cableData.GetIsFinish())
        {
            if (preCell != null)
            {
                foreach (GameObject currentCable in _cableData._conections)
                {
                    if (currentCable != preCell)
                    {
                        finish = currentCable.GetComponent<CableCheck>().CheckConection(_cableData);
                    }
                }
            }
            else
            {
                foreach (GameObject currentCable in _cableData._conections)
                {
                    finish = currentCable.GetComponent<CableCheck>().CheckConection(_cableData);
                }
            }
        }
        else
        {
            return true;
        }
        return finish;
    }
}
