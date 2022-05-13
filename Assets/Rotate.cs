using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public GameObject gearOne;

    // Update is called once per frame
    void Update()
    {
        gearOne.transform.Rotate(0, 1, 0);
    }
}
