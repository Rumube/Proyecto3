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
    float _newAngle;
    Vector3 _newDir;

    //Configuration
    float _shotCooldown = 1f;
    //Flags
    public bool _canShot = true;
    //References
    LineRenderer _lineRenderer;
    AsteroidBlaster _asteroidManager;

    public bool _canVibrate = true;
    // Start is called before the first frame update
    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _newAngle = 0;
        _asteroidManager = GetComponent<AsteroidBlaster>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (ServiceLocator.Instance.GetService<GameManager>()._gameStateClient == GameManager.GAME_STATE_CLIENT.playing && _asteroidManager._finishCreateAsteroids && !GetComponent<AsteroidBlaster>()._gameFinished)
        {
            _gunGo.transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(_newAngle, Vector3.forward), 1f);
            InputController();
            if (!_canShot)
                LineRendererController();
            else
                _lineRenderer.enabled = false;
        }
    }

    /// <summary>
    /// Draw the laser
    /// </summary>
    void LineRendererController()
    {
        _lineRenderer.enabled = true;
        _lineRenderer.SetPosition(0, new Vector3(0, -3.701f, -1f));
        _lineRenderer.SetPosition(1, new Vector3(_lastShotPostion.x, _lastShotPostion.y, -1f));
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

        _newDir = (new Vector3(_lastShotPostion.x, _lastShotPostion.y, 0) - _gunGo.transform.position).normalized;
        _newAngle= Mathf.Atan2(_newDir.y, _newDir.x) * Mathf.Rad2Deg;
        _newAngle -= 90;

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

    /// <summary>
    /// Waits to next shot
    /// </summary>
    IEnumerator WaitShot()
    {
        yield return new WaitForSeconds(_shotCooldown);
        _canShot = true;
    }
}
