using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Vibration : MonoBehaviour
{
    public bool _deleting = false;
    public bool _animating = false;
    public Text _textButton;
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

    /// <summary>Open/close the window credits</summary>
    public void PopupDeleteClass()
    {
        ServiceLocator.Instance.GetService<MobileUI>().PopupDeleteClass(_textButton.text);
    }
}
