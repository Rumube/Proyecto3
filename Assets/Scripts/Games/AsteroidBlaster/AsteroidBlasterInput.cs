using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AsteroidBlasterInput : MonoBehaviour
{
    public Text _prueba;
    public GameObject _gunGo;
    public Image _gunTarget;

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
            ShotGun(newInput);
        }
    }

    void ShotGun(AndroidInputAdapter.Datos input)
    {
        Vector2 inputPos = Camera.main.ScreenToWorldPoint(input.pos);

        RaycastHit2D hit = Physics2D.Raycast(inputPos, -Vector2.up);
        _gunTarget.transform.position = input.pos;
        _gunGo.GetComponent<Animator>().SetTrigger("Shot");

        Vector3 dir = new Vector3(inputPos.x, inputPos.y, 0) - _gunGo.transform.position;
        float angle = (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg) - 90;
        _gunGo.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if (hit.collider != null)
            AsteroidHit(hit);
    }

    void AsteroidHit(RaycastHit2D hit)
    {
        _prueba.text = "FUNCIONA! :" + hit.collider.gameObject.name;
        if (_canVibrate)
        {
            hit.collider.gameObject.GetComponent<Asteroid>().AsteroidShot();
            _canVibrate = false;
            StartCoroutine(WaitVibration());
        }
    }

    IEnumerator WaitVibration()
    {
        yield return new WaitForSeconds(1f);
        _canVibrate = true;
    }
}
