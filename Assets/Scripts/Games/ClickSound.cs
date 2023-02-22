using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickSound : MonoBehaviour
{
    public AudioSource font;
    public AudioClip clip;

    // Start is called before the first frame update
    void Start()
    {
        font.clip = clip;
    }
    /// <summary>
    /// run the clip
    /// </summary>
    public void Play()
    {
        font.Play();
    }
}
