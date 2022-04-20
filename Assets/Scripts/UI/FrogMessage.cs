using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Crosstales.RTVoice;

public class FrogMessage : MonoBehaviour, IFrogMessage
{
    AudioSource _audio;
    Text _textMessage;
    Image _imageMassage;

    public float _timeWriting;
    public float _messageDuration;
    private string _lastOrder;

    // Start is called before the first frame update
    void Start()
    {
        _audio = GetComponent<AudioSource>();
        Speaker.Instance.VoiceForCulture("es-Es");
        _audio.pitch = 1.2f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Start the messages process
    /// </summary>
    /// <param name="message">The message text</param>
    public void NewFrogMessage(string message)
    {
        StopAllCoroutines();
        _textMessage = GameObject.FindGameObjectWithTag("OrderText").GetComponent<Text>();
        _imageMassage = GameObject.FindGameObjectWithTag("OrderImage").GetComponent<Image>();
        StartCoroutine(FrogCoroutine(message));
    }
    /// <summary>
    /// Start the messages process and save the message in <see cref="_lastOrder"/>
    /// </summary>
    /// <param name="message">The message text</param>
    /// <param name="isOrder">The message is an order</param>
    public void NewFrogMessage(string message, bool isOrder)
    {
        StopAllCoroutines();
        _textMessage = GameObject.FindGameObjectWithTag("OrderText").GetComponent<Text>();
        _imageMassage = GameObject.FindGameObjectWithTag("OrderImage").GetComponent<Image>();
        StartCoroutine(FrogCoroutine(message));
        if (isOrder)
            _lastOrder = message;
    }

    /// <summary>
    /// Makes the message and opens the panel
    /// </summary>
    /// <param name="message">The message text</param>
    private IEnumerator FrogCoroutine(string message)
    {
        Speaker.Instance.Speak(message, _audio, Speaker.Instance.VoiceForCulture("es-Es"), true, 0.9f, 1f, 1f, "", true);
        _textMessage.text = "";
        _imageMassage.color = new Color(0, 0, 0, 1);

        string currentMessage = "";
        char[] textSplit = message.ToCharArray();
        float waitTime = _timeWriting / textSplit.Length;

        for (int i = 0; i < textSplit.Length; i++)
        {
            currentMessage += textSplit[i];
            _textMessage.text = currentMessage;

            yield return new WaitForSeconds(waitTime);
        }
        StartCoroutine(CloseMessage());
    }
    /// <summary>
    /// Repeat the last order gived by Min
    /// </summary>
    public void RepeatLastOrder()
    {
        NewFrogMessage(_lastOrder);
    }

    /// <summary>
    /// Close the message panel
    /// </summary>
    IEnumerator CloseMessage()
    {
        yield return new WaitForSeconds(_messageDuration);
        _textMessage.text = "";
        _imageMassage.color = new Color(0, 0, 0, 0);
    }
}
