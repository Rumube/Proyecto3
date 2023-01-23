using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragParentPropieties : MonoBehaviour
{
    [Header("Configuration")]
    public List<Transform> _goodGameObjects;
    [Tooltip("If all ganeObjects can put in this place")]
    public bool _canAll;
    private Transform _currentTransform;
    public bool _canParentNewItems = false;

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
}
