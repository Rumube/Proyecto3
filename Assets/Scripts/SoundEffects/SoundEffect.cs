using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffect : MonoBehaviour
{

    public GameObject starSound;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0)) 
        {
            Instantiate(starSound);
        }
    }
}
