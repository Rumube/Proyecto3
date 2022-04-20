using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Crosstales.RTVoice;
using Crosstales.RTVoice.Model;

public class FrogMessage : MonoBehaviour, IFrogMessage
{
    AudioSource _audio;
    Text _textMessage;
    Image _imageMassage;
    [SerializeField]
    public List<string> _messagePile = new List<string>();
    private bool _messageAtive;

    public float _timeWriting;
    public float _messageDuration;
    private string _lastOrder;

    // Start is called before the first frame update
    void Start()
    {
        _audio = GetComponent<AudioSource>();
        Speaker.Instance.VoiceForCulture("es-Es");
        _audio.pitch = 1.2f;
        _messageAtive = false;
        Speaker.Instance.OnSpeakComplete += MessageComplete;
    }

    // Update is called once per frame
    void Update()
    {
        if (_messagePile.Count >= 1 && !_messageAtive)
        {
            //if(_messagePile.Count > 1)
            //{
            //    int count = 0;
            //    List<string> deleteList = new List<string>(_messagePile);
            //    while (_messagePile[0] == _messagePile[count])
            //    {
            //        deleteList.Remove(_messagePile[count]);
            //        count++;
            //    }
            //    _messagePile = new List<string>(deleteList);
            //}

            _messageAtive = true;
            SendMessage(_messagePile[0]);
            _messagePile.Remove(_messagePile[0]);
        }
    }


    private void SendMessage(string message)
    {
        _textMessage = GameObject.FindGameObjectWithTag("OrderText").GetComponent<Text>();
        _imageMassage = GameObject.FindGameObjectWithTag("OrderImage").GetComponent<Image>();
        StartCoroutine(FrogCoroutine(message));
    }
    private void MessageComplete(Wrapper wrapper)
    {
        StartCoroutine(CloseMessage());
    }
    /// <summary>
    /// Start the messages process
    /// </summary>
    /// <param name="message">The message text</param>
    public void NewFrogMessage(string message)
    {
        _messagePile.Add(message);
    }
    /// <summary>
    /// Start the messages process and save the message in <see cref="_lastOrder"/>
    /// </summary>
    /// <param name="message">The message text</param>
    /// <param name="isOrder">The message is an order</param>
    public void NewFrogMessage(string message, bool isOrder)
    {
        if (isOrder)
            _lastOrder = message;
        _messagePile.Add(message);
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
        _messageAtive = false;
    }
}
