using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableAsteroid : MonoBehaviour
{
    [Header("References")]
    public List<Sprite> _spriteLsit = new List<Sprite>();
    public GameObject _selectIcon;
    [Header("State")]
    private bool _selected = false;
    // Start is called before the first frame update
    void Start()
    {
        SetSprite();
        transform.Rotate(0.0f, 0.0f, Random.Range(0.0f, 360.0f));
        transform.localScale = Vector3.one * Random.Range(0.7f, 1f);

    }

    // Update is called once per frame
    void Update()
    {
        _selectIcon.SetActive(_selected);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("Seleccionado");
    }

    private void SetSprite()
    {
        GetComponent<SpriteRenderer>().sprite = _spriteLsit[Random.Range(0, _spriteLsit.Count - 1)];
    }

    public void SelectAsteroid()
    {
        _selected = !_selected;
    }
    public bool GetSelected()
    {
        return _selected;
    }
}
