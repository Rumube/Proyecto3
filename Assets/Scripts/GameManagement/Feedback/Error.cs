using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Error : MonoBehaviour, IError
{
    [Header("Error Configuration")]
    public float _failDuration;
    public float _shakeAmount = 0.7f;
    public AudioClip _clip;
    private AudioSource _audio;

    [Header("Error Messages")]
    public List<string> _errorMessages;


    private void Start()
    {
        _audio = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Generates the error feedback
    /// </summary>
    public void GenerateError()
    {
        AudioManagement();
        GetComponent<CameraShake>().StartShake(_failDuration, _shakeAmount);
        StartCoroutine(StartFrogMessage());
    }

    /// <summary>
    /// Set the audio clip and play the sound
    /// </summary>
    private void AudioManagement()
    {
        _audio.clip = _clip;
        _audio.Play();
    }

    /// <summary>
    /// Waits to the sound finish
    /// </summary>
    IEnumerator StartFrogMessage()
    {
        yield return new WaitForSeconds(1f);
        ServiceLocator.Instance.GetService<IFrogMessage>().NewFrogMessage(_errorMessages[Random.Range(0, _errorMessages.Count)]);
    }
}
