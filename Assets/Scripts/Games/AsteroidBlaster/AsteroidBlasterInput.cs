using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AsteroidBlasterInput : MonoBehaviour
{
    public Text _prueba;

    bool _canVibrate = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        InputController();
    }

    void InputController()
    {
        AndroidInputAdapter.Datos newInput = ServiceLocator.Instance.GetService<IInput>().InputTouch();
        if (newInput.result)
        {
            RaycastHit2D hit = Physics2D.Raycast(newInput.pos, -Vector2.up);

            if(hit.collider != null)
            {
                _prueba.text = "FUNCIONA! :" + hit.collider.gameObject.name;
                if (_canVibrate)
                {
                    hit.collider.gameObject.GetComponent<Asteroid>().AsteroidShot();
                    _canVibrate = false;
                    StartCoroutine(WaitVibration());
                }
            }
        }
    }

    IEnumerator WaitVibration()
    {
        yield return new WaitForSeconds(1f);
        _canVibrate = true;
    }
}
