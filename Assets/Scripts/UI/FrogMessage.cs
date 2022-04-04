using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Crosstales.RTVoice;

public class FrogMessage : MonoBehaviour, IFrogMessage
{
    public AudioSource _audio;
    // Start is called before the first frame update
    void Start()
    {
        _audio = GetComponent<AudioSource>();
        Speaker.Instance.VoiceForCulture("es-es-x-eea-local");
        //Speaker.Instance.VoiceForCulture("ES-es");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NewFrogMessage(string message, float time, Text text)
    {
        StopAllCoroutines();
        StartCoroutine(FrogCoroutine(message, time, text));
    }

    private IEnumerator FrogCoroutine(string message, float time, Text text)
    {
        //MOSTRAR UI

        //AUDIO
        _audio.clip = null;
        Speaker.Instance.Speak(message, _audio);
        text.text = "";
        string currentMessage = "";
        char[] textSplit = message.ToCharArray();
        float waitTime = time / textSplit.Length;

        for (int i = 0; i < textSplit.Length; i++)
        {
            currentMessage += textSplit[i];
            text.text = currentMessage;

            yield return new WaitForSeconds(waitTime);
        }
        StartCoroutine(CloseMessage(time, text));
    }


    IEnumerator CloseMessage(float time, Text text)
    {
        yield return new WaitForSeconds(time);
    }
}
