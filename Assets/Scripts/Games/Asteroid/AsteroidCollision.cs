using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidCollision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        print("Hola");
        EDebug.Log("hOLIWIS");
        if(collision.gameObject.GetComponent<Asteroid>() != null)
        {
            EDebug.Log("Colisión Asteroide");
        }
    }

    /*
    private void OnCollisionEnter(Collision collision)
    {
        print("Hola");
        EDebug.Log("hOLIWIS");
    }
    */

}
