using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StudentClassButton : MonoBehaviour
{
    public bool _deleting = false;
    public bool _animating = false;
    public Text _textButton;
    public Image _highlighted;
    Animator _animator;

    [Header("Add student")]
    public bool _addingToTablet = false;
    public bool _studentButton;
    public Student _student;
    bool _add = true;
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        if (_studentButton)
        {
            GetComponent<Image>().color = new Color(GetComponent<Image>().color.r, GetComponent<Image>().color.g, GetComponent<Image>().color.b, 0);

        }
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
            if (_studentButton)
            {
                //Showing a little bit the background just for guiding the thumb
                GetComponent<Image>().color = new Color(GetComponent<Image>().color.r, GetComponent<Image>().color.g, GetComponent<Image>().color.b,100) ;
            }
        }
        else if(!_deleting)
        {
            if (_animating)
            {
                _animating = false;
                _animator.SetBool("StartAnim", false);
            }
            if (_studentButton)
            {
                GetComponent<Image>().color = new Color(GetComponent<Image>().color.r, GetComponent<Image>().color.g, GetComponent<Image>().color.b, 0);
            }
        }

    }

    public void SelectClass()
    {
        ServiceLocator.Instance.GetService<MobileUI>().QuitHighlightClass();
        ServiceLocator.Instance.GetService<MobileUI>().ActivateContinueMainMenu();
        _highlighted.gameObject.SetActive(true);
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

    public void AddingToTablet()
    {        
        if (_addingToTablet)
        {
            ServiceLocator.Instance.GetService<GameManager>().AddRemoveChildrenToTablet(_student,_add);
        }
        _add = !_add;
    }

}
