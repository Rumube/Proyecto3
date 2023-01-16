using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelescopeAddAndSubstractStars : MonoBehaviour
{
    [SerializeField]
    private int _order = 0;
    [Header("References")]
  
    public AudioClip _clipStarSelected;
    public GameObject _light;
    public Animator _anim;
    private bool _starConnected = false;
   

    // Start is called before the first frame update
    void Start()
    {

        _anim.gameObject.SetActive(false);      
    }

    // Update is called once per frame
    void Update()
    {

    }
   

    public bool GetStarConnected()
    {
        return _starConnected;
    }

    public void SetStarConnected(bool value)
    {
        _starConnected = value;
    }

}
