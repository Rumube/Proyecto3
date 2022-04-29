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
    private bool _isInGame;

    // Start is called before the first frame update
    void Start()
    {
        _audio = GetComponent<AudioSource>();
        Speaker.Instance.VoiceForCulture("es-Es");
        _audio.pitch = 1.2f;
        _messageAtive = false;
        Speaker.Instance.OnSpeakComplete += MessageComplete;
        _minAnim = GameObject.FindGameObjectWithTag("Min").GetComponent<Animator>();
        _minAnim.Play("MinIdle");
    }

    // Update is called once per frame
    void Update()
    {
        if (_messagePile.Count >= 1 && !_messageAtive)
        {
            //TODO: Delete duplicate messages

            //List<int> toDelete = new List<int>();
            //for (int i = 1; i < _messagePile.Count; i++)
            //{
            //    if(_messagePile[0] == _messagePile[i])
            //    {
            //        toDelete.Add(i);
            //    }
            //}

            //for (int i = 0; i < toDelete.Count; i++)
            //{
            //    _messagePile.RemoveAt(toDelete[i]);
            //}
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


    private new void SendMessage(string message)
    {
        _textMessage = GameObject.FindGameObjectWithTag("OrderText").GetComponent<TextMeshProUGUI>();
        _imageMassage = GameObject.FindGameObjectWithTag("OrderImage").GetComponent<Image>();
        _minAnim = GameObject.FindGameObjectWithTag("Min").GetComponent<Animator>();
        StartCoroutine(FrogCoroutine(message));
    }
    private void MessageComplete(Wrapper wrapper)
    {
        if (_minAnim != null)
        {
            _minAnim.Play("MinIdle");
            StartCoroutine(CloseMessage());
        }
    }
    private void MessageComplete()
    {
        if (_minAnim != null)
        {
            _minAnim.Play("MinIdle");
            StartCoroutine(CloseMessage());
        }
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
    public void NewFrogMessage(string message, bool isOrder, bool isGame = true)
    {
        if (isOrder)
            _lastOrder = message;
        _messagePile.Add(message);
        _isInGame = isGame; 
    }

    /// <summary>
    /// Makes the message and opens the panel
    /// </summary>
    /// <param name="message">The message text</param>
    private IEnumerator FrogCoroutine(string message)
    {
        int randomAnim = Random.Range(0, 3);
        string animName = "";
        switch (randomAnim)
        {
            case 0:
                animName = "MinTalk1";
                break;
            case 1:
                animName = "MinTalk2";
                break;
            case 2:
                animName = "MinTalk3";
                break;
            default:
                break;
        }
        _minAnim.Play(animName);
        if(_isInGame)
            Speaker.Instance.Speak(message, _audio, Speaker.Instance.VoiceForCulture("es-Es"), true, 0.9f, 1f, 1f, "", true);
        _textMessage.SetText("");
        _imageMassage.color = new Color(0, 0, 0, 1);

        string currentMessage = "";
        char[] textSplit = message.ToCharArray();
        float waitTime = _timeWriting / textSplit.Length;

        for (int i = 0; i < textSplit.Length; i++)
        {
            currentMessage += textSplit[i];
            _textMessage.SetText(currentMessage);
            yield return new WaitForSeconds(waitTime);
        }
        //if (!_isInGame)
        //{
        //    yield return new WaitForSeconds(3.0f);
        //    MessageComplete();
        //    StopFrogSpeaker();
        //}
       
    }
    /// <summary>
    /// Repeat the last order gived by Min
    /// </summary>
    public void RepeatLastOrder()
    {
        if(_lastOrder == "")
        {
            _lastOrder = "No hay órdenes";
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
        _imageMassage.color = new Color(0, 0, 0, 0);
        _messageAtive = false;
    }

    public void StopFrogSpeaker()
    {
        Speaker.Instance.Silence();
    }
}
