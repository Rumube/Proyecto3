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
    //public Animator _anim;
    private bool _starConnected = false;
    public GameObject _starAnim;
   

    // Start is called before the first frame update
    void Start()
    {

        _starAnim.gameObject.SetActive(false);      
    }

    // Update is called once per frame
    void Update()
    {

    }
   

    public bool GetStarConnected()
    {
        _starAnim.gameObject.SetActive(true);
        _starAnim.GetComponent<Animator>().Play("Star_Slected");
        return _starConnected;
    }

    public void SetStarConnected(bool value)
    {

        _starConnected = value;
    }

}
