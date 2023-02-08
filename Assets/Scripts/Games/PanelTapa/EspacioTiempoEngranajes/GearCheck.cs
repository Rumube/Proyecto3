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
