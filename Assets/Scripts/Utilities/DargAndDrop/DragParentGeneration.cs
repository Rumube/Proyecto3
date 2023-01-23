using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragParentGeneration : DragParentPropieties
{
    [Header("Properties")]
    public GameObject _dragItem;
    public DragManager _dndManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.childCount <= 0)
        {
            GameObject newItem = Instantiate(_dragItem, transform);
            newItem.transform.position = transform.position;
            newItem.GetComponent<DragableItem>().SetProperties();
            newItem.GetComponent<DragableItem>().SetDndManager(_dndManager);
        }
    }
}
