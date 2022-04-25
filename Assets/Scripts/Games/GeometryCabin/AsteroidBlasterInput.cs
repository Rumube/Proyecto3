using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AsteroidBlasterInput : MonoBehaviour
{
    [Header("References")]
    public GameObject _gunGo;
    public GameObject _gunTarget;
    LineRenderer _lineRenderer;
    GameObject _asteroidManager;

    private Vector2 _lastShotPostion;
    private float _newAngle;
    private Vector3 _newDir;

    //Configuration
    private float _shotCooldown = 1f;
    private enum ShotType
    {
        Move,
        Static
    }
    private ShotType _shotType;

    //Flags
    bool _canShot = true;
    bool _canVibrate = true;

    // Start is called before the first frame update
    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _newAngle = 0;
        _lastShotPostion = Vector2.zero;
        //_asteroidManager = GameObject.FindGameObjectWithTag("AsteroidManager");
        if (GetComponent<AsteroidBlaster>())
            _shotType = ShotType.Move;
        else if (GetComponent<SpaceTimeCabin>())
        {
            _shotType = ShotType.Static;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (_shotType)
        {
            case ShotType.Move:
                UpdateMoveInput();
                break;
            case ShotType.Static:
                UpdateStaticInput();
                break;
            default:
                break;
        }

    }

    /// <summary>
    /// Update when the <see cref="_shotType"/> is <see cref="ShotType.Move"/>
    /// </summary>
    private void UpdateMoveInput()
    {
        if (ServiceLocator.Instance.GetService<GameManager>()._gameStateClient == GameManager.GAME_STATE_CLIENT.playing && _asteroidManager.GetComponent<AsteroidBlaster>()._finishCreateAsteroids && !GetComponent<AsteroidBlaster>()._gameFinished)
        {
            _gunGo.transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(_newAngle, Vector3.forward), 1f);
            InputController();
        }
    }

    /// <summary>
    /// Update when the <see cref="_shotType"/> is <see cref="ShotType.Static"/>
    /// </summary>
    private void UpdateStaticInput()
    {
        if (ServiceLocator.Instance.GetService<GameManager>()._gameStateClient == GameManager.GAME_STATE_CLIENT.playing && !GetComponent<SpaceTimeCabin>()._gameFinished)
        {
            InputController();
        }
    }

    /// <summary>
    /// Draw the laser to the correct target position
    /// </summary>
    /// <param name="targetPos">Last point of the laser</param>
    private void LineRendererController(Vector2 targetPos)
    {
        _lineRenderer.enabled = true;
        _lineRenderer.SetPosition(0, _gunGo.transform.position);
        _lineRenderer.SetPosition(1, new Vector3(targetPos.x, targetPos.y, -1f));
        StartCoroutine(StopLaser());
    }

    IEnumerator StopLaser()
    {
        yield return new WaitForSeconds(0.1f);
        _lineRenderer.enabled = false;
    }

    /// <summary>
    /// Detects inputs during game play.
    /// </summary>
    private void InputController()
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
    private void ShotGun(AndroidInputAdapter.Datos input)
    {
        _canShot = false;
        if (_shotType == ShotType.Move)
        {
            _lastShotPostion = Camera.main.ScreenToWorldPoint(input.pos);
            _gunTarget.transform.position = input.pos;
            MoveGun(_lastShotPostion);
        }
        else
        {
            _lastShotPostion = _gunTarget.transform.position;
        }

        LineRendererController(_lastShotPostion);
        RaycastHit2D hit = Physics2D.Raycast(_lastShotPostion, -Vector2.up);
        _gunGo.GetComponent<Animator>().SetTrigger("Shot");

        StartCoroutine(WaitShot());

        switch (_shotType)
        {
            case ShotType.Move:
                if (hit.collider != null && hit.collider.tag == "Asteroid" && _canVibrate)
                    AsteroidHit(hit.collider);
                break;
            case ShotType.Static:
                Collider2D colliders = Physics2D.OverlapCircle(_lastShotPostion, 1f);
                if(colliders != null && colliders.tag == "Asteroid" && _canVibrate)
                    AsteroidHit(colliders);
                GetComponent<SpaceTimeCabin>().CheckIfIsCorrect(colliders);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Performs the functions of hitting an asteroid.
    /// </summary>
    /// <param name="hit">Hit values, position, collider...</param>
    private void AsteroidHit(Collider2D newCollider)
    {
        newCollider.gameObject.GetComponent<Asteroid>().AsteroidShot();
        _canVibrate = false;
        StartCoroutine(WaitVibration());
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

    /// <summary>
    /// Rotate the gun using a Vector2
    /// </summary>
    /// <param name="pos">Position to rotate</param>
    public void MoveGun(Vector2 pos)
    {
        _newDir = (new Vector3(pos.x, pos.y, 0) - _gunGo.transform.position).normalized;
        _newAngle = Mathf.Atan2(_newDir.y, _newDir.x) * Mathf.Rad2Deg;
        _newAngle -= 90;
        _gunGo.transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(_newAngle, Vector3.forward), 1f);
    }
}
