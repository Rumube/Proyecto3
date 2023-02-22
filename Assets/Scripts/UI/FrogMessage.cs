using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Crosstales.RTVoice;
using Crosstales.RTVoice.Model;
using TMPro;

public class FrogMessage : MonoBehaviour, IFrogMessage
{
    [Header("References")]
    private AudioSource _audio;
    private TextMeshProUGUI _textMessage;
    private Image _imageMassage;
    private Animator _minAnim;

    [Header("Min Configuration")]
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
        _minAnim = GameObject.FindGameObjectWithTag("Min").GetComponent<Animator>();
        _minAnim.Play("Idle");
    }

    // Update is called once per frame
    void Update()
    {
        if (_messagePile.Count >= 1 && !_messageAtive)
        {
            //TODO: Delete duplicate messages
            _messageAtive = true;
            SendMessage(_messagePile[0]);
            _messagePile.Remove(_messagePile[0]);
        }
    }
    /// <summary>
    /// Start the process to give the message
    /// </summary>
    /// <param name="message">The message to give</param>
    private new void SendMessage(string message)
    {
        _textMessage = GameObject.FindGameObjectWithTag("OrderText").GetComponent<TextMeshProUGUI>();
        _imageMassage = GameObject.FindGameObjectWithTag("OrderImage").GetComponent<Image>();
        _minAnim = GameObject.FindGameObjectWithTag("Min").GetComponent<Animator>();
        StartCoroutine(FrogCoroutine(message));
    }
    /// <summary>
    /// Return the animation to Idle when the message is complet
    /// </summary>
    private void MessageComplete(Wrapper wrapper)
    {
        if (_minAnim != null)
        {
            _minAnim.Play("Idle");
            StartCoroutine(CloseMessage());
        }
    }
    /// <summary>
    /// Return the animation to Idle when the message is complet
    /// </summary>
    private void MessageComplete()
    {
        if (_minAnim != null)
        {
            _minAnim.Play("Idle");
            StartCoroutine(CloseMessage());
        }
    }
    public void NewFrogMessage(string message)
    {
        _messagePile.Add(message);
    }
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
        
        _minAnim.Play("Talking");
        Speaker.Instance.Speak(message, _audio, Speaker.Instance.VoiceForCulture("es-Es"), true, 0.9f, 1f, 1f, "", true);
        _textMessage.SetText("");
        _imageMassage.GetComponent<Animator>().Play("Appear");

        string currentMessage = "";
        char[] textSplit = message.ToCharArray();
        float waitTime = _timeWriting / textSplit.Length;

        for (int i = 0; i < textSplit.Length; i++)
        {
            currentMessage += textSplit[i];
            _textMessage.SetText(currentMessage);
            yield return new WaitForSeconds(waitTime);
        }
    }
    public void RepeatLastOrder()
    {
        if(_lastOrder == "")
        {
            _lastOrder = "No hay �rdenes";
        }
        NewFrogMessage(_lastOrder);
    }
    /// <summary>
    /// Close the message panel
    /// </summary>
    IEnumerator CloseMessage()
    {
        yield return new WaitForSeconds(_messageDuration);
        _textMessage.text = "";
        _imageMassage.GetComponent<Animator>().Play("Disappear");
        _messageAtive = false;
    }
    public void StopFrogSpeaker()
    {
        Speaker.Instance.Silence();
    }
    public bool GetMessageAtive()
    {
        return _messageAtive;
    }
}
