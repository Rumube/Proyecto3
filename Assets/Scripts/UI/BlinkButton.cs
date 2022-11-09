using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BlinkButton : MonoBehaviour
{
    [Header("Preferences")]
    public GameObject _student;
    public TextMeshProUGUI _textButton;

    public Sprite _openEye;
    public Sprite _closeEye;

    private bool _pressed = false;
    private Image _imgEye;

    // Start is called before the first frame update
    void Start()
    {
        _imgEye = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void btnPressed()
    {
        //Invertimos valor
        _pressed = !_pressed;

        if (_pressed)
        {
            //APRETADO
            _imgEye.sprite = _closeEye;
            ServiceLocator.Instance.GetService<IGameManager>().SetNotPresentsStudents(_textButton.text);
        }
        else
        {
            //NO APRETADO
            _imgEye.sprite = _openEye;
            ServiceLocator.Instance.GetService<IGameManager>().DeleteNotPresentsStudent(_textButton.text);
        }
    }

    //public void hideStudent()
    //{
    //    _pressed = true;
    //}

    //public void showStudent()
    //{
    //    _pressed = false;
    //}


}
