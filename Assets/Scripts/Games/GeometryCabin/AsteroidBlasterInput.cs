using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AsteroidBlasterInput : MonoBehaviour
{
    [Header("References")]
    public GameObject _gunTarget;
    private Vector2 _lastShotPostion;
    public GameObject _panelAppear;
    public GameObject _aimGo;

    //Configuration
    private float _shotCooldown = 0.1f;
    public enum GAME_MODE
    {
        geometry,
        spaceTime,
        addSubtraction,
        series
    }
    private GAME_MODE _gameMode;
    private enum ShotType
    {
        Move,
        Static,
        Select
    }
    private ShotType _shotType;

    //Flags
    bool _canShot = true;
    bool _canVibrate = true;

    public List<GunClass> laserList = new List<GunClass>();

    private void Start()
    {
        _panelAppear.GetComponent<Animator>().Play("Static");
    }

    // Update is called once per frame
    void Update()
    {
        if (ServiceLocator.Instance.GetService<IGameManager>().GetClientState() == IGameManager.GAME_STATE_CLIENT.playing)
        {
            switch (_shotType)
            {
                case ShotType.Move:
                    UpdateMoveInput();
                    break;
                case ShotType.Static:
                    UpdateStaticInput();
                    break;
                case ShotType.Select:
                    UpdateSelectInput();
                    break;
                default:
                    break;
            }
        }
    }
    /// <summary>
    /// Manage the inputs when <see cref="IGameManager.GAME_STATE_CLIENT.playing"/>
    /// </summary>
    private void UpdateSelectInput()
    {
        IGameManager.GAME_STATE_CLIENT gameState = ServiceLocator.Instance.GetService<IGameManager>().GetClientState();
        if (gameState == IGameManager.GAME_STATE_CLIENT.playing)
        {
            InputController();
        }
    }
    /// <summary>
    /// Update when the <see cref="_shotType"/> is <see cref="ShotType.Move"/>
    /// </summary>
    private void UpdateMoveInput()
    {
        IGameManager.GAME_STATE_CLIENT gameState = ServiceLocator.Instance.GetService<IGameManager>().GetClientState();

        if (gameState == IGameManager.GAME_STATE_CLIENT.playing && GetComponent<AsteroidBlaster>()._finishCreateAsteroids && !GetComponent<AsteroidBlaster>()._gameFinished)
        {//GEOMETRIA
            foreach (GunClass currentGun in laserList)
            {
                currentGun.gun.transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(currentGun.newAngle, Vector3.forward), 1f);
            }
            InputController();
        }
    }

    /// <summary>
    /// Update when the <see cref="_shotType"/> is <see cref="ShotType.Static"/>
    /// </summary>
    private void UpdateStaticInput()
    {
        if (ServiceLocator.Instance.GetService<IGameManager>().GetClientState() == IGameManager.GAME_STATE_CLIENT.playing && !GetComponent<SpaceTimeCabin>()._gameFinished)
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
        if (_gameMode == GAME_MODE.geometry || _gameMode == GAME_MODE.spaceTime)
        {
            foreach (GunClass currentGun in laserList)
            {
                currentGun.line.enabled = true;
                currentGun.line.SetPosition(0, currentGun.gun.transform.position);
                currentGun.line.SetPosition(1, new Vector3(targetPos.x, targetPos.y, -1f));
            }

            StartCoroutine(StopLaser());
        }
    }
    /// <summary>
    /// Stop the line laser line renderer
    /// </summary>
    /// <returns></returns>
    IEnumerator StopLaser()
    {
        yield return new WaitForSeconds(0.1f);
        foreach (GunClass currentGun in laserList)
        {
            currentGun.line.enabled = false;
        }
    }

    /// <summary>
    /// Detects inputs during game play.
    /// </summary>
    private void InputController()
    {
        if (ServiceLocator.Instance.GetService<IGameManager>().GetClientState() == IGameManager.GAME_STATE_CLIENT.playing)
        {
            AndroidInputAdapter.Datos newInput = ServiceLocator.Instance.GetService<IInput>().InputTouch();
            if (newInput.result && _canShot)
            {
                ShotGun(newInput);
            }
        }
    }

    /// <summary>
    /// Activates gun animations and actions when fired.
    /// </summary>
    /// <param name="input">Input values</param>
    private void ShotGun(AndroidInputAdapter.Datos input)
    {
        _canShot = false;
        _lastShotPostion = _gunTarget.transform.position;

        if (_shotType != ShotType.Static)
        {
            _lastShotPostion = Camera.main.ScreenToWorldPoint(input.pos);
            _gunTarget.transform.position = _lastShotPostion;
        }
        if (_shotType == ShotType.Move || _shotType == ShotType.Static)
        {
            MoveGun(_lastShotPostion);
        }

        LineRendererController(_lastShotPostion);

        RaycastHit2D hit = Physics2D.Raycast(_lastShotPostion, -Vector2.up, Mathf.Infinity);

        switch (_gameMode)
        {
            case GAME_MODE.geometry:
            case GAME_MODE.spaceTime:
                _aimGo.GetComponent<Animator>().Play("RedFrameShoot_animation");
                foreach (GunClass currentGun in laserList)
                {
                    currentGun.gun.transform.GetChild(0).GetComponentInChildren<Animator>().SetTrigger("Shot");
                }
                break;
            case GAME_MODE.addSubtraction:
            case GAME_MODE.series:
                if (hit.transform.gameObject.name == "Asteroid")
                {
                    hit.transform.gameObject.GetComponent<PickableAsteroid>().SelectAsteroid();
                }
                break;
            default:
                break;
        }

        StartCoroutine(WaitShot());
        switch (_shotType)
        {
            case ShotType.Move:

                if (hit.collider != null && hit.collider.tag == "Border")
                {
                    foreach (GunClass currentGun in laserList)
                    {
                        currentGun.line.enabled = false;
                    }
                }
                else if (hit.collider != null && hit.collider.tag == "Asteroid" && _canVibrate)
                {
                    AsteroidHit(hit.collider);

                }
                break;
            case ShotType.Static:
                Collider2D[] colliders = Physics2D.OverlapCircleAll(_lastShotPostion, 1f);
                foreach (Collider2D currentCollider in colliders)
                {
                    if (colliders != null && currentCollider.tag == "Asteroid")
                        AsteroidHit(currentCollider);
                    GetComponent<SpaceTimeCabin>().CheckIfIsCorrect(currentCollider);
                }
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
        yield return new WaitForSeconds(_shotCooldown);
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

        for (int i = 0; i < laserList.Count; i++)
        {
            laserList[i].newDir = ((new Vector2(pos.x, pos.y) - (Vector2)laserList[i].gun.transform.position).normalized);
            laserList[i].newAngle = (Mathf.Atan2(laserList[i].newDir.y, laserList[i].newDir.x) * Mathf.Rad2Deg);
            laserList[i].newAngle -= 90;
            laserList[i].gun.transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(laserList[i].newAngle, Vector3.forward), 1f);
        }
    }
    /// <summary>
    /// Initialize the values using <see cref="_gameMode"/>
    /// </summary>
    /// <param name="mode">The <see cref="GAME_MODE"/></param>
    public void SetGameMode(GAME_MODE mode)
    {
        _gameMode = mode;
        _lastShotPostion = Vector2.zero;
        _shotCooldown = 0.5f;
        switch (_gameMode)
        {
            case GAME_MODE.geometry:
                _shotType = ShotType.Move;
                break;
            case GAME_MODE.addSubtraction:
            case GAME_MODE.series:
                _shotType = ShotType.Select;
                break;
            case GAME_MODE.spaceTime:
                _shotType = ShotType.Static;
                _shotCooldown = 1f;
                break;
            default:
                break;
        }
    }
}
