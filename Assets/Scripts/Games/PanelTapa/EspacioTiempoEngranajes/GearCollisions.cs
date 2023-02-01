using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearCollisions : MonoBehaviour
{
    public CellGear _cable;
    #region COLLISION
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "GearCollider")
        {
            _cable.SetCollision(collision.GetComponent<GearCollisions>()._cable.gameObject);
        }
    }
    #endregion
}
