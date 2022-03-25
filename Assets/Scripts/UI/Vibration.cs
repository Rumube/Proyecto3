using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Vibration : MonoBehaviour
{
    public bool _deleting = false;
    public bool _animating = false;
    public Text _textButton;
    public Image _highlighted;
    Animator _animator;
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_deleting)
        {
            if (!_animating)
            {
                _animating = true;
                _animator.SetBool("StartAnim",true);
            }
        }
        else
        {
            if (_animating)
            {
                _animating = false;
                _animator.SetBool("StartAnim", false);
            }
        }
    }

    public void SelectClass()
    {
        ServiceLocator.Instance.GetService<MobileUI>().QuitHighlightClass();
        _highlighted.enabled = true;
        ServiceLocator.Instance.GetService<GameManager>()._classNamedb = _textButton.text;
    }

    /// <summary>Open/close the window credits</summary>
    public void PopupDeleteClass()
    {
        if (_deleting)
        {
            ServiceLocator.Instance.GetService<MobileUI>().PopupDeleteClass(_textButton.text);
        }
    }

    public void PopupDeleteStudent()
    {
        if (_deleting)
        {
            ServiceLocator.Instance.GetService<MobileUI>().PopupDeleteStudent(_textButton.text);
        }
    }

}
