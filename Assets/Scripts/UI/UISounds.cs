using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISounds : MonoBehaviour
{
    public AudioSource _audioSource;
    public AudioClip _clip;

    private void Start()
    {
        if (_audioSource == null)
            _audioSource = GameObject.FindGameObjectWithTag("AudioEffectsManager").GetComponent<AudioSource>();
    }
    public void PlaySound()
    {
       _audioSource.clip = _clip;
       _audioSource.Play();
    }
}
