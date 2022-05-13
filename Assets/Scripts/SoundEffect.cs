using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffect : MonoBehaviour
{

    public AudioSource starSound;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // If the left mouse button is pressed down...
        if (Input.GetMouseButtonDown(0) == true)
        {
            GetComponent<AudioSource>().Play();
        }
        // If the left mouse button is released...
        if (Input.GetMouseButtonUp(0) == true)
        {
            GetComponent<AudioSource>().Stop();
        }
    }
}
