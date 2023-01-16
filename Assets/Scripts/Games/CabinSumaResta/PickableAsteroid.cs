using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableAsteroid : MonoBehaviour
{
    [Header("References")]
    public List<Sprite> _spriteLsit = new List<Sprite>();
    public GameObject _selectIcon;
    public Animator _anim;
    public Animator _asteroidAnim;
    [Header("State")]
    private bool _selected = false;
    // Start is called before the first frame update
    void Start()
    {
        _asteroidAnim = GetComponent<Animator>();
        AnimatorStateInfo state = _asteroidAnim.GetCurrentAnimatorStateInfo(0);
        _asteroidAnim.Play(state.fullPathHash, -1, Random.Range(0f,1f));
        SetSprite();
        transform.Rotate(0.0f, 0.0f, Random.Range(0.0f, 360.0f));
        transform.localScale = Vector3.one * Random.Range(0.7f, 1f);
        _selectIcon.SetActive(_selected);
        if (_selected)
        {
            _anim.Play("RedFrameSelect_animation");
        }
    }


    private void SetSprite()
    {
        GetComponent<SpriteRenderer>().sprite = _spriteLsit[Random.Range(0, _spriteLsit.Count - 1)];
    }

    public void SelectAsteroid()
    {
        _selected = !_selected;

        if (_selected)
        {
            _selectIcon.SetActive(true);
            _anim.Play("RedFrameSelect_animation");
        }
        else{
            _anim.Play("RedFrameDeselect_animation");
            StartCoroutine(DeselectAnimController());
        }
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
}
