using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StudentButton : MonoBehaviour
{
    [Header("Popup delete student")]
    public TextMeshProUGUI _textButton;
    [HideInInspector]
    public bool _deleting = false;
    [HideInInspector]
    public bool _animating = false;
    Animator _animator;

    [Header("Add student")]
    public GameObject _background;
    public Student _student;
    public Sprite _backgroundButton;
    public Sprite _backgroundButtonSelected;
    public Image _highlighted;
    bool _addingToTablet = false;
    public bool _add = true;
    public int _selectedTablet = -1;
    bool _selected = false;
    public Image _sprite;
    public List<Sprite> _colorTeams;

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
                _background.SetActive(false);
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
                _background.SetActive(true);
                _animating = false;
                _animator.SetBool("StartAnim", false);
            }
            GetComponent<Image>().color = new Color(GetComponent<Image>().color.r, GetComponent<Image>().color.g, GetComponent<Image>().color.b, 0);

        }
        switch (ServiceLocator.Instance.GetService<IGameManager>().GetServerState())
        {
            case IGameManager.GAME_STATE_SERVER.teamConfiguration:
                _addingToTablet = true;
                //Show the background button when adding student to tablet
                if (GetComponent<Image>().color.a == 0)
                {
                    GetComponent<Image>().color = new Color(GetComponent<Image>().color.r, GetComponent<Image>().color.g, GetComponent<Image>().color.b, 100);
                }
                if (ServiceLocator.Instance.GetService<INetworkManager>().GetSelectedTablet() == _selectedTablet && !_highlighted.gameObject.activeInHierarchy)
                {
                    _highlighted.gameObject.SetActive(true);
                }
                else if (ServiceLocator.Instance.GetService<INetworkManager>().GetSelectedTablet() != _selectedTablet && _highlighted.gameObject.activeInHierarchy)
                {
                    _highlighted.gameObject.SetActive(false);
                }
                if (_selected)
                {
                    GetComponent<Image>().sprite = _backgroundButtonSelected;
                }
                else
                {
                    GetComponent<Image>().sprite = _backgroundButton;
                }
                break;
        }

        if (_selectedTablet != -1)
        {
            _sprite.sprite = _colorTeams[_selectedTablet - 1];
        }
        else
        {
            _sprite.sprite = _colorTeams[6];
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
        if (_addingToTablet && (ServiceLocator.Instance.GetService<INetworkManager>().GetSelectedTablet() == _selectedTablet || _selectedTablet == -1))
        {
            if (_add)
            {
                //Don't do anything if the tablet already has 12 students
                if (ServiceLocator.Instance.GetService<INetworkManager>().GetStudentsToTablets()[ServiceLocator.Instance.GetService<INetworkManager>().GetSelectedTablet() - 1]._students.Count < 12)
                {
                    ServiceLocator.Instance.GetService<INetworkManager>().AddRemoveChildrenToTablet(_student, true);
                    _selectedTablet = ServiceLocator.Instance.GetService<INetworkManager>().GetSelectedTablet();
                    _highlighted.gameObject.SetActive(true);
                    _selected = true;
                    _add = !_add;
                }
            }
            else
            {
                ServiceLocator.Instance.GetService<INetworkManager>().AddRemoveChildrenToTablet(_student, false);
                _add = !_add;
                _selectedTablet = -1;
                _highlighted.gameObject.SetActive(false);
                _selected = false;
            }
            ServiceLocator.Instance.GetService<MobileUI>().ContinueButtonAddStudent(ServiceLocator.Instance.GetService<INetworkManager>().CheckIfTabletsHasStudents());
            ServiceLocator.Instance.GetService<MobileUI>().UpdateNumberMininautas();
        }
    }
}
