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
    /// <summary>
    /// Activates the animation and returns the connection status.
    /// </summary>
    /// <returns><see cref="_starConnected"/></returns>
    public bool GetStarConnected()
    {
        _starAnim.gameObject.SetActive(true);
        _starAnim.GetComponent<Animator>().Play("Star_Slected");
        return _starConnected;
    }
    /// <summary>
    /// Assigns the value of <see cref="_starConnected"/> with the given value
    /// </summary>
    /// <param name="value">New value</param>
    public void SetStarConnected(bool value)
    {

        _starConnected = value;
    }

}
