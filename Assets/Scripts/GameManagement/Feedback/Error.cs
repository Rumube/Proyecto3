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

    [Header("References")]
    private Animator _panelAnim;
    private Animator _minAnim;


    private void Start()
    {
        _audio = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Generates the error feedback
    /// </summary>
    public void GenerateError()
    {
        _minAnim = GameObject.FindGameObjectWithTag("Min").GetComponent<Animator>();
        _panelAnim = GameObject.FindGameObjectWithTag("CanvasPanel").GetComponent<Animator>();
        AudioManagement();
        GetComponent<CameraShake>().StartShake(_failDuration, _shakeAmount);
        _panelAnim.Play("Vibration");
        _minAnim.Play("Surprised");
        ServiceLocator.Instance.GetService<IFrogMessage>().NewFrogMessage(_errorMessages[Random.Range(0, _errorMessages.Count)]);
        StartCoroutine(FinishFail());
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
    IEnumerator FinishFail()
    {
        yield return new WaitForSeconds(_failDuration);
        if (ServiceLocator.Instance.GetService<IFrogMessage>().GetMessageAtive())
        {
            _minAnim.Play("Talking");
        }
        else
        {
            _minAnim.Play("Idle");
        }
    }
}
