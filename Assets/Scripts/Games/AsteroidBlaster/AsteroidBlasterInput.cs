using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AsteroidBlasterInput : MonoBehaviour
{
    public Text _prueba;
    public GameObject _gunGo;
    public Image _gunTarget;
    Vector2 _lastShotPostion;
    //Configuration
    float _shotCooldown = 1f;
    //Flags
    public bool _canShot = true;
    //References
    LineRenderer _lineRenderer;

    public bool _canVibrate = true;
    // Start is called before the first frame update
    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(ServiceLocator.Instance.GetService<GameManager>()._gameStateClient == GameManager.GAME_STATE_CLIENT.playing)
        {
            InputController();
            if (!_canShot)
                LineRendererController();
            else
                _lineRenderer.enabled = false;
        }
    }

    void LineRendererController()
    {
        _lineRenderer.enabled = true;
        _lineRenderer.SetPosition(0, new Vector2(0, -3.701f));
        _lineRenderer.SetPosition(1, _lastShotPostion);
    }

    /// <summary>
    /// Detects inputs during game play.
    /// </summary>
    void InputController()
    {
        AndroidInputAdapter.Datos newInput = ServiceLocator.Instance.GetService<IInput>().InputTouch();
        if (newInput.result && _canShot)
        {
            ShotGun(newInput);
        }
    }

    /// <summary>
    /// Activates gun animations and actions when fired.
    /// </summary>
    /// <param name="input">Input values</param>
    void ShotGun(AndroidInputAdapter.Datos input)
    {
        _canShot = false;
        _lastShotPostion = Camera.main.ScreenToWorldPoint(input.pos);

        RaycastHit2D hit = Physics2D.Raycast(_lastShotPostion, -Vector2.up);
        _gunTarget.transform.position = input.pos;
        _gunGo.GetComponent<Animator>().SetTrigger("Shot");

        Vector3 dir = new Vector3(_lastShotPostion.x, _lastShotPostion.y, 0) - _gunGo.transform.position;
        float angle = (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg) - 90;
        _gunGo.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        StartCoroutine(WaitShot());
        if (hit.collider != null)
            AsteroidHit(hit);
    }

    /// <summary>
    /// Performs the functions of hitting an asteroid.
    /// </summary>
    /// <param name="hit">Hit values, position, collider...</param>
    void AsteroidHit(RaycastHit2D hit)
    {
        _prueba.text = "FUNCIONA! :" + hit.collider.gameObject.name;
        if (_canVibrate && hit.collider.tag == "Asteroid")
        {
            hit.collider.gameObject.GetComponent<Asteroid>().AsteroidShot();
            _canVibrate = false;
            StartCoroutine(WaitVibration());
        }
    }

    /// <summary>
    /// Vibration control.
    /// </summary>
    IEnumerator WaitVibration()
    {
        yield return new WaitForSeconds(1f);
        _canVibrate = true;
    }

    IEnumerator WaitShot()
    {
        yield return new WaitForSeconds(_shotCooldown);
        _canShot = true;
    }
}
