using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClassButton : MonoBehaviour
{
    public TextMeshProUGUI _textButton;
    public Image _highlighted;
    [HideInInspector]
    public bool _deleting = false;
    [HideInInspector]
    public bool _animating = false;
    Animator _animator;

    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    /// <summary>Make an animation of shaking while the user is trying to delete</summary>
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
        else if(!_deleting)
        {
            if (_animating)
            {
                _animating = false;
                _animator.SetBool("StartAnim", false);
            }
        }
    }
    /// <summary>Select the class with highlight and save the name on UIManager</summary>
    public void SelectClass()
    {
        ServiceLocator.Instance.GetService<MobileUI>().QuitHighlightClass();
        ServiceLocator.Instance.GetService<MobileUI>().ActivateContinueMainMenu();
        _highlighted.gameObject.SetActive(true);
        ServiceLocator.Instance.GetService<UIManager>()._classNamedb = _textButton.text;
    }

    /// <summary>Send class name for checking twice in delete</summary>
    public void PopupDeleteClass()
    {
        //On MobileUI decides if this variable is true or not
        if (_deleting)
        {
            ServiceLocator.Instance.GetService<MobileUI>().PopupDeleteClass(_textButton.text);
        }
    }

}
