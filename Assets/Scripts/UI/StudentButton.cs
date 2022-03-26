using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StudentButton : MonoBehaviour
{
    [Header("Popup delete student")]
    public Text _textButton;
    [HideInInspector]
    public bool _deleting = false;
    [HideInInspector]
    public bool _animating = false;
    Animator _animator;

    [Header("Add student")]   
    public Student _student;
    bool _addingToTablet = false;
    bool _add = true;
    void Start()
    {
        _animator = GetComponent<Animator>();
        //Puts the background button invisible in order to not having misundertoods
         GetComponent<Image>().color = new Color(GetComponent<Image>().color.r, GetComponent<Image>().color.g, GetComponent<Image>().color.b, 0);
    }

    void Update()
    {
        if (_deleting)
        {
            if (!_animating)
            {
                _animating = true;
                _animator.SetBool("StartAnim", true);
            }
            //Showing the background just for guiding the thumb
            GetComponent<Image>().color = new Color(GetComponent<Image>().color.r, GetComponent<Image>().color.g, GetComponent<Image>().color.b, 100);
        }
        else if (!_deleting)
        {
            if (_animating)
            {
                _animating = false;
                _animator.SetBool("StartAnim", false);
            }
             GetComponent<Image>().color = new Color(GetComponent<Image>().color.r, GetComponent<Image>().color.g, GetComponent<Image>().color.b, 0);

        }
        switch (ServiceLocator.Instance.GetService<GameManager>()._gameStateServer)
        {
            case GameManager.GAME_STATE_SERVER.teamConfiguration:
                _addingToTablet = true;
                //Show the background button when adding student to tablet
                if (GetComponent<Image>().color.a == 0)
                {
                    GetComponent<Image>().color = new Color(GetComponent<Image>().color.r, GetComponent<Image>().color.g, GetComponent<Image>().color.b, 100);
                }
                break;
        }

    }

    /// <summary>Send the text to the popup that we want to delete</summary>
    public void PopupDeleteStudent()
    {
        //On MobileUI decides if this variable is true or not
        if (_deleting)
        {
            ServiceLocator.Instance.GetService<MobileUI>().PopupDeleteStudent(_textButton.text);
        }
    }

    /// <summary>Add this child to one tablet</summary>
    public void AddingToTablet()
    {
        //Check if she is on the correct screen in order to have different behaviours
        if (_addingToTablet)
        {
            ServiceLocator.Instance.GetService<UIManager>().AddRemoveChildrenToTablet(_student, _add);
        }
        _add = !_add;
    }

}
