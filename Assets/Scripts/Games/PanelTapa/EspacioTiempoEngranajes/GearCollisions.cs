using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearCollisions : MonoBehaviour
{
    public CellCable _cable;
    #region COLLISION
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "CableCollider")
        {
            _cable.SetCollision(collision.GetComponent<CableCollisions>()._cable.gameObject);
        }
    }
    #endregion
}
