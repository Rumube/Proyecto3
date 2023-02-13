using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragParentPropieties : MonoBehaviour
{
    [Header("Configuration")]
    public List<Transform> _goodGameObjects;
    [Tooltip("If all ganeObjects can put in this place")]
    public bool _canAll;
    public bool _canParentNewItems = false;
    [Header("References")]
    private Transform _currentTransform;
    private GameObject _itemDrag;

    /// <summary>
    /// Checks if the give Transform can
    /// stay in the DragParent, and save in <see cref="_currentTransform"/>
    /// </summary>
    /// <param name="gameObjectToCheck">The Transform to check</param>
    /// <returns></returns>
    public bool CheckCanPut(Transform gameObjectToCheck)
    {
        if (_canParentNewItems)
        {
            _currentTransform = gameObjectToCheck;
        }
        else if(_goodGameObjects.Contains(gameObjectToCheck))
        {
            _currentTransform = gameObjectToCheck;
        }
        return _canParentNewItems;
    }
    /// <summary>
    /// Check if <see cref="_currentTransform"/> is a valid GameObject
    /// </summary>
    /// <returns></returns>
    public bool CheckIfIsCorrect()
    {
        bool result = false;
        foreach (Transform currentTransfom in _goodGameObjects)
        {
            if (_currentTransform == currentTransfom)
            {
                result = true;
            }
        }
        return result;
    }

    /// <summary>
    /// Chechks if the parent can add new items
    /// and return if can do it.
    /// </summary>
    /// <param name="newItem">Item to add</param>
    /// <returns>If the item is added</returns>
    public bool canAddItem(GameObject newItem)
    {
        bool itemAdded = false;
        if(_itemDrag == null && _canParentNewItems)
        {
            _itemDrag = newItem;
            itemAdded = true;
        }
        return itemAdded;
    }
    /// <summary>
    /// Remove if exits the item
    /// </summary>
    /// <param name="removedItem"></param>
    public void removeItem(GameObject removedItem)
    {
        if (_itemDrag == removedItem)
        {
            _itemDrag = null;
        }
    }
}
