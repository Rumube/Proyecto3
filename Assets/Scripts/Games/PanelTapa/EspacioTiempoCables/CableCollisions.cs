using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CableCollisions : MonoBehaviour
{
    public CellCable _cable;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #region COLLISION
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "CableCollider")
        {
            print("Enter: " + collision.name);
            _cable.SetCollision(collision.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "CableCollider")
        {
            print("Exit: " + collision.name);
            _cable.SetExit(collision.gameObject);
        }
    }
    #endregion
}
