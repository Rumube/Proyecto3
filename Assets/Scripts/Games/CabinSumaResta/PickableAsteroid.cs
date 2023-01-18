using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PickableAsteroid : MonoBehaviour
{
    [Header("References")]
    public List<Sprite> _spriteLsit = new List<Sprite>();
    public List<Sprite> _spriteOrderList = new List<Sprite>();
    public GameObject _brokenAsteroid;
    public GameObject _selectIcon;
    public SpriteRenderer _iconSprite;
    public Animator _anim;
    public Animator _asteroidAnim;
    private CabinSeries _gm;
    [Header("Text")]
    //public TMP_Text _orderText;
    //public Canvas _canvas;

    [Header("State")]
    private bool _selected = false;
    public int _positionInOrder = -10;
    public int _playerOrder = -10;
    // Start is called before the first frame update
    void Start()
    {
        _asteroidAnim = GetComponent<Animator>();
        AnimatorStateInfo state = _asteroidAnim.GetCurrentAnimatorStateInfo(0);
        _asteroidAnim.Play(state.fullPathHash, -1, Random.Range(0f, 1f));
        if(!_gm)
        {
            SetSprite();
            transform.localScale = Vector3.one * Random.Range(0.7f, 1f);
            transform.Rotate(0.0f, 0.0f, Random.Range(0.0f, 360.0f));
        }



        _selectIcon.SetActive(_selected);
        if (_selected)
        {
            _anim.Play("RedFrameSelect_animation");
        }
    }

    /// <summary>
    /// Sets a random sprite
    /// </summary>
    private void SetSprite()
    {
        GetComponent<SpriteRenderer>().sprite = _spriteLsit[Random.Range(0, _spriteLsit.Count - 1)];
    }
    /// <summary>
    /// Sets the initial value to the asteroids
    /// in <see cref="CabinSeries"/>'s game
    /// </summary>
    /// <param name="scale">The scale multiplier</param>
    /// <param name="sprite">The used sprite</param>
    /// <param name="position">The position in the game order</param>
    /// <param name="gm">The <see cref="CabinSeries"/></param>
    public void SetValues(float scale, int sprite, int position, CabinSeries gm)
    {
        _gm = gm;
        transform.localScale = Vector3.one * scale;
        GetComponent<SpriteRenderer>().sprite = _spriteLsit[sprite];
        _positionInOrder = position;
    }
    /// <summary>
    /// Activates or deactivates the asteroids and
    /// controlls que red frame
    /// </summary>
    public void SelectAsteroid()
    {
        _selected = !_selected;

        if (_selected)
        {
            _selectIcon.SetActive(true);
            _anim.Play("RedFrameSelect_animation");
            if (_gm)
            {
                _gm.AddAsteroidToPlayerOrder(gameObject);
            }
        }
        else
        {
            _anim.Play("RedFrameDeselect_animation");
            StartCoroutine(DeselectAnimController());
            if (_gm)
            {
                _gm.RemoveAsteroidToPlayerOrder(gameObject);
            }
        }
    }
    /// <summary>
    /// Change the visual position text
    /// </summary>
    /// <param name="position">The new position</param>
    public void UpdateOrderText(int position)
    {
        _iconSprite.sprite = _spriteOrderList[position];
        _playerOrder = position;
        //_orderText.text = position.ToString();
    }
    /// <summary>
    /// Activates the RedFrameShoot_animation
    /// </summary>
    public IEnumerator BrokenAsteroid()
    {
        _brokenAsteroid.SetActive(true);
        _anim.Play("RedFrameShoot_animation");
        yield return new WaitForSeconds(0.3f);
        SelectAsteroid();
    }
    private IEnumerator DeselectAnimController()
    {
        yield return new WaitForSeconds(0.5f);
        _selectIcon.SetActive(false);
    }
    public bool GetSelected()
    {
        return _selected;
    }

    public int GetPositionInOrder()
    {
        return _positionInOrder;
    }
    public int GetPlayerPositionOrder()
    {
        return _playerOrder;
    }
    public void SetCabinSeries(CabinSeries gm)
    {
        _gm = gm;
    }
}
